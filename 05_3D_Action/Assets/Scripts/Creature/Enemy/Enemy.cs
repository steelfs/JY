using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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

    protected EnemyState State
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
                        WaitTimer = waitTime;
                        onStateUpdate = UpdateWait;
                        break;
                    case EnemyState.Patrol:
                        agent.SetDestination(moveTarget.position);
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
    float waitTimer = 1.0f;
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTime = value;
            if (waitTimer < 0)
            {
                State = EnemyState.Patrol;
            }
        }
    }

    public float sightRange;//시야범위
    public float closeSightRange;//근접 시야범위
    public float sightHalfAngle;//시야각의 절반
    public float moveSpeed = 3.0f;

    public WayPoints wayPoints; //적이 순찰할 웨이포인트
    protected Transform WayPointTarget
    {
        get => wayPoints.Current;
    }

    Animator anim;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rb;
    Transform chaseTarget; //추격대상
    protected Transform moveTarget;// 적이 이동할 트렌스폼 (웨이포인트 위치 or 플레이어 위치)



    Action onStateUpdate;
    private void Awake()
    {
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

}
