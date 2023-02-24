using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏控制管理，负责控制游戏的整个逻辑
/// </summary>
public class GameManager : MonoBehaviour
{
    public CursorManager cursorManager;
    public SaveManager saveManager;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        cursorManager = new CursorManager();
        saveManager = new SaveManager();
    }

    private void Update()
    {
    }
}
