using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    public static event Action<Vector3> MouseClickedBlockEvent;
    public static void CallMouseClickedBlockEvent(Vector3 worldPos)
    {
        MouseClickedBlockEvent?.Invoke(worldPos);
    }

    public static event Action<BaseBlock> BlockSelectedEvent;
    public static void CallBlockSelectedEvent(BaseBlock baseBlock)
    {
        BlockSelectedEvent?.Invoke(baseBlock);
    }

    public static event Action DragEvent;
    public static void CallDragEvent()
    {
        DragEvent?.Invoke();
    }
}