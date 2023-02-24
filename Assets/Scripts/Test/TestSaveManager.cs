using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveManager : MonoBehaviour
{      
    public GameObject gridMap; 
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log(default(GridMapGrassData));
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("TestSaveManager:data save");
            GridMapGrassData gridMapGrassData = new GridMapGrassData();
            //gridMapGrassData.GrassMapValueData = new Vector2Int[Settings.width,Settings.height];
            for(int i = 0; i<Settings.width;i++)
            {
                for(int j = 0 ; j < Settings.height; j++)
                {
                    //gridMapGrassData.GrassMapValueData[i,j] = gridMap.GetComponent<GridMapGrass>().MapValue[i,j];
                    //gridMapGrassData.GrassMapValueData.Add(gridMap.GetComponent<GridMapGrass>().MapValue[i,j]);
                    System.Random random = new System.Random(Utilities.GetRandomSeed());
                    gridMapGrassData.GrassMapValueData.Add(new Vector2Int(15+random.Next(0,15),1));
                }
            }
            //gridMapGrassData.GrassMapValueData = gridMap.GetComponent<GridMapGrass>().MapValue;
            //Debug.Log("TestSaveManager:"+gridMapGrassData.GrassMapValueData.Length);
            //Debug.Log("TestSaveManager:"+gridMap.GetComponent<GridMapGrass>().MapValue.Length);
            //Debug.Log("TestSaveManager:"+gridMapGrassData.GrassMapValueData[0,0]);
            //gridMapGrassData.GrassMapValueData.Add(new Vector2Int(0,0));
            //gridMapGrassData.GrassMapValueData.Add(new Vector2Int(1,1));
            GameManager.Instance.saveManager.SaveByJson(gridMapGrassData,"GridMapGrassData.txt");
        }
    }


}
