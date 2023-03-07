using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager
{
    public UIFacade mUIFacade;
    public Dictionary<string, GameObject> currentScenePanelDict;
    private GameManager mGameManager;

    public UIManager()
    {
        mGameManager = GameManager.Instance;
        currentScenePanelDict = new Dictionary<string, GameObject>();
        mUIFacade = new UIFacade(this);
        //mUIFacade.currentSceneState = new StartLoadSceneState(mUIFacade);
    }

    //将UIPanel放回工厂
    private void PushUIPanel(string uiPanelName,GameObject uiPanelGo)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory,uiPanelName, uiPanelGo);
    }

    //清空字典
    public void ClearDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            PushUIPanel(item.Value.name.Substring(0,item.Value.name.Length-7),item.Value);
        }

        currentScenePanelDict.Clear();
    }
}
