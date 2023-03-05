using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("MoveTest: set target");
            player.GetComponent<Movement>().SetTargetPos(CursorManager.MouseWorldPosition);
        }
    }
}
