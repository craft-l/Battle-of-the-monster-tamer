using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BaseManager
{
    GameObject MainCamera;
    public float moveSpeed = 0.1f;
    Vector3 pastPos;
    float mouseScrollWheel => -1*Input.GetAxis("Mouse ScrollWheel");

    public override void OnInit()
    {
        base.OnInit();
        MainCamera = Camera.main.gameObject;
    }
    
    public void OnUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            pastPos = MainCamera.transform.position;
        }
        if(GameManager.Instance.cursorManager.mouseDrag)
        {
            //Debug.Log("MouseDrag");
            Vector3 moreDir = (GameManager.Instance.cursorManager.MousePos-Input.mousePosition)*moveSpeed;
            MainCamera.transform.position = new Vector3(pastPos.x+moreDir.x, pastPos.y,pastPos.z+moreDir.y);
        }
        if(mouseScrollWheel!=0)
        {
            if(!((MainCamera.transform.position.y>12&&mouseScrollWheel>0)||(MainCamera.transform.position.y<8&&mouseScrollWheel<0)))
            {
                MainCamera.transform.position += new Vector3(0,mouseScrollWheel*5,0);
                MainCamera.transform.localRotation = MainCamera.transform.localRotation * Quaternion.Euler(mouseScrollWheel*15, 0, 0);
            }
        }
    }
}
