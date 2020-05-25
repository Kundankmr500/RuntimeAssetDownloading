using UnityEngine;
using Generic;
using System.IO;
using UnityExtension;

[RequireComponent(typeof(MeshFilter))]
public class LoadModel : MonoSingletonGeneric <LoadModel>
{
    private const string INPUT_PATH = @"Assets/OBJ-IO/Examples/Meshes/Teapot.obj";
    private const string OUTPUT_PATH = @"Assets/OBJ-IO/Examples/Meshes/Teapot_Modified.obj";


    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadObject()
    {
        //	Load the OBJ in
        var lStream = new FileStream(INPUT_PATH, FileMode.Open);
        var lOBJData = OBJLoader.LoadOBJ(lStream);
        var lMeshFilter = GetComponent<MeshFilter>();
        lMeshFilter.mesh.LoadOBJ(lOBJData);
        lStream.Close();

        lStream = null;
        lOBJData = null;

        if (File.Exists(OUTPUT_PATH))
        {
            File.Delete(OUTPUT_PATH);
        }
        lStream = new FileStream(OUTPUT_PATH, FileMode.Create);
        lOBJData = lMeshFilter.mesh.EncodeOBJ();
        OBJLoader.ExportOBJ(lOBJData, lStream);
        lStream.Close();
    }
}
