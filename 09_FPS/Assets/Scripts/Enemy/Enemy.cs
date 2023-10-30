using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

public enum HitLocation : byte
{
    Body,
    Head,
    Arm,
    Leg
}
public enum BehaviourState : byte
{
    Wander,
    Chase,
    Attack,
    Find,
    Dead
}
public class Enemy : MonoBehaviour
{
    Player player;
    NavMeshAgent agent;
    Vector3 destination;
    int size = CellVisualizer.CellSize;
    Action on_Update = null;
    public Action<Enemy> onDie;


    public float sightRange = 20;
    public float sightAngle = 90;
    public float walkSpeed = 2;
    public float runSpeed = 7;
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
                Update_Dead();
            }
        }
    }
    public float maxHp = 30.0f;

    BehaviourState state = BehaviourState.Dead;
    public BehaviourState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                OnStateExit(state);
                state = value;
                OnStateEnter(state);
            }
        }
    }

    private void OnStateEnter(BehaviourState newState)
    {
        switch (newState)
        {
            case BehaviourState.Wander:
                StopAllCoroutines();
                agent.speed = walkSpeed;
                on_Update = Update_Wander;
                break;
            case BehaviourState.Chase:
                agent.speed = runSpeed;
                on_Update = Update_Chase;
                break;
            case BehaviourState.Attack:
                on_Update = Update_Attack;
                break;
            case BehaviourState.Find:
                findTimeElapsed = findTime;
                on_Update = Update_Find;
                agent.speed = runSpeed;
                StartCoroutine(LookAround());
                break;
            case BehaviourState.Dead:
                agent.speed = 0;
                on_Update = Update_Dead;
                break;
            default:
                break;
        }
    }
    private void OnStateExit(BehaviourState prevState)
    {
        switch (prevState)
        {
            case BehaviourState.Chase:
                agent.speed = walkSpeed;

                break;
            case BehaviourState.Wander:
            case BehaviourState.Dead:
            case BehaviourState.Attack:
                break;
            case BehaviourState.Find:
                break;
            default:
                break;
        }
    }

    public float findTime = 5;
    public float findTimeElapsed = 5;
    private void Update_Find()
    {
        findTimeElapsed -= Time.deltaTime;
        if (findTimeElapsed < 0)
        {
            State = BehaviourState.Wander;
        }
        if(PlayerFind())
        {
            State = BehaviourState.Chase;
        }
    }
    IEnumerator LookAround()
    {
        Vector3 left = transform.position - transform.right * 0.1f;
        Vector3 right = transform.position + transform.right * 0.1f;
        Vector3 back = transform.position - transform.forward * 0.1f;
        Vector3[] positions = { left , right, back};

        int index = 0;
        int length = positions.Length;
        while (true)
        {
            agent.SetDestination(positions[index]);
            index = (index + 1) % length;
            yield return new WaitForSeconds(1);
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        on_Update = Update_Wander;
        State = BehaviourState.Wander;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = sightRange;
    }
    private void OnEnable()
    {
    
    }
    private void Start()
    {
        player = GameManager.Inst.Player;
        State = BehaviourState.Wander;
    }
    private void Update()
    {
        on_Update();
       // SetDestination();

    }
    Collider[] playerCollider = new Collider[1];
    Transform target = null;
    bool PlayerFind()
    {
        bool result = false;
        //target != null &&
        if (target != null)
        {
            result = IsPlayerInSight(out _);
        }
    

        return result;
    }
    bool IsPlayerInSight(out Vector3 position)
    {
        bool result = false;
        position = Vector3.zero;
        if (target != null && Physics.OverlapSphereNonAlloc(transform.position, sightRange, playerCollider, LayerMask.GetMask("Player")) > 0)
        {
            Vector3 playerPos = playerCollider[0].transform.position;
            Vector3 dir = playerCollider[0].transform.position - transform.position;
            Ray ray = new(transform.position + transform.up, dir);
            if (Physics.Raycast(ray, out RaycastHit hitInfo ))
            {
                if (target.transform == hitInfo.transform)//플레이어가 맞았다.
                {
                    //추적상태로
                    float angle = Vector3.Angle(transform.forward, dir);
                    if (angle * 2 < sightAngle)//시야범위 안에 있다.
                    {
                        position = playerCollider[0].transform.position;
                        result = true;
                    }

                }
            }
        }

        return result;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }
    void Update_Wander()
    {
        //SetDestination();
        if (PlayerFind())//플레이어가 시야범위에 들어오면
        {
            State = BehaviourState.Chase;
        }
        else if ( agent.remainingDistance < 0.01f)
        {
            Vector3 position = NewDestination();
            agent.SetDestination(position);
        }
       // HP = maxHp;
       // agent.speed = walkSpeed;
       // speedPenalty = 0;
    }
    void Update_Chase()
    {
        if (IsPlayerInSight(out Vector3 newPos))
        {
            if ((newPos - transform.position).sqrMagnitude < 1.0f)
            {
                Debug.Log("공격범위 진입");
            }
            agent.SetDestination(newPos);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.01f)
        {
                //마지막 목격장소에 도착했는데 플레이어가 안보일 때
            State = BehaviourState.Find;
        }
        //마지막 목격한 장소까지 이동
        //이동후 플레이어가 없으면 다시 패트롤

    }
    void Update_Attack()
    {
        //
    }
    private void Update_Dead()
    {
        onDie?.Invoke(this);
        gameObject.SetActive(false);
    }
    void SetDestination()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.01f)
        {
            Vector3 destination = NewDestination();
            agent.SetDestination(destination);
        }
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
   
   
  
  
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;

        Quaternion leftAngle = Quaternion.AngleAxis(sightAngle * 0.5f, Vector3.up);
        Quaternion rightAngle = Quaternion.AngleAxis(-sightAngle * 0.5f, Vector3.up);
        Vector3 leftRotation = leftAngle * transform.forward;
        Vector3 rightRotation = rightAngle * transform.forward;
        Vector3 left = transform.position + leftRotation * sightRange;
        Vector3 right = transform.position + rightRotation * sightRange;
        Handles.DrawLine(transform.position, left, 3.0f);
        Handles.DrawLine(transform.position, right, 3.0f);

        switch (State)
        {
            case BehaviourState.Wander:
                Handles.color = Color.blue;
                break;
            case BehaviourState.Chase:
                Handles.color = Color.yellow;
                break;
            case BehaviourState.Attack:
                Handles.color = Color.red;
                break;
            case BehaviourState.Dead:
                Handles.color = Color.black;
                break;
            default:
                break;
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

