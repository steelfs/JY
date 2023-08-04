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

    protected EnemyState State // �Ը� Ŀ�� �� ������ ���º��� Ŭ������ ������ ���� ����.
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
                        agent.isStopped = true; //������Ʈ ���� ��Ű��
                        agent.velocity = Vector3.zero; // �����ִ� ���, ���� ���� 
                        WaitTimer = waitTime;//���ð� �ʱ�ȭ
                        onStateUpdate = UpdateWait;// �븮�� ��ȣ�� ������ ���� �ƴ϶�  �Լ��� �����ϴ� �뵵�� ��� 
                        break;
                    case EnemyState.Patrol:
                        agent.isStopped = false; // ������Ʈ �ٽ� �ѽ�
                        agent.SetDestination(wayPointTarget.position);//������ �缳��
                        onStateUpdate = UpdatePatrol;
                        break;
                    case EnemyState.Chase:
                        agent.isStopped = false;
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
                State = EnemyState.Patrol;// �ð��� �� �Ǹ� 
            }
        }
    }

    public float farSightRange;//�þ߹���
    public float closeSightRange;//���� �þ߹���
    public float sightHalfAngle;//�þ߰��� ����
    public float moveSpeed = 3.0f;

    public WayPoints wayPoints; //���� ������ ��������Ʈ
    protected Transform WayPointTarget
    {
        get => wayPoints.Current;
    }

    Action onStateUpdate;

    Animator anim;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rb;
    Transform chaseTarget = null; //�߰ݴ��
    protected Transform wayPointTarget;// ���� �̵��� Ʈ������ (��������Ʈ ��ġ or �÷��̾� ��ġ) waypoints.Current



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
            Debug.LogWarning("��������Ʈ�� �����ϴ�.");
            wayPointTarget = transform;
        }
        else
        {
            wayPointTarget = wayPoints.Current;
        }
        State = EnemyState.Wait;
    
    }
    private void Update()
    {
        onStateUpdate();
    }

    //�Ʒ� 5�� �Լ����� �� ���º��� ����� �Լ� 
    void UpdateWait()
    {
        if (SearchPlayer())
        {
            State = EnemyState.Chase;
        }
        else
        {
            WaitTimer -= Time.deltaTime;
        }
    }
    void UpdatePatrol()
    {
        if (SearchPlayer())
        {
            State = EnemyState.Chase;
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//!agent.pathPending�� ��ΰ���� ���������� Ȯ���ϴ� ������Ƽ, true�� ��� �����
            {
                wayPointTarget = wayPoints.MoveNext();
                State = EnemyState.Wait;
            }
        }
     
    }
    void UpdateChase()
    {
        if (SearchPlayer())
        {
            agent.SetDestination(chaseTarget.position);
        }
        else
        {
            State = EnemyState.Wait;
        }
    }
    void UpdateAttack()
    {

    }
    void UpdateDead()
    {

    }

    bool SearchPlayer()//�þ߹��� �ȿ� �÷��̾ �ִ��� ã�� �Լ�  ã���� true ����, 
    {
        //���귮 ���̱� - �ϸ��� ������ ���� üũ�ϰ� ������ ������ �� ������ ������ ������ üũ�Ѵ�.
        bool result = false;
        chaseTarget = null;
        Collider[] playerCollider = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));//�Ķ������  int�� ��Ʈ���̴�

        if (playerCollider.Length > 0) 
        {
      
            Vector3 playerPos = playerCollider[0].transform.position;
            Vector3 toPlayerDir = playerPos - transform.position; // �� ��ġ���� �÷��̾�� ���� ���⺤�� 

            if (toPlayerDir.sqrMagnitude < closeSightRange * closeSightRange)
            {
                chaseTarget = playerCollider[0].transform;
                result = true;
            }
            else
            {
                //�����þ߹������ٴ� ���̹Ƿ� �þ߰� �������� Ȯ��
                if (IsInSightAngle(toPlayerDir))
                {
                    if (IsSightClear(toPlayerDir))
                    {
                        chaseTarget = playerCollider[0].transform;
                        result = true;
                    }
                }
            }
        }
   
        //farSightRange �ȿ��ִ��� Ȯ�� @

        //�ȿ������� �þ߰� �ȿ� �ִ��� Ȯ��
        //�þ߰� �������� Ȯ��
        //����


        return result;
    }
    bool IsInSightAngle(Vector3 toTargetDirection)//param ������� ���ϴ� ���� ����// �þ߰� �ȿ� ������ true 
    {
        //Vector3 dir1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up) * transform.forward;
        //Vector3 dir2 = Quaternion.AngleAxis(sightHalfAngle, transform.up) * transform.forward;
        //float angle = Vector3.Angle(dir2, dir1);
        
        float angle = Vector3.Angle(transform.forward, toTargetDirection);
        return sightHalfAngle > angle;
    }
    bool IsSightClear(Vector3 toTargetDirection)//����� �ٶ� �� �þ߰� �ٸ� ������Ʈ�� ���������� Ȯ���ϴ� �Լ�, Ray�� Ȯ��
    {
        bool result = false;
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDirection);//������origin ��ġ�� �� ��ġ�� ����
        if (Physics.Raycast(ray, out RaycastHit hitInfo, farSightRange))
        {
            if (hitInfo.collider.CompareTag("Player"))//
            {
                result = true;
            }
        }
        return result;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        bool playerShow = false; // �÷��̾ �þ߹����ȿ� ���Դ��� Ȯ��

        Handles.color = playerShow ? Color.red : Color.green;
        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f);

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);// param = ����, ��
        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward);
        Handles.DrawLine(transform.position, transform.position + q2 * forward);

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2.0f, farSightRange, 2.0f); // ȣ �׸���

        Handles.color = playerShow ? Color.red : Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, closeSightRange);
        //Handles.DrawWireDisc(transform.position, transform.up, farSightRange, 2);
        //Handles.color = Color.yellow;
        //Handles.DrawWireDisc(transform.position, transform.up, closeSightRange, 2);

        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * farSightRange);

        //Vector3 from = transform.position;
        //Vector3 to = transform.position + transform.forward * farSightRange;

        //Vector3 dir1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up) * transform.forward;
        //Vector3 dir2 = Quaternion.AngleAxis(sightHalfAngle, transform.up) * transform.forward;

        //Gizmos.color = Color.white;
        //to = transform.position + dir1 * farSightRange;
        //Gizmos.DrawLine(from, to);
        //to = transform.position + dir2 * farSightRange;
        //Gizmos.DrawLine(from, to);
    }
#endif
}
