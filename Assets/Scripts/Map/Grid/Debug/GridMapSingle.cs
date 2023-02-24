using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class GridMapSingle: MonoBehaviour
{
    public Material material;
    public Vector2 uv00 = Vector2.zero;
    public Vector2 uv11 = Vector2.zero;
    Vector3[] vertices = new Vector3[4];
    Vector2[] uv = new Vector2[4];
    int[] triangles = new int[6];

    private void Start()
    {
        CreatGridMap();
    }
    private void Update()
    {
        CreatGridMap();
    }

    private void CreatGridMap()
    {
        Mesh mesh = new Mesh();

        vertices[0] = new Vector3(0,0,0);
        vertices[1] = new Vector3(Settings.offsetX,0,-Settings.offsetZ);
        vertices[2] = new Vector3(0,0,-(Settings.offsetZ)*2);
        vertices[3] = new Vector3(-Settings.offsetX,0,-Settings.offsetZ);
            
        //纹理坐标左下角为原点，右上角为（1，1）
        /*
        uv[0] = new Vector2(0,0);
        uv[1] = new Vector2(1,0);
        uv[2] = new Vector2(1,1);
        uv[3] = new Vector2(0,1);*/
        /*
        uv[0] = new Vector2(uv00.x,uv00.y);
        uv[1] = new Vector2(uv11.x,uv00.y);
        uv[2] = new Vector2(uv11.x,uv11.y);
        uv[3] = new Vector2(uv00.x,uv11.y);
            */
        uv[0] = new Vector2(uv00.x,uv11.y);
        uv[1] = new Vector2(uv11.x,uv11.y);
        uv[2] = new Vector2(uv11.x,uv00.y);
        uv[3] = new Vector2(uv00.x,uv00.y);
        
        triangles[0] = 3;//012表示vertices的索引
        triangles[1] = 0;
        triangles[2] = 2;
            
        triangles[3] = 0;//第二个三角型
        triangles[4] = 1;
        triangles[5] = 2;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;
       
    }
    void UpdateUV()
    {
        Mesh mesh = new Mesh();
        uv[0] = new Vector2(uv00.x,uv00.y);
        uv[1] = new Vector2(uv11.x,uv00.y);
        uv[2] = new Vector2(uv11.x,uv11.y);
        uv[3] = new Vector2(uv00.x,uv11.y);
        mesh.uv = uv;
    }

}
