using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 其他种类资源工厂接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseResourcesFactory<T>
{
    T GetSingleResources(string resourcePath);
}
