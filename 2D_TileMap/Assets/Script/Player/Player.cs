using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInputAction action;
    Animator anim;

    Rigidbody2D rb;
    Vector2 inputDir = Vector2.zero;
    Vector2 oldInputDir = Vector2.zero;// 이동중 공격했을때 공격하기 전의 방향 저장하기 위한 벡터
    public float speed = 5.0f;

    private int killCount = int.MinValue;
    public int KillCount
    {
        get => killCount; 
        set
        {
            if (killCount != value)
            {
                killCount = value;
                onKillCountChange?.Invoke(value);
            }
        }
    }
    float totalPlayTime = 0.0f;



    float currentAttackCoolTime = 0.0f; //현재 남아있는 쿨타임
    public float attackCoolTime = 0.5f;
    bool AttackReady => currentAttackCoolTime < 0;
    bool isAttackValid = false;// 공격이 현재 유효한지 표시하는 변수 (true면 유효)

    List<Slime> attackTargetList; //플레이어의 공격범위 안에 들어와있는 모든 슬라임
    Transform attackSensorAxis;
    WorldManager worldManager;

    bool isMove = false;// 공격이 끝났을 때 이동키를 누르고 있다면 복구를 위해

    Vector2Int currentMapPos;
    public Vector2Int CurrentMapPos//currentMapPos가 변경되면 델리게이트 실행(현재 플레이어가 위치하고 있는 맵의 좌표 )
    {
        get => currentMapPos;
        set
        {
            if (value != currentMapPos)
            {
                currentMapPos = value;
                onMapMoved?.Invoke(currentMapPos);//
            }
        }
    }
    
    public Action<Vector2Int> onMapMoved;// 플레이어가 있는 서브맵이 변경되었을 때 실행(파라미터 = 진입한 맵의 그리드 좌표)
    public Action<float, int> onDie;// 플레이어가 죽었을 때 실행될 델리게이트 (파라미터 = 전체 플레이 시간, 킬 카운트)
    public Action<int> onKillCountChange;

    public float maxLifeTime = 10.0f;
    float lifeTime;

    public float LifeTime
    {
        get => lifeTime;
        set
        {
            lifeTime = value;
            if (lifeTime < 0 && !isDead)
            {
                Die();
            }
            else
            {   //살아있는 상태면 최소 0 최대 maxLifeTime 으로 클램프 
                lifeTime = Mathf.Clamp(lifeTime, 0.0f, maxLifeTime);// lifeTime이  maxLifeTime을 초과할 수 없도록 MathfClamp 사용
            }
            onLifeTimeChange?.Invoke(lifeTime / maxLifeTime);// 수명이 변했음을 알림
        }
    }
    public Action<float> onLifeTimeChange;

    bool isDead = false;
  
    public bool isAttack = false;

    readonly int inputXhash = Animator.StringToHash("InputX");
    readonly int inputYhash = Animator.StringToHash("InputY");
    readonly int IsMovehash = Animator.StringToHash("IsMove");
    readonly int Attackhash = Animator.StringToHash("Attack");



    private void Awake()
    {
        action = new PlayerInputAction();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        attackSensorAxis = transform.GetChild(0);

        attackTargetList = new List<Slime>(4);
        AttackSensor sensor = attackSensorAxis.GetComponentInChildren<AttackSensor>();
        sensor.onEnemyEnter += (slime) =>
        {
            if (isAttackValid)
            {
                slime.Die();
            }
            else
            {
                attackTargetList.Add(slime);
                slime.ShowOutLine();
            }
        };
        sensor.onEnemyExit += (slime) =>
        {
            attackTargetList.Remove(slime);
            slime.ShowOutLine(false);
        };
    }
    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += OnMove;
        action.Player.Move.canceled += OnStop; ;
        action.Player.Attack.performed += OnAttack;
    }
    private void OnDisable()
    {
        action.Player.Attack.performed -= OnAttack;
        action.Player.Move.canceled -= OnStop; ;
        action.Player.Move.performed -= OnMove;
        action.Player.Disable();
    }
    private void Start()
    {
        worldManager = GameManager.Inst.WorldManager;
        LifeTime = maxLifeTime;
        KillCount = 0;
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (isAttack) //공격중일때 백업에 저장
        {
            oldInputDir = input;
        }
        else //일반적인상황
        {
            inputDir = input;
            anim.SetFloat(inputXhash, inputDir.x);
            anim.SetFloat(inputYhash, inputDir.y);

            AttackSensorRotate(inputDir);
        }
        isMove = true;
        anim.SetBool(IsMovehash, true);
    }
    private void OnStop(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        inputDir = Vector2.zero;
        isMove = false;
        anim.SetBool(IsMovehash, false);
    }

    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if (AttackReady)
        {
            isAttack = true;
            anim.SetTrigger(Attackhash);
            oldInputDir = inputDir;
            inputDir = Vector2.zero;

            currentAttackCoolTime = attackCoolTime; 
        }

      //  inputDir = oldiInputDir;
 
    }

    public void RestoreInputDir() //블랜드 트리에서 호출, 이동키를 누르고 있을 때만 이전 벡터로 복구
    {
        if (isMove)
        {
            inputDir = oldInputDir;
            anim.SetFloat(inputXhash, inputDir.x);
            anim.SetFloat(inputYhash, inputDir.y);

            AttackSensorRotate(inputDir);
        }
        isAttack = false;
    }



    public void AttackValid()// 공격 애니메이션 도중 공격이 유효하기 시작하면  애니메이터에서 호출
    {
        isAttackValid = true;

        foreach(var slime in attackTargetList)
        {
            slime.Die();
        }
        attackTargetList.Clear();
    }
    public void AttackNotValid() // 공격 애니메이션이 끝나갈때쯤 애니메이터에서 호출
    {
        isAttackValid = false;
    }
    void AttackSensorRotate(Vector2 dir)
    {
        if (dir.y < 0)
        {
            attackSensorAxis.rotation = Quaternion.identity;
        }
        else if(dir.y > 0)
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, 180.0f);
        }
        else if (dir.x > 0)
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, 90.0f);
        }
        else if (dir.x < 0)
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, -90.0f);
        }
        else
        {
            attackSensorAxis.rotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
        totalPlayTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * speed * inputDir);
        CurrentMapPos = worldManager.worldToGrid(rb.position);
    }
    private void Die()
    {
        isDead = true;//죽었다고 표시 
        LifeTime = 0.0f; // 수명으 ㄹ깔끔하게 0으로 표시 
        action.Player.Disable();// 인풋시스템 비활성화
        onDie?.Invoke(totalPlayTime, killCount);//죽었다고 알리기
    }

    public void KillMonster(float bonus) // param = 추가될 LifeTime
    {
        if (!isDead)
        {
            LifeTime += bonus;
            KillCount++;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 collisionPos;
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            collisionPos = collision.transform.position;
            
        }
    }
    
}
