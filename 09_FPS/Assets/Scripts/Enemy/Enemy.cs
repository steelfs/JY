using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum HitLocation
{
    Body,
    Head,
    Arm,
    Leg
}
public enum State
{
    Patrol,
    Chase,
    Attack,
    Die
}
public class Enemy : MonoBehaviour
{
    Player player;
    NavMeshAgent agent;
    Vector3 destination;
    int size = CellVisualizer.CellSize;

    public float sightRange = 10;
    public float sightAngle = 30;
    public float walkSpeed = 5;
    public float runSpeed = 10;
    float speedPenalty = 0;

    public float hp = 30.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Die();
            }
        }
    }
    public float maxHp = 30.0f;

    State state;
    public State State
    {
        get => state;
        set
        {
            state = value;
            switch (state)
            {
                case State.Patrol:
                    Patrol();
                    StartCoroutine(DetectCoroutine);
                    break;
                case State.Chase:
                    break;
                case State.Attack:
                    break;
                case State.Die:
                    break;
                default:
                    break;
            }
        }
    }

    public Action<Enemy> onDie;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }
    private void OnEnable()
    {
    
    }
    private void Start()
    {
        player = GameManager.Inst.Player;
        DetectCoroutine = DetectPlayerCoroutine();
        State = State.Patrol;

    }
    void Patrol()
    {
        HP = maxHp;
        agent.speed = walkSpeed;
        speedPenalty = 0;
        SetDestination();
    }
    void Chase()
    {

    }
    void Attack()
    {

    }
    private void Die()
    {
        onDie?.Invoke(this);
        gameObject.SetActive(false);
    }
    void SetDestination()
    {
        Vector3 destination = NewDestination();
        agent.SetDestination(destination);
    }
    Vector3 NewDestination()
    {
        destination.x = (int)(transform.position.x + UnityEngine.Random.Range(-3, 3) * size);
        destination.z = (int)(transform.position.z + UnityEngine.Random.Range(-3, 3) * size);

        return destination;
    }
    //이동속도가지기
    // NavMeshAgent 를 이용해 이동을 한다.
    //3. 목적지 기준 +- 3칸 이내 목적지 설정
    // 도착시 목적지 재설정
    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.01f)
        {
            SetDestination();
        }
    }
    IEnumerator DetectCoroutine;
    IEnumerator DetectPlayerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Vector3 toPlayer = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, toPlayer);

            if (angle < sightAngle / 2)
            { // 시야각 체크
                if (Physics.Raycast(transform.position, toPlayer, out RaycastHit hit, sightRange))
                {
                    if (hit.collider.tag == "Player")
                    {
                        // 플레이어 발견!
                        agent.SetDestination(player.transform.position);
                    }
                }
            }
        }
    }
   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerPos = other.transform.position;
            Vector3 dir = (playerPos - transform.position).normalized;
            float degree = Vector3.Angle(transform.forward, dir);
            if ((sightAngle / 2) > degree)
            {
                UnityEngine.Debug.Log("범위 안쪽");
            }
            else
            {
                UnityEngine.Debug.Log("범위 바깥쪽");
            }

        }
    }
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;

        Quaternion leftAngle = Quaternion.AngleAxis(sightAngle, Vector3.up);
        Quaternion rightAngle = Quaternion.AngleAxis(-sightAngle, Vector3.up);
        Vector3 leftRotation = leftAngle * transform.forward;
        Vector3 rightRotation = rightAngle * transform.forward;
        Vector3 left = transform.position + leftRotation * 5;
        Vector3 right = transform.position + rightRotation * 5;
        Handles.DrawLine(transform.position, left, 3.0f);
        Handles.DrawLine(transform.position, right, 3.0f);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("플레이어 감지해제");
        }
    }
    public void OnAttacked(HitLocation hitLocation, float damage)
    {
        
        switch (hitLocation)
        {
            case HitLocation.Body:
                
                HP -= damage;
                break;
            case HitLocation.Head:
                HP -= damage * 2;
                break;
            case HitLocation.Arm:
                HP -= damage;
                break;
            case HitLocation.Leg:
                speedPenalty += 1;
                agent.speed = walkSpeed - speedPenalty;
                HP -= damage;
                break;
            default:
                break;
        }
      
    }
}


//상태를 가진다.
// 순찰 : 랜덤으로 계속 이동, 추적
// : 플레이어가 시야에 들어오면 추적상태가 되어서 마지막으로 목격한 위치를 목적지로 도착후 ,
// 공격 : 추적상태에서 공격범위 안으로 들어왔을 때 공격시작,
// 사망 : 일정시간 지나서 부활 후 순찰상태

