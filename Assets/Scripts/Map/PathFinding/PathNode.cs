using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridISO<PathNode> grid;

    public int u;
    public int v;
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode cameFromNode;

    public PathNode(GridISO<PathNode> grid, int u,int v)
    {
        this.grid = grid;
        this.u = u;
        this.v = v;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

    public override string ToString()
    {
        return u + "," + v;
    }
}
