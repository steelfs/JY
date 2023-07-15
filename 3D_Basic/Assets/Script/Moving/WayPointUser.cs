using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WayPoints 클래스를 사용하는 클래스 //톱니 or etc
//웨이포인트를 따라서 이동함
public class WayPointUser : MonoBehaviour
{
    public WayPoints targetWayPoints;// 이 오브젝트가 따라움직일 웨이포인트들을 관리하는 클래스

    public float moveSpeed = 5.0f;

    Transform target; //현재 목표로하는 트렌스폼
    Vector3 moveDir; // target으로 가는 방향

    protected Vector3 moveDelta = Vector3.zero; //이번 물리프레임에 이동한 정도
    protected virtual Transform Target //목적지 지정 프로퍼티
    {
        get => target;
        set
        {
            target = value;
            moveDir = (target.position - transform.position).normalized; //방향설정
        }
    }
    bool IsArrived//현재위치가 도착지점에 근접해지면 true
    {
        get
        {
            return (target.position - transform.position).sqrMagnitude < 0.02f;
        }
    }
    private void Start()
    {
        Target = targetWayPoints.currentWayPoint;
    }
    private void FixedUpdate()
    {
        OnMove();
    }
    protected virtual void OnArrived()//도착했을 때 실행되는 함수
    {
        Target = targetWayPoints.GetNextWayPoint();
    }
    protected virtual void OnMove() //이동처리용 함수 FixedUpdate에서 호출
    {
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDir;
        transform.Translate(moveDelta, Space.World);
        if (IsArrived)
        {
            OnArrived();
        }
    }
    
}
