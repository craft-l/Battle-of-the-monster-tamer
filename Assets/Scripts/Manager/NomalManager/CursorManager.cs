using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CursorManager : BaseManager
{
    public static Vector3 MouseWorldPosition => CursorManager.GetMouseWorldPosition_Instance();//GameManager.Instance.cursorManager.GetMouseWorldPosition_Instance();
    public bool mouseClick = false;
    public bool mouseDrag = false;
    private GraphicRaycaster graphicRaycasterScreenSize;
    private GraphicRaycaster graphicRaycasterConstantSize;
    private GraphicRaycaster graphicRaycasterWorld;
    private EventSystem eventSystem;

    public Vector3 MousePos;
    public bool clickUI;
    public override void OnInit()
    {
        base.OnInit();
        graphicRaycasterScreenSize = GameObject.Find("CanvasScreenSize").GetComponent<GraphicRaycaster>();
        graphicRaycasterConstantSize = GameObject.Find("CanvasConstantSize").GetComponent<GraphicRaycaster>();
        graphicRaycasterWorld = GameObject.Find("WorldCanvas").GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void CursorJudge()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MousePos = Input.mousePosition;
            clickUI = CheckGuiRaycastObjects()? true:false;
        }
        if(Input.GetMouseButton(0))//&&!CheckGuiRaycastObjects())
        {
            if(MousePos != Input.mousePosition)
            {
                mouseDrag = true;
                EventHandler.CallDragEvent();
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(!mouseDrag&&!CheckGuiRaycastObjects()&&!clickUI&&MouseWorldPosition != new Vector3(-1,0,-1))
            {
                //Debug.Log("cursorMge:call");
                EventHandler.CallMouseClickedBlockEvent(MouseWorldPosition);
            }
            mouseDrag = false;
        }
    }

    bool CheckGuiRaycastObjects()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list1 = new List<RaycastResult>();
        List<RaycastResult> list2 = new List<RaycastResult>();
        List<RaycastResult> list3 = new List<RaycastResult>();
        graphicRaycasterScreenSize.GetComponent<GraphicRaycaster>().Raycast(eventData, list1);
        graphicRaycasterConstantSize.GetComponent<GraphicRaycaster>().Raycast(eventData, list2);
        graphicRaycasterWorld.GetComponent<GraphicRaycaster>().Raycast(eventData, list3);
        return (list1.Count > 0)||(list2.Count >0)||(list3.Count > 0);
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
            return new Vector3(-1,0,-1);
        }
    }

}