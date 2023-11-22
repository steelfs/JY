using UnityEngine;

public struct TileGroupElement 
{
    /// <summary>
    /// 지역 상태 저장용 
    /// </summary>
    CurrentTileGroupState groupState;

    public CurrentTileGroupState GroupState => groupState;
    /// <summary>
    /// 해당그룹의 셀시작및 끝위치 x = 시작위치  , y = 끝나는위치
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