using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.MouseClickedBlockEvent += OnMouseClickedBlockEvent;
    }

    private void OnMouseClickedBlockEvent(Vector3 pos)
    {
        this.transform.position = Utilities.WorldToWorldSkewedOffsetZ(pos);
    }



    void Update()
    {
        this.transform.localRotation = Camera.main.transform.localRotation;
    }
}
