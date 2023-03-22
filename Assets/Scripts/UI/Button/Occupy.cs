using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupy : MonoBehaviour
{
    public GameObject selectedBlock;
    public Vector2Int coordinate;
    public GameObject selectTroop;

    public void ShowSelectTroop()
    {
        BaseBlock block = selectedBlock.GetComponent<SelectBlockUI>().block;
        coordinate = new Vector2Int(block.x,block.z);
        selectTroop.SetActive(true);
        selectTroop.GetComponent<SelectTroopUI>().coordinate = coordinate;
        selectedBlock.SetActive(false);
    }
}
