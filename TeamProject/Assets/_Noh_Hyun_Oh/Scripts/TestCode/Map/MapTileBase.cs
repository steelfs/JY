using System;
using UnityEngine;
/// <summary>
/// 배틀 화면의 타일
/// </summary>
public class MapTileBase : MonoBehaviour, ITileBase
{
    /// <summary>
    /// 타일의 인덱스값
    /// </summary>
    int index = -1;
    public int TileIndex => index;

    /// <summary>
    /// 타일의 좌표값
    /// </summary>
    Vector3Int gridPos = Vector3Int.zero;
    public Vector3Int Tile3DPos => gridPos;

    /// <summary>
    /// 타일의 상태값
    /// </summary>
    CurrentTileState curruntTileState = CurrentTileState.None;
    public CurrentTileState CurruntTileState { get; set; }

    /// <summary>
    /// 타일의 기본 크기값 설정안하면 기본값 1로셋팅
    /// </summary>
    Vector3 tileSize = Vector3.one;
    public Vector3 TileSize => tileSize;

    public Action OnInitData { get; set; }
    public Action OnResetData { get; set; }
    public Action<int> OnClick { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    /// <summary>
    /// 타일 프리팹 
    /// </summary>
    GameObject tilePrefab = null;

    void TileInitialaze(int tileIndex, Vector3Int tilePos, GameObject prefab , CurrentTileState tileState = CurrentTileState.None) 
    {
        index = tileIndex;
        gridPos = tilePos;
        curruntTileState = tileState;
        tilePrefab = prefab;
        if (tilePrefab != null) 
        {
            SetTileSize();
        }
    }

    /// <summary>
    /// 타일 프리팹이 존재할경우 크기구하기 
    /// </summary>
    private void SetTileSize() 
    {
    }

    /// <summary>
    /// 초기화용 값셋팅
    /// </summary>
    public void ResetTile() 
    {
        index = -1;
        gridPos = Vector3Int.zero;
        curruntTileState = CurrentTileState.None;
        tilePrefab = null;

    }

    void ITileBase.OnInitData(int index, Vector3Int grid3DPos, CurrentTileState tileState)
    {
        throw new NotImplementedException();
    }

    void ITileBase.OnResetData()
    {
        throw new NotImplementedException();
    }
}