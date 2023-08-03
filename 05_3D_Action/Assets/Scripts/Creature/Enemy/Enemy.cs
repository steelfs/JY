using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //���¸ӽ�
    // ��� : ���� : ���� : ���� : ���
    // ��������Ʈ
    // �÷��̾�
   
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

    public float sightRange;//�þ߹���
    public float closeSightRange;//���� �þ߹���
    public float sightHalfAngle;//�þ߰��� ����
    public float moveSpeed = 3.0f;

    public WayPoints wayPoints; //���� ������ ��������Ʈ
    protected Transform WayPointTarget
    {
        get => wayPoints.Current;
    }

    Animator anim;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rb;
    Transform chaseTarget; //�߰ݴ��
    protected Transform moveTarget;// ���� �̵��� Ʈ������ (��������Ʈ ��ġ or �÷��̾� ��ġ)



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
            Debug.LogWarning("��������Ʈ�� �����ϴ�.");
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
