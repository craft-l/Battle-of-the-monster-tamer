using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryTest : MonoBehaviour
{
    private GameManager mGameManager;
    // Start is called before the first frame update
    void Start()
    {
        mGameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("FactoryTest");
            Instantiate(GetGameObjectResource(FactoryType.GameFactory,"Wood/1"),CursorManager.MouseWorldPosition,Quaternion.identity);
        }
    }

    public GameObject GetGameObjectResource(FactoryType factoryType, string resourcePath)
    {
        return mGameManager.GetGameObjectResource(factoryType,resourcePath);
    }
}
