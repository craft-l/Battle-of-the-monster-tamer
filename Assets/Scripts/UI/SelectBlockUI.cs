using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBlockUI : MonoBehaviour
{
    public BaseBlock block;
    public Text blockDetails;//llv+type
    public Text coordinate;
    public Text blockType;
    public Text enemyNum;

    private void OnEnable()
    {
        EventHandler.BlockSelectedEvent += OnBlockSelectedEvent;
        EventHandler.DragEvent += OnDragEvent;
    }

    private void OnDragEvent()
    {
        this.gameObject.SetActive(false);
    }

    private void OnBlockSelectedEvent(BaseBlock BaseBlock)
    {
        block = BaseBlock;
        this.gameObject.SetActive(true);
        blockDetails.text = block.mLevel.ToString() + "级" + block.mBlockType.ToString();
        coordinate.text = "(" + block.x.ToString() + "," + block.z.ToString() + ")";
        blockType.text = block.mBlockType.ToString();
    }

}
