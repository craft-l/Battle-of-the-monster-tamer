using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    public string path = "Prefabs/";
    public BlockData blockData = new BlockData();//数据存储
    public GridISO<BaseBlock> grid;
    public GameObject[,] blockBuilding = new GameObject[Settings.width,Settings.height];//地块上建筑和光标

    private void OnEnable()
    {
        EventHandler.MouseClickedBlockEvent += OnMouseClickBlockEvent;
    }

    private void OnMouseClickBlockEvent(Vector3 worldPos)
    {
        Vector2Int pos = Utilities.WorldToLogicSkewed(worldPos);
        Debug.Log("GridBlock: get"+worldPos);
        EventHandler.CallBlockSelectedEvent(grid.GetGridObject(pos.x,pos.y));
    }

    private void Awake()
    {
        GridInit();
        LoadDataByJson();
        BlockBuildingInit();
    }

    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SaveDataByJson();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            LoadDataByJson();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            BlockBuildingInit();
        }*/
    }

    private void GridInit()
    {
        grid = new GridISO<BaseBlock>(Settings.width,Settings.height,new Vector2(Settings.offsetX,Settings.offsetZ),new Vector3(0,0,0),(GridISO<BaseBlock> g, int x, int z) => new BaseBlock(g,x,z),false);
    }

    //根据地块信息生成建筑
    private void BlockBuildingInit()
    {
        for(int x = 0; x < Settings.width; x++)
        {
            for(int z = 0; z < Settings.height; z++)
            {
                if(grid.GetGridObject(x,z).mBlockType != BlockType.Empty)
                {
                    blockBuilding[x,z] = Instantiate(Resources.Load<GameObject>(path+grid.GetGridObject(x,z).mBlockType.ToString()+"/"+grid.GetGridObject(x,z).mLevel.ToString()),Utilities.LogicToWorldSkewedOffsetZ(x,z),Quaternion.identity,transform);      
                }
            }
        }
    }

    private void SaveDataByJson()
    {
        for(int x = 0; x < Settings.width; x++)
        {
            for(int z = 0; z < Settings.height; z++)
            {
                blockData.BlockDataList.Add(grid.GetGridObject(x,z));
            }
        }
        GameManager.Instance.saveManager.SaveByJson(blockData,"BlockData.txt");
    }

    private void LoadDataByJson()
    {
        blockData = GameManager.Instance.saveManager.LoadFromJson<BlockData>("BlockData.txt");
        for(int x = 0; x < Settings.width; x++)
        {
            for(int z = 0; z < Settings.height; z++)
            {
                int index = x * Settings.height + z;
                grid.SetValue(x,z,blockData.BlockDataList[index]);
            }
        }
    }
}
