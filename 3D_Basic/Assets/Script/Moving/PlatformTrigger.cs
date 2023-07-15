using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 올라가면 다음 플랫폼까지 움직이는 플랫폼
public class PlatformTrigger : PlatformBase
{
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
    private void OnTriggerEnter(Collider other) // 플레이어가 트리거 영역안에 들어왔을때 움직이기 시작
    {
        isMoveStart = other.CompareTag("Player");
    }
    protected override void OnArrived()// 도착하면 멈춤
    {
        base.OnArrived();
        isMoveStart = false;
    }

}
