using System;
using UnityEngine;
/// <summary>
/// ��Ʋ ȭ���� Ÿ��
/// </summary>
public class MapTileBase : MonoBehaviour, ITileBase
{
    /// <summary>
    /// Ÿ���� �ε�����
    /// </summary>
    int index = -1;
    public int TileIndex => index;

    /// <summary>
    /// Ÿ���� ��ǥ��
    /// </summary>
    Vector3Int gridPos = Vector3Int.zero;
    public Vector3Int Tile3DPos => gridPos;

    /// <summary>
    /// Ÿ���� ���°�
    /// </summary>
    CurrentTileState curruntTileState = CurrentTileState.None;
    public CurrentTileState CurruntTileState { get; set; }

    /// <summary>
    /// Ÿ���� �⺻ ũ�Ⱚ �������ϸ� �⺻�� 1�μ���
    /// </summary>
    Vector3 tileSize = Vector3.one;
    public Vector3 TileSize => tileSize;

    public Action OnInitData { get; set; }
    public Action OnResetData { get; set; }
    public Action<int> OnClick { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    /// <summary>
    /// Ÿ�� ������ 
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
    /// Ÿ�� �������� �����Ұ�� ũ�ⱸ�ϱ� 
    /// </summary>
    private void SetTileSize() 
    {
    }

    /// <summary>
    /// �ʱ�ȭ�� ������
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