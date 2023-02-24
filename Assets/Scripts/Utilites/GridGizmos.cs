using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i = 0;i <= 100; i = i + 1)
        {
            Gizmos.DrawLine(new Vector3(-2*i,0,-i), new Vector3(200-(2*i),0,-100-i));
            Gizmos.DrawLine(new Vector3(2*i,0,-i), new Vector3(2*i-200,0,-100-i));
            //Gizmos.DrawLine(new Vector3(-i,-i,0), new Vector3(100-i,-100-i,0));
            //Gizmos.DrawLine(new Vector3(i,-i,0), new Vector3(i-100,-100-i,0));
        }
    }
}
