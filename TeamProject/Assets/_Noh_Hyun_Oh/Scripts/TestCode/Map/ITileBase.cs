

using System;
using UnityEngine;
/// <summary>
/// Ÿ���� ������ �� ������Ƽ
/// </summary>
public interface ITileBase
{
    /// <summary>
    /// ���� Ÿ���� �׸��� ��ǥ �� (�ε����� ��밡���ϴ�.)
    /// </summary>
    Vector3Int Tile3DPos { get; }

    /// <summary>
    /// Ÿ���� ũ�� �����ص� ���� 
    /// </summary>
    Vector3 TileSize { get; }

    /// <summary>
    /// ���� Ÿ���� �� (�̵���������, �Ұ�������, ĳ�������� ���)
    /// </summary>
    CurrentTileState CurruntTileState { get; set; }

    /// <summary>
    /// ���� Ŭ���Ǹ� ������ ��������Ʈ
    /// </summary>
    Action<int> OnClick { get; set; }

    /// <summary>
    /// ������ �⺻�� �����Ҷ� ����� �Լ�
    /// </summary>
    /// <param name="index">���� �ε�����</param>
    /// <param name="grid3DPos">���� ��ǥ��</param>
    /// <param name="tileState">���� ���°�</param>
    void OnInitData(int index, Vector3Int grid3DPos, CurrentTileState tileState);

    /// <summary>
    /// ������ �ʱⰪ���� ����������� ���� �Լ�
    /// </summary>
    void OnResetData();
}
