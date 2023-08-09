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
    //���¸ӽ�
    // ��� : ���� : ���� : ���� : ���
    // ��������Ʈ
    // �÷��̾�
    [System.Serializable]
    public struct ItemDropInfo
    {
        [Range(0, 1)]
        public float dropRate;
        public ItemCode code;
    }
    public ItemDropInfo[] dropInfo;
  

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
                        anim.SetTrigger("Stop");
                        agent.isStopped = true; //������Ʈ ���� ��Ű��
                        agent.velocity = Vector3.zero; // �����ִ� ���, ���� ���� 
                        WaitTimer = waitTime;//���ð� �ʱ�ȭ
                        onStateUpdate = UpdateWait;// �븮�� ��ȣ�� ������ ���� �ƴ϶�  �Լ��� �����ϴ� �뵵�� ��� 
                        break;
                    case EnemyState.Patrol:
                        anim.SetTrigger("Move");
                        agent.isStopped = false; // ������Ʈ �ٽ� �ѽ�
                        agent.SetDestination(wayPointTarget.position);//������ �缳��
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
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        anim.SetTrigger("Die");
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
            hp = value;
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

    EnemyHP_Bar enemyHP_Bar;
    ParticleSystem ps;

    Animator anim;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rb;
    Transform chaseTarget = null; //�߰ݴ��
    IBattle attackTarget = null;
    protected Transform wayPointTarget;// ���� �̵��� Ʈ������ (��������Ʈ ��ġ or �÷��̾� ��ġ) waypoints.Current



    private void Awake()
    {
        enemyHP_Bar = GetComponentInChildren<EnemyHP_Bar>();
        ps = transform.GetChild(4).GetComponent<ParticleSystem>();

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bodyCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();

        AttackArea attackArea = GetComponentInChildren<AttackArea>();
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)//�������¸� 
            {
                attackTarget = target;
                State = EnemyState.Attack;//���ݻ��·� ����
            }
        };
        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target) // ���ݴ���� ������ 
            {
                attackTarget = null; // ���ݴ�� ���� 
                if (State != EnemyState.Dead) //�������°� �ƴϸ�
                {
                    State = EnemyState.Chase;// �������·� ���� 
                }
            }
        };
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
        anim.ResetTrigger("Stop"); // Wait tstate���� stopƮ���� ������ �ٷ� ó������ ���ϰ� ���̴� ���� �����ϱ� ����
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
        attackCoolTime -= Time.deltaTime;
        //attackTarget.transform.position - transform.position�� �������� ���⺤�͸� ���ϴ� ���̴�
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

    bool SearchPlayer()//�þ߹��� �ȿ� �÷��̾ �ִ��� ã�� �Լ�  ã���� true ����, //Ű����� �÷��̾� ������ �� �ݶ��̴��� null �� �ƴ��� Ȯ�� �ʿ� 
    {
        //���귮 ���̱� - �ϸ��� ������ ���� üũ�ϰ� ������ ������ �� ������ ������ ������ üũ�Ѵ�.
        bool result = false;
        chaseTarget = null;
        Collider[] playerCollider = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));//�Ķ������  int�� ��Ʈ���̴�

        if (playerCollider.Length > 0) 
        {
      
            Vector3 playerPos = playerCollider[0].transform.position;
            Vector3 toPlayerDir = playerPos - transform.position; // �� ��ġ���� �÷��̾�� ���� ���⺤�� 

            //���� ���� �����Ÿ��� �ִ��� Ȯ���ϴ� �ڵ�sqrMagnitude�� �����̱⶧���� closeSightRange���� �������� ���Ѵ�
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
        
        float angle = Vector3.Angle(transform.forward, toTargetDirection);//�ΰ��� ���͸� ������ �� ��ġ ������ ���� �������ִ� �Լ�
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
    public void Attack(IBattle target)
    {
        target.defence(AttackPower); // ��󿡰� �������� �ְ� 
        attackCoolTime = attackSpeed;// ��Ÿ�� �ʱ�ȭ 
    }

    public void defence(float damage)
    {
        if (State != EnemyState.Dead)
        {
            anim.SetTrigger("Hit");
            HP -= Mathf.Max(0, damage - defancePower);//���¸�ŭ �����ϰ� HP���� 
        }
    }

    public void Die()
    {
        State = EnemyState.Dead;
        StartCoroutine(DeadSequence());
        onDie?.Invoke();
    }
    IEnumerator DeadSequence()
    {
        bodyCollider.enabled = false;
        //�ٴڿ� ���̴� ����Ʈ �ѱ�
        //HP �� ���ֱ�
        ps.Play();
        ps.transform.SetParent(null);// ���� �ȶ������� �ϱ�
        enemyHP_Bar.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.35f);

        MakeDropItems();

        yield return new WaitForSeconds(1.15f);//�״� �ִϸ��̼� ���� �� ����
        agent.enabled = false;          // navMesh �� ���������� �׻� navMesh  ���� �ִ�.�������� �ʴ´�.
        rb.isKinematic = false;
        rb.drag = 10.0f; //������ ���� infinite => �������� �ʹ� Ŀ�� �ȶ�����
        yield return new WaitForSeconds(1.5f); // ������ ������ ������ ���

        Destroy(this.gameObject);
        //�״� �ִϸ��̼� ������ �ٴھƷ��� ����Ʈ����
        //����� �������� ������ ����
        //����Ʈ ����
        //navmeshagent����
        Destroy(ps.gameObject);
    }

    private void MakeDropItems()//�������� ����ϴ� �Լ� 
    {
        //Dictionary<ItemCode, uint> droptable = new Dictionary<ItemCode, uint>(5);
        //droptable.Add(ItemCode.FishSteak, 0);
        //droptable.Add(ItemCode.CopperCoin, 0);
        //droptable.Add(ItemCode.SilverCoin, 0);
        //droptable.Add(ItemCode.GoldCoin, 0);
        //droptable.Add(ItemCode.Apple, 0);
        //for (int i = 0; i < 1000000; i++)
        //{
            foreach (var item in dropInfo)
            {
                //uint repeatCount = 0;
                uint count = 0;
                while (count < 3)
                {
                    if (UnityEngine.Random.value < item.dropRate)
                    {
                        count++;

                    }
                    else
                    {
                        break;
                    }
                }
               // droptable[item.code] += count;
                 ItemFactory.MakeItems(item.code, count, transform.position, true);
            }
        //}
  
        //foreach (var item in droptable)
        //{
        //    Debug.Log($"{item} : {droptable.Values}");
        //}
    }

    public void HealthRegenerate(float totalRegen, float duration)//duration���� totalRegen��ŭ ȸ���ϴ� �Լ� 
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
            //HP += totalRegen / duration * Time.deltaTime; ���� �̸� ĳ�� �صδ� ���� ���귮�� �� ���� �� �ִ�.
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
  

    private void OnDrawGizmosSelected()
    {
        bool playerShow = SearchPlayer(); // �÷��̾ �þ߹����ȿ� ���Դ��� Ȯ��

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

    }


#endif
}
