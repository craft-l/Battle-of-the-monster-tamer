using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    Vector3 pastPos;
    float mouseScrollWheel => -1*Input.GetAxis("Mouse ScrollWheel");
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            pastPos = transform.position;
        }
        GameManager.Instance.cursorManager.CursorJudge();
        if(GameManager.Instance.cursorManager.mouseDrag)
        {
            //Debug.Log("MouseDrag");
            Vector3 moreDir = (GameManager.Instance.cursorManager.MousePos-Input.mousePosition)*moveSpeed;
            transform.position = new Vector3(pastPos.x+moreDir.x, pastPos.y,pastPos.z+moreDir.y);
        }
        if(mouseScrollWheel!=0)
        {
            if(!((transform.position.y>12&&mouseScrollWheel>0)||(transform.position.y<8&&mouseScrollWheel<0)))
            {
                transform.position += new Vector3(0,mouseScrollWheel*5,0);
                transform.localRotation = transform.localRotation * Quaternion.Euler(mouseScrollWheel*15, 0, 0);
            }
        }
    }
}
