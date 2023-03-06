using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏物体工厂接口
/// </summary>
public interface IBaseFactory
{
    //根据名字通过路径加载资源
    GameObject GetItem(string itemName);
    //
    void PushItem(string itemName, GameObject item);
}
