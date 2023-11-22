using UnityEngine;

public struct TileGroupElement 
{
    /// <summary>
    /// ���� ���� ����� 
    /// </summary>
    CurrentTileGroupState groupState;

    public CurrentTileGroupState GroupState => groupState;
    /// <summary>
    /// �ش�׷��� �����۹� ����ġ x = ������ġ  , y = ��������ġ
    /// </summary>
    Vector2Int gridPosX;

    Vector2Int gridPosY;
    
    Vector2Int gridPosZ;

    public TileGroupElement(Vector2Int posX, Vector2Int posY, Vector2Int posZ , CurrentTileGroupState groupState) 
    {
        gridPosX = posX;
        gridPosY = posY;
        gridPosZ = posZ;
        this.groupState = groupState;
        //Debug.Log($"{posX}_{posY}_{posZ}_{groupState}");
    }
}