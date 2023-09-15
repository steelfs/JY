using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Board board;//이 플레이어의 보드 
    public Board Board => board;

    protected Ship[] ships;//이 플레이어가 갖고있는 함선들 //
    public Ship[] Ships => ships;

    protected int remainShipCount;//남은 배 카운트
    public bool IsDefeat => remainShipCount < 1;

    bool isActionDone = false;//턴이 종료 되어있는지 여부 
    public bool IsActionDone => isActionDone;

    protected PlayerBase opponent;//상대방 

    public Action<PlayerBase> onAttackFail;//이 플레이어의 공격이 실패했으을 알리는 신호 param = 자기 자신
    public Action<PlayerBase> onDefead;// 이 플레이어가 패배했을음 알리는 신호 
    public Action onActionEnd;

    //턴 관리용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }
    public virtual void OnPlayerTurnEnd()
    {
    }
    //턴 관리용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //공격 관련 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public void Attack(Vector2Int attackGridPos)//
    {

    }
    public void Attack(int index)
    {

    }
    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.World_To_Grid(worldPos));
    }
    
    public void AutoAttack()//CPU, 인간 플레이어가 타임아웃 됐을 때 사용 
    {

    }
    //공격 관련 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //함선 배치용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public void AutoShipDeployment(bool isShowShips)//이 플레이어의 보드에 함선을 배치하는 함수 
    {

    }
    private List<int> GetShipAroundPosition(Ship ship)//함선 주변의 인덱스들을 구하는 함수 
    {
        return null;
    }
    public void UndoAllShipDeployment()//모든함선의 배치를 취소하는 함수 
    {

    }
    //함선 배치용 함수ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ


    //함선 침몰 및 패배처리 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    void OnShipDestroy(Ship ship)//내가 가진 특정배가 침몰됐을 떄 실행될 함수 
    {

    }

    void OnDefeat()//모든 배가 침몰되었을 때 실행될 함수 
    {

    }

    //기타ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    public void Clear()//초기화 . 게임시작 직전 상태로 변경 
    {

    }

    //기타ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

}
