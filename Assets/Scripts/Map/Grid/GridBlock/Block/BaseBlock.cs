using UnityEngine;

[System.Serializable]
public class BaseBlock
{
    public int x;//坐标
    public int z;
    public int mLevel;//地块等级
    public BlockType mBlockType;//地块种类
    public Climate mClimate;//地块所处地气候状态
    public int mEnemyIndex;//Path:mLevel+mEnemyIndex
    public int mHaloIndex;//光环指示，
    public int mHasBuilding;//是否有建筑
    public int mOwnerID;//所有者ID
    public GridISO<BaseBlock> grid;

    public BaseBlock(GridISO<BaseBlock> grid, int x,int z,int level = 0,BlockType blockType = BlockType.Empty, Climate climate = Climate.Warmth,int enemyIndex = 0,int haloIndex = 0,int hasBuilding = 0,int ownerID = 0)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        this.mLevel = level;
        this.mBlockType = blockType;
        this.mClimate = climate;
        this.mEnemyIndex = enemyIndex;
        this.mHaloIndex = haloIndex;
        this.mHasBuilding = hasBuilding;
        this.mOwnerID = ownerID;
    }

}
