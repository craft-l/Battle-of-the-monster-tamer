using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager
{
    public static Vector3 MouseWorldPosition => CursorManager.GetMouseWorldPosition_Instance();//GameManager.Instance.cursorManager.GetMouseWorldPosition_Instance();
    public bool mouseClick = false;
    public bool mouseDrag = false;

    public Vector3 MousePos;
    public void CursorJudge()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MousePos = Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
            if(MousePos != Input.mousePosition)
            {
                mouseDrag = true;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(!mouseDrag)
            {
                EventHandler.CallMouseClickedEvent(Input.mousePosition);
            }
            mouseDrag = false;
        }
    }
    private static Vector3 GetMouseWorldPosition_Instance()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin ,ray.direction , Color.red);
        if(Physics.Raycast(ray, out RaycastHit raycastHit,99f,1<<LayerMask .NameToLayer ("MouseCollider")))
        {
            return raycastHit.point;
        }
        else{
            return Vector3.zero;
        }
    }

}