using UnityEngine;
using System;

public class GridISO <TGridObject>{

    //uv逻辑坐标，xz世界坐标
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {//利用EventArgs传递数据
        public int u;
        public int v;
    }

    private int width;
    private int height;
    private Vector2 offset;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    bool showDebug;
    public GridISO(int width, int height, Vector2 offset, Vector3 originPosition, Func<GridISO<TGridObject>, int,int , TGridObject> createGridObject, bool showDebug = false) {
        this.width = width;
        this.height = height;
        this.offset = offset;
        this.originPosition = originPosition;
        this.showDebug = showDebug;

        gridArray = new TGridObject[width, height];

        for(int x= 0; x < gridArray.GetLength(0); x++)
        {
            for(int z = 0; z < gridArray.GetLength(1); z++){
                gridArray[x,z] = createGridObject(this, x, z);
            }
        }


        if(showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width,height];

            for(int x = 0; x < gridArray.GetLength(0); x++)
            {
                for(int z = 0; z < gridArray.GetLength(1); z++)
                {
                    debugTextArray[x,z] = Utilities.CreateWorldText(gridArray[x,z]?.ToString(),null,LogicToWorldOffsetZ(x,z));
                    Debug.DrawLine(LogicToWorld(x,z),LogicToWorld(x,z)-new Vector3(Settings.offsetX,0,Settings.offsetZ),Color.white,100f);
                    Debug.DrawLine(LogicToWorld(x,z),LogicToWorld(x,z)+new Vector3(Settings.offsetX,0,-Settings.offsetZ),Color.white,100f);
                }
            }
            Debug.DrawLine(LogicToWorld(0,height),LogicToWorld(width,height),Color.white,100f);
            Debug.DrawLine(LogicToWorld(width,0),LogicToWorld(width,height),Color.white,100f);
        }
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public Vector2 GetOffset() {
        return offset;
    }

    public TGridObject GetGridObject(int u, int v)
    {
        return gridArray[u,v];
    }

    public void SetValue(int u, int v, TGridObject value) {
        if (u >= 0 && v >= 0 && u < width && v < height) {
            gridArray[u, v] = value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { u = u, v = v });
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value) {
        int u, v;
        Utilities.WorldToLogic(worldPosition, out u, out v);
        SetValue(u, v, value);
    }

    public TGridObject GetValue(int u, int v) {
        if (u >= 0 && v >= 0 && u < width && v < height) {
            return gridArray[u, v];
        } else {
            return default(TGridObject);//返回类型的默认值
        }
    }

    public TGridObject GetValue(Vector3 worldPosition) {
        int u, v;
        Utilities.WorldToLogic(worldPosition, out u, out v);
        return GetValue(u, v);
    }

    public Vector3 LogicToWorld(int u, int v)
    {
        return Utilities.LogicToWorldSkewed(u,v,originPosition);
    }
    public Vector3 LogicToWorldOffsetZ(int u, int v)
    {
        return Utilities.LogicToWorldSkewedOffsetZ(u,v,originPosition);
    }

    public Vector2Int WorldToLogic(Vector3 worldPos)
    {
        return Utilities.WorldToLogicSkewed(worldPos,originPosition);
    }

}
