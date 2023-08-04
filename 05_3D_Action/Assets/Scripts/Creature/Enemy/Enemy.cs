using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy : TestBase
{
    //상태머신
    // 대기 : 순찰 : 추적 : 공격 : 사망
    // 웨이포인트
    // 플레이어
   

    protected enum EnemyState
    {
        Wait = 0,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    EnemyState state = EnemyState.Patrol;

    protected EnemyState State // 규모가 커질 것 같으면 상태별로 클래스를 나누는 것이 좋다.
    {
        get => state;
        set
        {
            if (state != value) 
            {
                state = value;
                switch (state)
                {
                    case EnemyState.Wait:
                        WaitTimer = waitTime;//대기시간 초기화
                        onStateUpdate = UpdateWait;// 대리자 신호를 보내는 것이 아니라  함수를 저장하는 용도로 사용 
                        break;
                    case EnemyState.Patrol:
                        agent.SetDestination(moveTarget.position);//목적지 재설정
                        onStateUpdate = UpdatePatrol;
                        break;
                    case EnemyState.Chase:
                        onStateUpdate = UpdateChase;
                        break;
                    case EnemyState.Attack:
                        onStateUpdate = UpdateAttack;
                        break;
                    case EnemyState.Dead:
                        onStateUpdate = UpdateDead;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public float waitTime = 1.0f;
    float waitTimer = 1.0f;//
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waitTimer < 0)
            {
                State = EnemyState.Patrol;// 시간이 다 되면 
            }
        }
    }

    public float farSightRange;//시야범위
    public float closeSightRange;//근접 시야범위
    public float sightHalfAngle;//시야각의 절반
    public float moveSpeed = 3.0f;
    Vector3 playerPos;

    public WayPoints wayPoints; //적이 순찰할 웨이포인트
    protected Transform WayPointTarget
    {
        get => wayPoints.Current;
    }

    Action onStateUpdate;

    Animator anim;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rb;
    //Transform chaseTarget; //추격대상
    protected Transform moveTarget;// 적이 이동할 트렌스폼 (웨이포인트 위치 or 플레이어 위치) waypoints.Current



    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bodyCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        agent.speed = moveSpeed;
        if (wayPoints == null)
        {
            Debug.LogWarning("웨이포인트가 없습니다.");
            moveTarget = transform;
        }
        else
        {
            moveTarget = wayPoints.Current;
        }
        State = EnemyState.Wait;
    
    }
    private void Update()
    {
        onStateUpdate();
    }

    //아래 5개 함수들은 각 상태별로 실행될 함수 
    void UpdateWait()
    {
        WaitTimer -= Time.deltaTime;
    }
    void UpdatePatrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            moveTarget = wayPoints.MoveNext();
            State = EnemyState.Wait;
        }
    }
    void UpdateChase()
    {

    }
    void UpdateAttack()
    {

    }
    void UpdateDead()
    {

    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        SearchPlayer();
    }
    bool SearchPlayer()//시야범위 안에 플레이어가 있는지 찾는 함수  찾으면 true 리턴, 
    {
        bool result = false;
        Collider[] playerCollider = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));

        if (playerCollider != null) 
        {
            foreach(var player in playerCollider)
            {
                playerPos = player.transform.position;
            }            
        }
        if (IsInSightAngle(playerPos))
        {

        }
        //farSightRange 안에있는지 확인 @

        //안에있으면 시야각 안에 있는지 확인
        //시야가 막혔는지 확인
        //근접


        return result;
    }
    bool IsInSightAngle(Vector3 toTargetDirection)//param 대상으로 향하는 방향 벡터// 시야각 안에 있으면 true 
    {
        Vector3 dir1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(sightHalfAngle, transform.up) * transform.forward;

        float angle = Vector3.Angle(dir2, dir1);

        Debug.Log(angle);
        bool result = false;
        return result;
    }
    bool IsSightBlocked(Vector3 toTargetDirection)//대상을 바라볼 때 시야가 다른 오브젝트에 가려졌는지 확인하는 함수, Ray로 확인
    {
        bool result = false;
        return result;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, farSightRange, 2);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, closeSightRange, 2);

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * farSightRange);

        Vector3 from = transform.position;
        Vector3 to = transform.position + transform.forward * farSightRange;

        Vector3 dir1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(sightHalfAngle, transform.up) * transform.forward;

        Gizmos.color = Color.white;
        to = transform.position + dir1 * farSightRange;
        Gizmos.DrawLine(from, to);
        to = transform.position + dir2 * farSightRange;
        Gizmos.DrawLine(from, to);



        // Gizmos.DrawLine(from, to);
        //원거리 시야범위 녹색
        //근거리 시야범위 노란색
        //플레이어가 시야범위 안에 들어오면 빨간색
        //Handles.DrawWireDisc(transform.position, transform.up, sightRange, 2);//빌드할때 빼줘야함

        //if (barrelBodyTransform == null)
        //{
        //    barrelBodyTransform = transform.GetChild(4);
        //}
        //Gizmos.DrawLine(barrelBodyTransform.position, barrelBodyTransform.position + barrelBodyTransform.forward * sightRange);

        //Vector3 from = barrelBodyTransform.position;
        //Vector3 to = barrelBodyTransform.position + barrelBodyTransform.forward * sightRange;
        //Gizmos.color = IsFiring ? Color.red : Color.green; // true면 왼쪽 
        //Gizmos.DrawLine(from, to);

        //Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        //Vector3 dir2 = Quaternion.AngleAxis(fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        ////drawLine = 2D에서 vector3를 하드코딩으로 넣어준 것과 형태만 다를 뿐이다. 굳이 어렵게 생각할 필요는 없다.
        ////발사각
        //Gizmos.color = Color.white;
        //to = barrelBodyTransform.position + dir1 * sightRange;
        //Gizmos.DrawLine(from, to);

        //to = barrelBodyTransform.position + dir2 * sightRange;
        //Gizmos.DrawLine(from, to);
    }
#endif
}
