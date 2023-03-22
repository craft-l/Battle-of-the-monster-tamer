using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line2 : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public void UpdateLine(Vector3 pos,Vector3 target)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0,pos);
        lineRenderer.SetPosition(1,target);
    }
}
