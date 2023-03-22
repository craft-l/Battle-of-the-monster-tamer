using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTroop : MonoBehaviour
{
    public GameObject troop;
    public GameObject selectTroopUI;
    // Update is called once per frame
    private void OnEnable()
    {
        EventHandler.BlockSelectedEvent += OnBlockSelectedEvent;
        EventHandler.DragEvent += OnDragEvent;
    }

    private void OnDragEvent()
    {
        selectTroopUI.SetActive(false);
    }

    private void OnBlockSelectedEvent(BaseBlock obj)
    {
        selectTroopUI.SetActive(false);
    }

    public void Move()
    {
        Vector2Int coordinate = selectTroopUI.GetComponent<SelectTroopUI>().coordinate;
        troop.GetComponent<Movement>().SetTargetPos(Utilities.LogicToWorldSkewedOffsetZ(coordinate.x,coordinate.y));
        selectTroopUI.SetActive(false);
    }
}
