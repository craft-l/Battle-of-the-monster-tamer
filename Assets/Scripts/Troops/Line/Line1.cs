using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line1 : MonoBehaviour
{
 [SerializeField]
    private LineRenderer lineRenderer;
    private Material material;
    private Vector2 tiling;
    private Vector2 offset;
    private int nodeNum;
    private float lineLen;
    private float density = 2f;
    private int mainTexProperty;
    // 定时器
    private float timer = 0;
    // 频率间隔
    private float frequency = 0.03f;
    private float moveSpeed = 0.04f;
    void Start()
    {
        // 缓存材质实例
        material = lineRenderer.material;
        // 缓存属性id，防止下面设置属性的时候重复计算名字的哈希
        mainTexProperty = Shader.PropertyToID("_MainTex");

        tiling = new Vector2(20, 0);
        offset = new Vector2(0, 0);
        // 设置Tiling
        material.SetTextureScale(mainTexProperty, tiling);
        // 设置Offset
        material.SetTextureOffset(mainTexProperty, offset);
    }
    private void Update()
    {

        timer += Time.deltaTime;
        if(timer >= frequency)
        {
            timer = 0;
            offset -= new Vector2(moveSpeed, 0);
            material.SetTextureOffset(mainTexProperty, offset);
        }

    }

    public void SetPath(List<Vector3> path)
    {
        nodeNum = path.Count;
        lineLen =  ((lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).magnitude) * nodeNum;
        tiling = new Vector2(lineLen * density,0);
        material.SetTextureScale(mainTexProperty, tiling);
        lineRenderer.positionCount = nodeNum;
        for(int i = 0; i < nodeNum; i++)
        {
            lineRenderer.SetPosition(i,path[i]);
        }
    }
}
