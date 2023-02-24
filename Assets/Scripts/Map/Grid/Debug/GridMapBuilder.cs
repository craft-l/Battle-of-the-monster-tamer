using UnityEngine;

public class GridMapBuilder : MonoBehaviour
{
    public GameObject gridMap;
    private int[,] valueArrary;
    private bool addValue = true;
    private void OnEnable()
    {
        EventHandler.MouseClickedEvent += OnMouseClickEvent;
    }
    void Start()
    {
    }

    void Update()
    {/*
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Utilities.GetMouseWorldPosition());
            Debug.Log("MouseWorld"+MouseWorldPosition());
            Debug.Log("Logic"+WorldToLogic(MouseWorldPosition()));
            Instantiate(gridMap,GameManager.Instance.utilities.LogicToWorld(WorldToLogic(MouseWorldPosition())),Quaternion.identity,gridMapParent.transform);
            //Debug.Log();       
        }*/
        //加减切换
        if(Input.GetKeyDown(KeyCode.A))
        {
            addValue = addValue? false: true;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("GridMapBuilder:data save");
            GridMapGrassData gridMapGrassData = new GridMapGrassData();
            //gridMapGrassData.GrassMapValueData = gridMap.GetComponent<GridMapGrass>().MapValue;
            GameManager.Instance.saveManager.SaveByJson(gridMapGrassData,"GridMapGrassDataPath");
        }
    }
    
    void OnMouseClickEvent(Vector3 pos)
    {
        //Instantiate(gridMap,GameManager.Instance.utilities.LogicToWorld(WorldToLogic(MouseWorldPosition())),Quaternion.identity,gridMapParent.transform);
        gridMap.GetComponent<GridMapGrass>().UpdateMapVisual(Utilities.WorldToLogic(CursorManager.MouseWorldPosition),addValue);
        
    }

    Vector3 MouseWorldPosition()
    {
        /*
        //用于平行摄像机的面
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosOnScreen = Input.mousePosition;
        mousePosOnScreen.z = screenPos.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
        return new Vector3(worldPos.x,0,worldPos.z);*/
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin ,ray.direction , Color.red);
        if(Physics.Raycast(ray, out RaycastHit raycastHit,99f,1<<LayerMask .NameToLayer ("MouseCollider")))
        {
            return raycastHit.point;
        }
        else{
            return Vector3.zero;
        }
    }

}
