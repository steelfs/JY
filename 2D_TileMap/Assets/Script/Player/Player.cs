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
    Vector2 oldInputDir = Vector2.zero;// �̵��� ���������� �����ϱ� ���� ���� �����ϱ� ���� ����
    public float speed = 5.0f;

     float currentAttackCoolTime = 0.0f; //���� �����ִ� ��Ÿ��
    public float attackCoolTime = 0.5f;
    bool AttackReady => currentAttackCoolTime < 0;
    bool isAttackValid = false;// ������ ���� ��ȿ���� ǥ���ϴ� ���� (true�� ��ȿ)

    List<Slime> attackTargetList; //�÷��̾��� ���ݹ��� �ȿ� �����ִ� ��� ������
    Transform attackSensorAxis;

    bool isMove = false;// ������ ������ �� �̵�Ű�� ������ �ִٸ� ������ ����

    public Action<Vector2Int> onMapMoved;// �÷��̾ �ִ� ������� ����Ǿ��� �� ����(�Ķ���� = ������ ���� �׸��� ��ǥ)
    public Action<float, int> onDie;// �÷��̾ �׾��� �� ����� ��������Ʈ (�Ķ���� = ��ü �÷��� �ð�, ų ī��Ʈ)

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
        if (isAttack) //�������϶� ����� ����
        {
            oldInputDir = input;
        }
        else //�Ϲ����λ�Ȳ
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

    public void RestoreInputDir() //���� Ʈ������ ȣ��, �̵�Ű�� ������ ���� ���� ���� ���ͷ� ����
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



    public void AttackValid()// ���� �ִϸ��̼� ���� ������ ��ȿ�ϱ� �����ϸ�  �ִϸ����Ϳ��� ȣ��
    {
        isAttackValid = true;

        foreach(var slime in attackTargetList)
        {
            slime.Die();
        }
        attackTargetList.Clear();
    }
    public void AttackNotValid() // ���� �ִϸ��̼��� ���������� �ִϸ����Ϳ��� ȣ��
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
