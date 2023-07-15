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

     float currentAttackCoolTime = 0.0f; //현재 남아있는 쿨타임
    public float attackCoolTime = 0.5f;
    bool AttackReady => currentAttackCoolTime < 0;
    bool isAttackValid = false;// 공격이 현재 유효한지 표시하는 변수 (true면 유효)

    List<Slime> attackTargetList; //플레이어의 공격범위 안에 들어와있는 모든 슬라임
    Transform attackSensorAxis;

    bool isMove = false;// 공격이 끝났을 때 이동키를 누르고 있다면 복구를 위해

    public Action<Vector2Int> onMapMoved;// 플레이어가 있는 서브맵이 변경되었을 때 실행(파라미터 = 진입한 맵의 그리드 좌표)
    public Action<float, int> onDie;// 플레이어가 죽었을 때 실행될 델리게이트 (파라미터 = 전체 플레이 시간, 킬 카운트)

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
        Debug.Log(attackSensorAxis.rotation);
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * speed * inputDir);
      
    }

}
