using System.Xml;
using System.IO.Pipes;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    //实际地块奇数列还要偏移半个单位，所以前往周围六个地块所需代价相同
    private const int Move_Cost = 1;
    private GridISO<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closeList;

    public Pathfinding(int width, int height)
    {
        grid = new GridISO<PathNode>(width,height,new Vector2(Settings.offsetX,Settings.offsetZ),new Vector3(0,0,0),(GridISO<PathNode> g, int x, int z) => new PathNode(g,x,z),true);
    }

    public GridISO<PathNode> GetGrid()
    {
        return grid;
    }

    private PathNode GetNode(int u, int v)
    {
        return grid.GetGridObject(u,v);
    }

    public void UpdateIsWalkable(int u, int v, bool isWalkable)
    {
        GetNode(u,v).isWalkable = isWalkable;
    }

    public List<Vector3> FindPath(Vector3 startWorldPostion, Vector3 targetWorldPosition)
    {
        Vector2Int startUV = grid.WorldToLogic(startWorldPostion);
        Vector2Int targetUV = grid.WorldToLogic(targetWorldPosition);

        List<PathNode>path = FindPath(startUV.x,startUV.y,targetUV.x,targetUV.y);
        if(path == null)
        {
            return null;
        }else{
            List<Vector3> vectorPath = new List<Vector3>();
            foreach(PathNode pathNode in path)
            {
                vectorPath.Add(grid.LogicToWorldOffsetZ(pathNode.u,pathNode.v));
                Debug.Log("pathfinding:logic->"+pathNode.u+","+pathNode.v);
                Debug.Log("pathfinding:World->"+grid.LogicToWorldOffsetZ(pathNode.u,pathNode.v));
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startU,int startV, int endU, int endV){
        PathNode startNode = grid.GetGridObject(startU,startV);
        PathNode endNode = grid.GetGridObject(endU,endV);

        openList = new List<PathNode>{startNode};
        closeList = new List<PathNode>();

        //init
        for(int u = 0; u < grid.GetWidth(); u++)
        {
            for(int v = 0; v < grid.GetHeight(); v++)
            {
                PathNode pathNode= grid.GetGridObject(u,v);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode,endNode);
        startNode.CalculateFCost();

        //
        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestCostNode(openList);
            if(endNode == currentNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if(closeList.Contains(neighbourNode)){continue;}
                if(!neighbourNode.isWalkable)
                {
                    closeList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode,neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode,endNode);
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //openlist为空证明无路线
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        foreach(var d in Utilities.dir6)
        {
            Vector2Int dir = currentNode.u%2==1? d: -1 * d;
            Vector2Int neighbourNode = new Vector2Int(currentNode.u,currentNode.v) + dir;
            if(Utilities.IsInGridRange(neighbourNode,grid.GetWidth(),grid.GetHeight()))
            {
                neighbourList.Add(GetNode(neighbourNode.x,neighbourNode.y));
            }
        }
        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int uDistance = Mathf.Abs(a.u - b.u);
        int vDistance = Mathf.Abs(a.v - b.v);
        //往四周的代价相同
        return Move_Cost * Mathf.Max(uDistance,vDistance);
        /*
        //计算正方形网格
        MOVE_DIAGONAL_COST * Mathf.Min(xDistance - yDistance) + MOVE_STRAIGHT_COST * Mathf.Abs(xDistance - yDistance)
        */
    }

    //TODO转移至服务器再进行优化
    private PathNode GetLowestCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFcostNode = pathNodeList[0];

        for(int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestFcostNode.fCost)
            {
                lowestFcostNode = pathNodeList[i];
            }
        }
        return lowestFcostNode;
    }
}
