using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinding : MonoBehaviour
{
    private Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(Settings.width,Settings.height);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = CursorManager.MouseWorldPosition;
            Vector2Int targetNode = pathfinding.GetGrid().WorldToLogic(mousePos);
            List<PathNode> path = pathfinding.FindPath(0,0,targetNode.x,targetNode.y);
            if(path != null)
            {
                for(int i = 0; i < path.Count - 1; i++)
                {
                    //Debug.Log("TestPathFinding: Path坐标"+new Vector2Int(path[i].u,path[i].v));
                    Debug.DrawLine(Utilities.LogicToWorldSkewed(path[i].u,path[i].v) - new Vector3(0,0,Settings.offsetZ),Utilities.LogicToWorldSkewed(path[i+1].u,path[i+1].v) - new Vector3(0,0,Settings.offsetZ),Color.green,100f);
                }
            }
        }
    }
}
