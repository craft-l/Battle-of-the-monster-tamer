using UnityEngine;

/// <summary>
/// 游戏控制管理，负责控制游戏的整个逻辑
/// </summary>
public class GameManager : MonoBehaviour
{
    public CursorManager cursorManager;
    public SaveManager saveManager;
    public AudioSourceManager audioSourceManager;
    public FactoryManager factoryManager;
    public PlayerManager playerManager;

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
        audioSourceManager = new AudioSourceManager();
        factoryManager = new FactoryManager();
        playerManager = new PlayerManager();
    }

    private void Update()
    {
    }

    public GameObject CreateItem(GameObject item, Vector3 pos = default(Vector3), Quaternion quaternion = default(Quaternion))
    {
        GameObject go = Instantiate(item,pos,quaternion);
        return go;
    }

    public Sprite GetSprite(string resourcePath)
    {
        return factoryManager.spriteFactory.GetSingleResources(resourcePath);
    }

    public AudioClip GetAudioClip(string resourcePath)
    {
        return factoryManager.audioClipFactory.GetSingleResources(resourcePath);
    }

    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return  factoryManager.runtimeAnimatorControllerFactory.GetSingleResources(resourcePath);
    }

    //通过GameFactory获取物体
    public GameObject GetGameObjectResource(FactoryType factoryType, string resourcePath)
    {
        return factoryManager.factoryDict[factoryType].GetItem(resourcePath);
    }

    //将游戏物体放回对象池
    public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath,GameObject item)
    {
        factoryManager.factoryDict[factoryType].PushItem(resourcePath,item);
    }
}
