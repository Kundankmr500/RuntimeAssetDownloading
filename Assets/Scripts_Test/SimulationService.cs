using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SimulationService : MonoBehaviour {

    public List<GameObject> AllBuildingsInScene = new List<GameObject>();
    public List<GameObject> Buildings = new List<GameObject>();
    public Transform SceneObjectParent;
    public Dropdown Dropdown;

    List<Dropdown.OptionData> OptionDatas = new List<Dropdown.OptionData>();
    private List<BuildingInfo> buildingInfos = new List<BuildingInfo>();
    private const string FileName = "/scene_json.txt";



    private void Start()
    {
        AddDropDownOption();
    }


    private void AddDropDownOption()
    {
        for (int i = 0; i < Buildings.Count; i++)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = Buildings[i].name;
            OptionDatas.Add(optionData);
        }
        Dropdown.AddOptions(OptionDatas);
    }

    public void AddBuildingFromDropdown()
    {
        InstantiateBuilding(Dropdown.value, GetRandomPos(), new Vector3(-90, 0, 0));
    }


    public void SaveData()
    {
        buildingInfos.Clear();
        for (int i = 0; i < AllBuildingsInScene.Count; i++)
        {
            BuildingInfo buildingInfo = new BuildingInfo();
            buildingInfo.Position = AllBuildingsInScene[i].transform.position;
            buildingInfo.Rotation = AllBuildingsInScene[i].transform.eulerAngles;
            buildingInfo.ID = int.Parse(AllBuildingsInScene[i].name);

            buildingInfos.Add(buildingInfo);
        }

        string path = Application.streamingAssetsPath + FileName;
        string jsonString = JsonConvert.SerializeObject(buildingInfos, Formatting.Indented);

        File.WriteAllText(path, jsonString);

        Debug.Log("Scene Saved ");
    }


    private GameObject InstantiateBuilding(int index , Vector3 position, Vector3 rotation)
    {
        GameObject building = Instantiate(Buildings[index], SceneObjectParent);

        building.transform.position = position;
        building.transform.eulerAngles = rotation;
        building.name = index.ToString();
        building.transform.GetChild(0).gameObject.AddComponent(typeof(BuildingDrag));
        building.transform.GetChild(0).gameObject.AddComponent(typeof(BoxCollider));
        AllBuildingsInScene.Add(building);
        return building;
    }


    public void AddRandomBulding()
    {
        int randomIndex = Random.Range(0, Buildings.Count);
        InstantiateBuilding(randomIndex, GetRandomPos() ,new Vector3(-90, 0, 0));
        
        Debug.Log(AllBuildingsInScene.Count);
    }


    Vector3 GetRandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(-9, 9), 0, Random.Range(-8, 8));
        return randomPos;
    }


    public void ClearScene()
    {
        for (int i = 0; i < AllBuildingsInScene.Count; i++)
        {
            Destroy(AllBuildingsInScene[i]);
        }
        AllBuildingsInScene.Clear();
        buildingInfos.Clear();
    }


    private void LoadBuilding(List<BuildingInfo> buildingInfos)
    {
        for (int i = 0; i < buildingInfos.Count; i++)
        {
            InstantiateBuilding(buildingInfos[i].ID, buildingInfos[i].Position, buildingInfos[i].Rotation);

        }
    }



    public void LoadData()
    {
        string path = Application.streamingAssetsPath + FileName;

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            if (jsonString.Length > 0)
            {
                buildingInfos = JsonConvert.DeserializeObject<List<BuildingInfo>>(jsonString);
                LoadBuilding(buildingInfos);
                Debug.Log(buildingInfos.Count);
                Debug.Log("Scene loaded ");
            }
            else
            {
                Debug.Log("File is blank");
            }
        }
        else
        {
            Debug.Log("File not found in " + path);
        }
    }
}
