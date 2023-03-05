using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool canMove = true;
    private Vector3 startPos;
    private Vector3 targetPos;
    private int curPathIndex;
    private float moveTime = 1;//移动一格所需时间
    private float startTime;//出发时间
    private float arriveTime;//到达时间
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
        HandleMovement();
    }

    public void HandleMovement()
    {
        if(pathList != null)
        {
            Vector3 nextPos = pathList[curPathIndex];
            float distance = Vector3.Distance(GetPos(),nextPos);
            startTime += Time.deltaTime;
            if(distance > 0.1f)//为确保时间准确，服务端的移动实现将改为用时间判断而不是距离判断
            {
                Vector3 moveDir = (nextPos - GetPos()).normalized;

                transform.position = Vector3Lerp(startPos,nextPos,1 - (arriveTime-startTime)/moveTime);
                
            }
            else
            {
                startPos = pathList[curPathIndex];
                curPathIndex++;
                arriveTime = startTime + moveTime;
                if(curPathIndex >= pathList.Count)
                {
                    StopMoving();
                }
            }
        }
        else{
            //动画机
        }
    }

    public void SetTargetPos(Vector3 target)
    {
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
}
