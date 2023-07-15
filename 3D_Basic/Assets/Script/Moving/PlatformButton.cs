using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : PlatformBase, Iinteractable
{
    public bool IsDirectUse => true;
    bool isMoveStart = false;
    private void Start()
    {
        Target = targetWayPoints.GetNextWayPoint();// 첫번째 웨이포인트를 설정하기 안하면 OnArrived 실행되서 안움직임
    }
    private void FixedUpdate()
    {
        if (isMoveStart)
        {
            OnMove();
        }
    }
    protected override void OnArrived()
    {
        base.OnArrived();
        isMoveStart = false;
    }

    // 발동 순서 : 플레이어 에서 F인풋을 받아 애니매이션발동 -> 애니매이션발동되면 콜라이더 컴포넌트 활성화 ->  ItemUseChecker의 OntriggerEnter 발동-> OntriggerEnter에서 do while 루프로
    //Iinteractable 을 상속받은 것이있는지 최사우이 부모까지 검색  찾았다면 찾은 Iinteractable을 파라미터로 델리게이트 신호를 보내서 플레이어의 ItemUse를 실행시킴 -> 플레이어의 Use함수에서 Iinteractable.Use실행
    // bool 변수  변경 -> 이동
    public void Use()
    {
        isMoveStart = true;
    }
}
