using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlcokSelectedCursor : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.MouseClickedBlockEvent += OnMouseClickedBlockEvent;
    }

    private void OnMouseClickedBlockEvent(Vector3 worldPos)
    {
        this.transform.position =  Utilities.WorldToWorldSkewedOffsetZ(worldPos);
    }

    void Update()
    {
        
    }
}
