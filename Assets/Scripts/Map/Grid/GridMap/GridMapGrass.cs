using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class GridMapGrass : MonoBehaviour
{
    public Vector2Int[,] MapValue =  new Vector2Int[Settings.width,Settings.height];//x为TextureID，y为是否可加值
    private void Awake()
    {
        MapValueInit();
        MapVisual();
    }
    void MapValueInit()
    {
        GridMapGrassData gridMapGrassData = new GridMapGrassData();
        gridMapGrassData = GameManager.Instance.saveManager.LoadFromJson<GridMapGrassData>("GridMapGrassData.txt");
        if(gridMapGrassData.GrassMapValueData.ToArray().Length != 0)//&&gridMapGrassData.GrassMapValueData.Length != 0)
        {
            for(int i = 0; i < Settings.width; i++)
            {
                for(int j = 0 ; j < Settings.height; j++)
                {
                    int index = i * Settings.height + j;
                    MapValue[i,j] = gridMapGrassData.GrassMapValueData[index];
                }
            }
        }else{
            Debug.Log("GridMapGrass: gridMapGrassData length == 0");
        }
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

        Vector3[] vertices;
        Vector2[] uv;
        Vector2 uv00;
        Vector2 uv11;

        MeshUtils.CreateEmptyMeshArrays(Settings.width*Settings.height,out vertices,out uv,out triangles);

        for(int u = 0; u < Settings.width; u++)
        {
            for(int v = 0; v < Settings.height; v++)
            {
                int index = u *Settings.width + v;
                GetUV(MapValue[u,v].x,out uv00,out uv11);
                MeshUtils.AddToMeshArraysIsometric(vertices,uv,triangles,index,Utilities.LogicToWorld(u,v),new Vector2(Settings.offsetX,Settings.offsetZ),uv00,uv11);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void UpdateMapVisual(Vector2Int v,bool addValue)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uv = mesh.uv;
        Vector2 uv00;
        Vector2 uv11;

        if((addValue&&MapValue[v.x,v.y].y==0)||(!addValue&&MapValue[v.x,v.y].y==1)){
            for(int i = 0; i < 4; i++)
            {
                Vector2Int vd = v + Utilities.dir4[i];
                if(Utilities.IsInGridRange(vd))
                {
                    if(addValue){
                        MapValue[vd.x,vd.y].x += (MapValue[vd.x,vd.y].x += 1 << i ) > 31 ? -10: 1 << i ;
                        if(MapValue[vd.x,vd.y].x == 15)
                        {
                            System.Random random = new System.Random(Utilities.GetRandomSeed());
                            MapValue[vd.x,vd.y].x += random.Next(0,15);
                        }
                    }
                    else
                    {
                        if(MapValue[vd.x,vd.y].x > 15)
                        {
                            MapValue[vd.x,vd.y].x = 15;
                        }
                        MapValue[vd.x,vd.y].x -= (MapValue[vd.x,vd.y].x -= 1 << i ) > -1 ?  1 << i : MapValue[vd.x,vd.y].x;
                    }
                    GetUV(MapValue[vd.x,vd.y].x,out uv00,out uv11);
                    MeshUtils.AddToMeshArraysIsometric(vertices,uv,triangles,vd.x *Settings.width + vd.y,Utilities.LogicToWorld(vd.x,vd.y),new Vector2(Settings.offsetX,Settings.offsetZ),uv00,uv11);
                }
            }
            MapValue[v.x,v.y].y = addValue? 1 : 0;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
