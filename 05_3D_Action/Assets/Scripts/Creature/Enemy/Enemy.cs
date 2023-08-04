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
                        WaitTimer = waitTime;//���ð� �ʱ�ȭ
                        onStateUpdate = UpdateWait;// �븮�� ��ȣ�� ������ ���� �ƴ϶�  �Լ��� �����ϴ� �뵵�� ��� 
                        break;
                    case EnemyState.Patrol:
                        agent.SetDestination(moveTarget.position);//������ �缳��
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
                State = EnemyState.Patrol;// �ð��� �� �Ǹ� 
            }
        }
    }

    public float farSightRange;//�þ߹���
    public float closeSightRange;//���� �þ߹���
    public float sightHalfAngle;//�þ߰��� ����
    public float moveSpeed = 3.0f;
    Vector3 playerPos;

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
    //Transform chaseTarget; //�߰ݴ��
    protected Transform moveTarget;// ���� �̵��� Ʈ������ (��������Ʈ ��ġ or �÷��̾� ��ġ) waypoints.Current



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

    //�Ʒ� 5�� �Լ����� �� ���º��� ����� �Լ� 
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
    bool SearchPlayer()//�þ߹��� �ȿ� �÷��̾ �ִ��� ã�� �Լ�  ã���� true ����, 
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
        //farSightRange �ȿ��ִ��� Ȯ�� @

        //�ȿ������� �þ߰� �ȿ� �ִ��� Ȯ��
        //�þ߰� �������� Ȯ��
        //����


        return result;
    }
    bool IsInSightAngle(Vector3 toTargetDirection)//param ������� ���ϴ� ���� ����// �þ߰� �ȿ� ������ true 
    {
        Vector3 dir1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up) * transform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(sightHalfAngle, transform.up) * transform.forward;

        float angle = Vector3.Angle(dir2, dir1);

        Debug.Log(angle);
        bool result = false;
        return result;
    }
    bool IsSightBlocked(Vector3 toTargetDirection)//����� �ٶ� �� �þ߰� �ٸ� ������Ʈ�� ���������� Ȯ���ϴ� �Լ�, Ray�� Ȯ��
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
        //���Ÿ� �þ߹��� ���
        //�ٰŸ� �þ߹��� �����
        //�÷��̾ �þ߹��� �ȿ� ������ ������
        //Handles.DrawWireDisc(transform.position, transform.up, sightRange, 2);//�����Ҷ� �������

        //if (barrelBodyTransform == null)
        //{
        //    barrelBodyTransform = transform.GetChild(4);
        //}
        //Gizmos.DrawLine(barrelBodyTransform.position, barrelBodyTransform.position + barrelBodyTransform.forward * sightRange);

        //Vector3 from = barrelBodyTransform.position;
        //Vector3 to = barrelBodyTransform.position + barrelBodyTransform.forward * sightRange;
        //Gizmos.color = IsFiring ? Color.red : Color.green; // true�� ���� 
        //Gizmos.DrawLine(from, to);

        //Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        //Vector3 dir2 = Quaternion.AngleAxis(fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        ////drawLine = 2D���� vector3�� �ϵ��ڵ����� �־��� �Ͱ� ���¸� �ٸ� ���̴�. ���� ��ư� ������ �ʿ�� ����.
        ////�߻簢
        //Gizmos.color = Color.white;
        //to = barrelBodyTransform.position + dir1 * sightRange;
        //Gizmos.DrawLine(from, to);

        //to = barrelBodyTransform.position + dir2 * sightRange;
        //Gizmos.DrawLine(from, to);
    }
#endif
}
