using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class GridMapDirt : MonoBehaviour
{
    public int[,] MapValue = new int[Settings.width,Settings.height];//y为TextureID，xz为坐标

    private void Awake()
    {
        MapVisual();
    }
    void GetUV(int value,out Vector2 uv00, out Vector2 uv11)
    {
        //int x = value%4;
        uv00 = new Vector2(value/4*0.125f,(3-value%4)*0.25f);
        uv11 = uv00 + new Vector2(0.125f,0.25f);
    }
    void MapVisual()
    {
        Mesh mesh = new Mesh();
        int[] triangles;

        //MapValue = new Vector3[Settings.width,Settings.height];
        Vector3[] vertices;
        Vector2[] uv;
        Vector2 uv00 = new Vector2(0,0);
        Vector2 uv11 = new Vector2(0.5f,1);

        MeshUtils.CreateEmptyMeshArrays(Settings.width*Settings.height,out vertices,out uv,out triangles);

        for(int u = 0; u < Settings.width; u += 4)
        {
            for(int v = 0; v < Settings.height; v+=4)
            {
                int index = u *Settings.width + v;
                //GetUV(MapValue[u,v],out uv00,out uv11);
                MeshUtils.AddToMeshArraysIsometric(vertices,uv,triangles,index,Utilities.LogicToWorld(u,v),new Vector2(Settings.offsetX*4,Settings.offsetZ*4),uv00,uv11);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
