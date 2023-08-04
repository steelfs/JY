using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy : MonoBehaviour, IBattle, IHealth
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
                        anim.SetTrigger("Stop");
                        agent.isStopped = true; //에이전트 정지 시키기
                        agent.velocity = Vector3.zero; // 남아있던 운동량, 관성 제거 
                        WaitTimer = waitTime;//대기시간 초기화
                        onStateUpdate = UpdateWait;// 대리자 신호를 보내는 것이 아니라  함수를 저장하는 용도로 사용 
                        break;
                    case EnemyState.Patrol:
                        anim.SetTrigger("Move");
                        agent.isStopped = false; // 에이전트 다시 켜시
                        agent.SetDestination(wayPointTarget.position);//목적지 재설정
                        onStateUpdate = UpdatePatrol;
                        break;
                    case EnemyState.Chase:
                        anim.SetTrigger("Move");
                        agent.isStopped = false;
                        onStateUpdate = UpdateChase;
                        break;
                    case EnemyState.Attack:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        attackCoolTime = attackSpeed;
                        anim.SetTrigger("Attack");

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

    public WayPoints wayPoints; //적이 순찰할 웨이포인트
    protected Transform WayPointTarget = null;


    public float attackPower = 10.0f;
    public float AttackPower => attackPower;

    float attackSpeed = 1.0f;
    float attackCoolTime = 1.0f;

    public float defancePower = 3.0f;
    public float DefencePower => defancePower;

    float hp = 100.0f;
    public float  maxHP= 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            if (State != EnemyState.Dead && hp <= 0)
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, maxHP);
            onHealthChange?.Invoke(hp/maxHP);
        }
    }

    public float MaxHP => maxHP;

    public Action<float> onHealthChange { get; set; }
    public Action onDie { get; set; }

    public bool IsAlive => hp > 0;



    Action onStateUpdate;

    
    Animator anim;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rb;
    Transform chaseTarget = null; //추격대상
    IBattle attackTarget = null;
    protected Transform wayPointTarget;// 적이 이동할 트렌스폼 (웨이포인트 위치 or 플레이어 위치) waypoints.Current



    private void Awake()
    {

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bodyCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        AttackArea attackArea = GetComponentInChildren<AttackArea>();
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)//추적상태면 
            {
                attackTarget = target;
                State = EnemyState.Attack;//공격상태로 변경
            }
        };
        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target) // 공격대상이 나가면 
            {
                attackTarget = null; // 공격대상 비우고 
                if (State != EnemyState.Dead) //죽은상태가 아니면
                {
                    State = EnemyState.Chase;// 추적상태로 변경 
                }
            }
        };
    }
    private void Start()
    {
        agent.speed = moveSpeed;
        if (wayPoints == null)
        {
            Debug.LogWarning("웨이포인트가 없습니다.");
            wayPointTarget = transform;
        }
        else
        {
            wayPointTarget = wayPoints.Current;
        }
        State = EnemyState.Wait;
        anim.ResetTrigger("Stop"); // Wait tstate에서 stop트리거 오더가 바로 처리되지 못하고 쌓이는 현상 방지하기 위함
    }
    private void Update()
    {
        onStateUpdate();
    }

    //아래 5개 함수들은 각 상태별로 실행될 함수 
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
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//!agent.pathPending은 경로계산이 진행중인지 확인하는 프로퍼티, true면 경로 계산중
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
        attackCoolTime -= Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f);
        if (attackCoolTime < 0)
        {
            anim.SetTrigger("Attack");
            Attack(attackTarget);
        }
    }
    void UpdateDead()
    {

    }

    bool SearchPlayer()//시야범위 안에 플레이어가 있는지 찾는 함수  찾으면 true 리턴, //키보드로 플레이어 조작할 때 콜라이더가 null 이 아닌지 확인 필요 
    {
        //연산량 줄이기 - 완만한 조건을 먼저 체크하고 조건을 만족할 시 점점더 세세한 조건을 체크한다.
        bool result = false;
        chaseTarget = null;
        Collider[] playerCollider = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));//파라미터의  int는 비트값이다

        if (playerCollider.Length > 0) 
        {
      
            Vector3 playerPos = playerCollider[0].transform.position;
            Vector3 toPlayerDir = playerPos - transform.position; // 적 위치에서 플레이어로 가는 방향벡터 

            if (toPlayerDir.sqrMagnitude < closeSightRange * closeSightRange)
            {
                chaseTarget = playerCollider[0].transform;
                result = true;
            }
            else
            {
                //근접시야범위보다는 밖이므로 시야각 내부인지 확인
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
   
        //farSightRange 안에있는지 확인 @

        //안에있으면 시야각 안에 있는지 확인
        //시야가 막혔는지 확인
        //근접


        return result;
    }
    bool IsInSightAngle(Vector3 toTargetDirection)//param 대상으로 향하는 방향 벡터// 시야각 안에 있으면 true 
    {
        //Vector3 dir1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up) * transform.forward;
        //Vector3 dir2 = Quaternion.AngleAxis(sightHalfAngle, transform.up) * transform.forward;
        //float angle = Vector3.Angle(dir2, dir1);
        
        float angle = Vector3.Angle(transform.forward, toTargetDirection);
        return sightHalfAngle > angle;
    }
    bool IsSightClear(Vector3 toTargetDirection)//대상을 바라볼 때 시야가 다른 오브젝트에 가려졌는지 확인하는 함수, Ray로 확인
    {
        bool result = false;
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDirection);//레이의origin 위치를 눈 위치로 조정
        if (Physics.Raycast(ray, out RaycastHit hitInfo, farSightRange))
        {
            if (hitInfo.collider.CompareTag("Player"))//
            {
                result = true;
            }
        }
        return result;
    }
    public void Attack(IBattle target)
    {
        target.defence(AttackPower); // 대상에게 데미지를 주고 
        attackCoolTime = attackSpeed;// 쿨타임 초기화 
    }

    public void defence(float damage)
    {
        if (State != EnemyState.Dead)
        {
            anim.SetTrigger("Hit");
            HP -= (damage - defancePower);//방어력만큼 차감하고 HP감소 
        }
    }

    public void Die()
    {
        State = EnemyState.Dead;
        onDie?.Invoke();
    }

    public void HealthRegenerate(float totalRegen, float duration)//duration동안 totalRegen만큼 회복하는 함수 
    {
        StartCoroutine(RecoveryHealth(totalRegen, duration));
    }
    public void HealthRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount)
    {
        StartCoroutine(RecoveryHpByTick(tickRegen, tickTime, totalTickCount));
    }
    IEnumerator RecoveryHealth(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;
        float time = 0.0f;
        while (time < duration)
        {
            //HP += totalRegen / duration * Time.deltaTime; 값을 미리 캐싱 해두는 것이 연산량을 더 줄일 수 있다.
            HP += Time.deltaTime * regenPerSec;
            time += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RecoveryHpByTick(float hpValuePerTick, float timePerTick, uint totalTick)
    {
        int tick = 0;
        WaitForSeconds tickValue = new WaitForSeconds(timePerTick);
        while (tick < totalTick)
        {
            HP += hpValuePerTick;
            yield return tickValue;
            tick++;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        bool playerShow = SearchPlayer(); // 플레이어가 시야범위안에 들어왔는지 확인

        Handles.color = playerShow ? Color.red : Color.green;
        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f);

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);// param = 각도, 축
        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward);
        Handles.DrawLine(transform.position, transform.position + q2 * forward);

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2.0f, farSightRange, 2.0f); // 호 그리기

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
