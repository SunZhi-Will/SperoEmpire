using ProtoVoxelLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAllVoxels : MonoBehaviour {

	public void Generate()
    {
        RenderProtoVoxel[] list = GameObject.FindObjectsOfType<RenderProtoVoxel>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].NewSeed();
             list[i].RenderVoxel();
            
        }
    }
}
