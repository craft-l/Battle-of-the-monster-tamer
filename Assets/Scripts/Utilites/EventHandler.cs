using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    public static event Action<Vector3> MouseClickedEvent;
    public static void CallMouseClickedEvent(Vector3 pos)
    {
        MouseClickedEvent?.Invoke(pos);
    }
}