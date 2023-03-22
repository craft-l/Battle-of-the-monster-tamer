using System.Net.Mail;
using UnityEngine;
using System;

public class Utilities
{
    public static Vector2Int[] dir4 = {new Vector2Int(-1,0),new Vector2Int(0,0),new Vector2Int(-1,-1),new Vector2Int(0,-1)};
    public static Vector2Int[] dir6 = {new Vector2Int(-1,0),new Vector2Int(0,-1),new Vector2Int(1,0),new Vector2Int(1,1),new Vector2Int(0,1),new Vector2Int(-1,1)};
    public static Vector3 LogicToWorld(int u, int v, Vector3 origin = default(Vector3))
    {
        return new Vector3((u-v) * Settings.offsetX,0,-((u+v)*Settings.offsetZ)) + origin;
    }

    public static Vector3 LogicToWorldSkewed(int u, int v, Vector3 origin = default(Vector3))
    {
        return u%2 == 1? new Vector3((u-v-0.5f) * Settings.offsetX,0,-((u+v+0.5f)*Settings.offsetZ)) + origin:new Vector3((u-v) * Settings.offsetX,0,-((u+v)*Settings.offsetZ)) + origin;
    }
    //格子中心
    public static Vector3 LogicToWorldSkewedOffsetZ(int u, int v, Vector3 origin = default(Vector3))
    {
        return u%2 == 1? new Vector3((u-v-0.5f) * Settings.offsetX,0,-((u+v+1.5f)*Settings.offsetZ)) + origin:new Vector3((u-v) * Settings.offsetX,0,-((u+v+1)*Settings.offsetZ)) + origin;
    }

    public static Vector3 WorldToWorldSkewedOffsetZ(Vector3 pos)
    {
        Vector2Int logicPos = WorldToLogicSkewed(pos);
        return LogicToWorldSkewedOffsetZ(logicPos.x,logicPos.y);
    }

    public static Vector3 LogicToWorld(Vector2 v, Vector3 origin = default(Vector3))
    {
        return new Vector3((v.x-v.y) * Settings.offsetX,0,-((v.x+v.y)*Settings.offsetZ)) + origin;
    }
    //顶点
    public static Vector3 LogicToWorldSkewed(Vector2 v, Vector3 origin = default(Vector3))
    {
        return v.x%2 == 1? new Vector3((v.x-v.y-0.5f) * Settings.offsetX,0,-((v.x+v.y+0.5f)*Settings.offsetZ)) + origin:new Vector3((v.x-v.y) * Settings.offsetX,0,-((v.x+v.y)*Settings.offsetZ)) + origin;
    }

    public static Vector2Int WorldToLogic(Vector3 worldPos, Vector3 origin = default(Vector3))
    {
        int u = (int)((worldPos.x - origin.x)/(Settings.offsetX*2) - (worldPos.z - origin.z)/(Settings.offsetZ*2));
        int v = (int)(((worldPos.z - origin.z)/(Settings.offsetZ*2) + (worldPos.x - origin.x)/(Settings.offsetX*2))*(-1));
        return new Vector2Int(u,v);
    }

    public static Vector2Int WorldToLogicSkewed(Vector3 worldPos, Vector3 origin = default(Vector3))
    {
        int u = (int)((worldPos.x - origin.x)/(Settings.offsetX*2) - (worldPos.z - origin.z)/(Settings.offsetZ*2));
        int v = u%2 == 1? (int)(((worldPos.z - origin.z + 0.5f * Settings.offsetZ)/(Settings.offsetZ*2) + (worldPos.x - origin.x + Settings.offsetX * 0.5)/(Settings.offsetX*2))*(-1)):
                        (int)(((worldPos.z - origin.z)/(Settings.offsetZ*2) + (worldPos.x - origin.x)/(Settings.offsetX*2))*(-1));
        return new Vector2Int(u,v);
    }

    public static void WorldToLogic(Vector3 worldPos,out int u,out int v, Vector3 origin = default(Vector3))
    {
        u = (int)((worldPos.x - origin.x)/(Settings.offsetX*2) - (worldPos.z - origin.z)/(Settings.offsetZ*2));
        v = (int)(((worldPos.z - origin.z)/(Settings.offsetZ*2) + (worldPos.x - origin.x)/(Settings.offsetX*2))*(-1));
    }

    public static void WorldToLogicSkewed(Vector3 worldPos,out int u,out int v, Vector3 origin = default(Vector3))
    {
        u = (int)((worldPos.x - origin.x)/(Settings.offsetX*2) - (worldPos.z - origin.z)/(Settings.offsetZ*2));
        v = u%2 == 1? (int)(((worldPos.z - origin.z + Settings.offsetZ * 0.5f)/(Settings.offsetZ*2) + (worldPos.x - origin.x + Settings.offsetX *0.5f)/(Settings.offsetX*2))*(-1)):
                     (int)(((worldPos.z - origin.z)/(Settings.offsetZ*2) + (worldPos.x - origin.x)/(Settings.offsetX*2))*(-1));
    }

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 10, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 0) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static bool IsInGridRange(Vector2Int v,int width = Settings.width, int height = Settings.height)
    {
        return v.x<=width&&v.x>=0&&v.y<height&&v.y>=0;
    }

    public static int GetRandomSeed()//获取随机种子用于产生随机数
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
}
