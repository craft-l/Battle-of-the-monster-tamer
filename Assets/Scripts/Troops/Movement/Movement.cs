using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject Line1;
    public GameObject Line2;
    public bool canMove = true;
    public bool isMoving = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private int curPathIndex;
    private float moveTime = 1;//移动一格所需时间
    private float startTime;//出发时间
    private float arriveTime;//到达时间
    private float totalTime;
    private List<Vector3> pathList;
    private Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(Settings.width,Settings.height);
    }

    void Update()
    {
        if(canMove)
        {
            HandleMovement();
            if(isMoving)
            {
                Line1.GetComponent<Line1>().SetPath(pathList);
                Line2.GetComponent<Line2>().UpdateLine(transform.position,targetPos);
            }
        }
    }

    public void HandleMovement()
    {
        if(pathList != null)
        {
            isMoving = true;
            Vector3 nextPos = pathList[curPathIndex];
            //Debug.Log("Movement:nextPos"+nextPos);
            float distance = Vector3.Distance(GetPos(),nextPos);
            startTime += Time.deltaTime;
            if(distance > 0.1f)//为确保时间准确，服务端的移动实现将改为用时间判断而不是距离判断
            {
                Vector3 moveDir = (nextPos - GetPos()).normalized;

                transform.position = Vector3Lerp(startPos,nextPos,1 - (arriveTime-startTime)/moveTime);

            }
            else
            {
                isMoving = false;
                startPos = pathList[curPathIndex];
                curPathIndex++;
                arriveTime = startTime + moveTime;
                if(curPathIndex >= pathList.Count)
                {
                    StopMoving();
                    Line1.SetActive(false);
                    Line2.SetActive(false);
                }
            }
        }
        else{
            //动画机
        }
    }

    public List<Vector3> GetPath(Vector3 target)
    {
        return pathList;
    }

    public void SetTargetPos(Vector3 target)
    {
        if(isMoving)return;
        Line2.SetActive(true);
        startPos = GetPos();
        targetPos = target;
        pathList = pathfinding.FindPath(GetPos(),targetPos);
        curPathIndex = 0;
        startTime = 0;//获取当前时间

        arriveTime = startTime + moveTime;

        //删除初始位置
        if(pathList != null && pathList.Count > 1)
        {
            pathList.RemoveAt(0);
        }

        totalTime = pathList.Count * moveTime;
        SendTarget();
    }

    public void SetMoveTime(float time)
    {
        moveTime = time;
    }

    private Vector3 Vector3Lerp(Vector3 a, Vector3 b,float c)
    {
        float x = Mathf.Lerp(a.x,b.x,c);
        float z = Mathf.Lerp(a.z,b.z,c);
        return new Vector3(x,0,z);
    }

    private void StopMoving()
    {
        pathList = null;
    }

    private Vector3 GetPos()
    {
        return transform.position;
    }

    private void SendTarget()
    {
        Debug.Log("Movement:Netcore connect"+NetCore.connected);
        var req = new SprotoType.get_troop_target.request();
        req.troop = new SprotoType.Troop();
        req.troop.id = 1;
        req.troop.x = targetPos.x;
        req.troop.y = targetPos.y;
        req.troop.z = targetPos.z;
        NetSender.Send<Protocol.get_troop_target>(req,(data) =>
        {
            var rsp = data as SprotoType.get_troop_target.response;
            Debug.Log("Movement:rsp"+rsp);
            Debug.Log("Movement:rsp.troop_list"+rsp.troop_list);
            Debug.Log("Movement:rsp.troop_list.x"+rsp.troop_list.x);
        }
        );
    }
}
