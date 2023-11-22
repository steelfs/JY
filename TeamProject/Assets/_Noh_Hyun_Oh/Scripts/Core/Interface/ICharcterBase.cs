using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 배틀 맵에서 유닛들이 가지고 있어야할 인터페이스 필요하면 추가예정
/// </summary>
public interface ICharcterBase 
{
    public Transform transform { get; }
    /// <summary>
    /// 현재 컨트롤 정보를 가져온다
    /// </summary>
    public bool IsControll { get; set; }
    /// <summary>
    /// 이동 버그가 있어서 추가 이동 끝낫는지 체크하는 프로퍼티 
    /// </summary>
    public bool IsMoveCheck { get;}

    /// <summary>
    /// 추적형 UI 캐싱용 프로퍼티
    /// </summary>
    TrackingBattleUI BattleUI { get; set; }

    /// <summary>
    /// 추적형 UI 가 있는 캔버스 위치
    /// </summary>
    Transform BattleUICanvas { get;  }

    /// <summary>
    /// 현재 캐릭터가 있는 타일 
    /// </summary>
    Tile CurrentTile { get; }

    /// <summary>
    /// 외부에서 타일 셋팅용
    /// </summary>
    Func<Tile> GetCurrentTile { get; set; }

    
    /// <summary>
    /// 캐릭터가 이동할수있는 거리 (행동력값을 넘겨줘도됨)
    /// </summary>
    float MoveSize { get; }

    /// <summary>
    /// 턴유닛이 사라질때 초기화할 함수
    /// </summary>
    public void ResetData();

    /// <summary>
    /// 캐릭터의 이동함수
    /// </summary>
    public IEnumerator CharcterMove(Tile selectedTile);

    /// <summary>
    /// 캐릭터의 공격함수
    /// </summary>
    public IEnumerator CharcterAttack(Tile selectedTile);

    /// <summary>
    /// 공격 범위 안에 있는지 체크하는 함수
    /// </summary>
    /// <returns></returns>
    public bool IsAttackRange();

}
