

using System;
using UnityEngine;
/// <summary>
/// 타일의 공통기능 및 프로퍼티
/// </summary>
public interface ITileBase
{
    /// <summary>
    /// 현재 타일의 그리드 좌표 값 (인덱스로 사용가능하다.)
    /// </summary>
    Vector3Int Tile3DPos { get; }

    /// <summary>
    /// 타일의 크기 저장해둘 백터 
    /// </summary>
    Vector3 TileSize { get; }

    /// <summary>
    /// 현재 타일의 값 (이동가능한지, 불가능한지, 캐릭터인지 등등)
    /// </summary>
    CurrentTileState CurruntTileState { get; set; }

    /// <summary>
    /// 셀이 클릭되면 반응할 델리게이트
    /// </summary>
    Action<int> OnClick { get; set; }

    /// <summary>
    /// 데이터 기본값 셋팅할때 사용할 함수
    /// </summary>
    /// <param name="index">셀의 인덱스값</param>
    /// <param name="grid3DPos">셀의 좌표값</param>
    /// <param name="tileState">셀의 상태값</param>
    void OnInitData(int index, Vector3Int grid3DPos, CurrentTileState tileState);

    /// <summary>
    /// 데이터 초기값으로 돌리기용으로 사용될 함수
    /// </summary>
    void OnResetData();
}
