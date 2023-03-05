using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    //逻辑坐标大小
    int width = 100;
    int height = 100;
    float offsetZ = 1;
    float offsetX = 2;

    public int x = 0;
    public int z = 0;

    public GameObject gridMap;
    public GameObject gridMapParent;
    private void Start()
    {
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Test: (u,v)-"+Utilities.WorldToLogicSkewed(CursorManager.MouseWorldPosition));
            
            Test();
        }
    }

    void Test()
    {
        //gridMap.GetComponent<GridMap>().CreatGridMap(1,2);

        //Instantiate(gridMap,LogicToWorld(x,z),Quaternion.identity,gridMapParent.transform);
        //Debug.Log(GameManager.Instance.utilities.LogicToWorld(1,2));
    }

    void CreatTileMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4*(width * height)];
        Vector2[] uv = new Vector2[4*(width * height)];
        int[] triangles = new int[6*(width * height)];

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                int index = i  * height + j;

                vertices[0 + index * 4] = LogicToWorld(i,j);
                vertices[1 + index * 4] = LogicToWorld(i+1,j);
                vertices[2 + index * 4] = LogicToWorld(i+1,j+1);
                vertices[3 + index * 4] = LogicToWorld(i,j+1);

                //纹理坐标左下角为原点，右上角为（1，1）
                uv[0 + index * 4] = new Vector2(0,0);
                uv[1 + index * 4] = new Vector2(1,0);
                uv[2 + index * 4] = new Vector2(1,1);
                uv[3 + index * 4] = new Vector2(0,1);

                triangles[0 + index * 6] = 0 + index * 4;//012表示vertices的索引
                triangles[1 + index * 6] = 1 + index * 4;
                triangles[2 + index * 6] = 2 + index * 4;
                
                triangles[3 + index * 6] = 0 + index * 4;//第二个三角型
                triangles[4 + index * 6] = 2 + index * 4;
                triangles[5 + index * 6] = 3 + index * 4;
            }
        }


        //构造mesh前最好clear，不然索引容易出错
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        GetComponent<MeshFilter>().mesh = mesh;
    }

    Vector3 LogicToWorld(int x, int z)
    {
        return new Vector3((x-z) * offsetX,0,-((x+z)*offsetZ));
    }

    Vector2 WorldToLogic()
    {

        return Vector2.zero;
    }

}
