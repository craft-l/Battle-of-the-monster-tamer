using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFacade
{
    //管理者
    private UIManager mUIManager;
    private GameManager mGameManager;
    private AudioSourceManager audioSourceManager;
    //UI面板
    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();
    //mask做渐变用
    private GameObject mask;
    private Image maskImage;
    public Transform canvasTransform;

    public UIFacade(UIManager uiManager)
    {
        mGameManager = GameManager.Instance;
        mUIManager = uiManager;
    }

    public void InitMask()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        mask = GetGameObjectResource(FactoryType.UIFactory,"Img_Mask");
    }

    //再次封装
    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }

    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }

    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }

    public GameObject GetGameObjectResource(FactoryType factoryType, string resourcePath)
    {
        return mGameManager.GetGameObjectResource(factoryType,resourcePath);
    }
    public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath,GameObject item)
    {
        mGameManager.PushGameObjectToFactory(factoryType,resourcePath,item);
    }
}
