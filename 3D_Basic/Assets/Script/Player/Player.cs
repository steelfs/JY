using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputAction inputActions;
    Animator anim;
    Rigidbody rigid;

    float moveDir = 0.0f;//�̵����� -1 ����, ~1 ����
    float rotateDir = 0.0f;//�Է¹��� ȸ������ -1 ��ȸ�� ~1 ��ȸ��
    public float moveSpeed = 5.0f;
    float currentMoveSpeed = 5.0f;
    public float rotateSpeed = 180.0f;
    public float jumpPower = 7.0f;
    public float jumpCoolTimeMax = 5.0f;
    public bool isAlive = true;

    public Action<float> onLifeTimeChange;
    public Action onDie;

    float lifeTime = 10.0f;
    public float lifeTimeMax = 10.0f;
    public float LifeTime //���� �ð� ����� ������ ��ȣ ������ 
    {
        get => lifeTime;
        private set
        {
            if (!isclear)
            {
                lifeTime = value;
                if (lifeTime <= 0)
                {
                    lifeTime = 0;
                    Die();
                    onDie?.Invoke();
                }
                onLifeTimeChange?.Invoke(lifeTime);
            }

        }
    }

    [SerializeField]//��ü���� or �ӵ�(public)
    float jumpCoolTime = 0; //�����ִ� ��Ÿ�ӽð�
    
    float JumpCoolTime
    {
        get => jumpCoolTime;
        set
        {
            jumpCoolTime = value;
            if (jumpCoolTime < 0.0f)
            {
                jumpCoolTime = 0.0f;
            }
            onJumpCooltimeChange?.Invoke(jumpCoolTime/ jumpCoolTimeMax);
        }
    }
    
    bool IsJumpCoolEnd => (JumpCoolTime <= 0.0);
    Action<float> onJumpCooltimeChange;

    bool isclear = false;

    bool duringJump = false;

    Vector3 dir;
    // const a = 10; ������ Ÿ�ӿ� ���� �����ȴ� ����� ���� �ݵ�� �־�����Ѵ�.
    // readonly = ��Ÿ�ӿ� ���� �����ȴ�. ���� ���� �����Ҷ� ���������� �ʾƵ� �ȴ�.
    readonly int isMoveHash = Animator.StringToHash("IsMove"); // IsMove��� ���ڿ��� ���ڷ� �ٲ㼭 �����س���
    TrapBase trapBase;

    private void Awake()
    {
        inputActions = new();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        ItemUsechecker checker = GetComponentInChildren<ItemUsechecker>();
        checker.onItemUse += UseItem;
  
    }
    private void Start()
    {
        currentMoveSpeed = moveSpeed;
        LifeTime = lifeTimeMax;

        VirtualStick stick = FindObjectOfType<VirtualStick>();
        VirtualButton button = FindObjectOfType<VirtualButton>();
        if (stick != null)
        {
            stick.onMoveInput += (input) => SetInput(input, input != Vector2.zero);
        }
        if (button != null)
        {
            button.onClick += Jump;
            onJumpCooltimeChange += button.RefreshCoolDown;
        }
  
    }


    private void UseItem(Iinteractable interactable)// ����ġ �۵��� ȣ�� 
    {
        if (interactable.IsDirectUse)
        {
            interactable.Use();
        }
        
    }

    private void OnEnable()
    {
        inputActions.Player.Enable(); //�׼Ǹ� Ȱ��ȭ
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.started += OnJump;
        inputActions.Player.Use.performed += Use_performed;
        
    }
    private void Use_performed(InputAction.CallbackContext _)
    {
        anim.SetTrigger("Use");
    }

    
    private void OnDisable()
    {
        inputActions.Player.Use.performed -= Use_performed;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Jump.started -= OnJump;

        inputActions.Player.Disable();
    }
    private void Update()
    {
        JumpCoolTime -= Time.deltaTime;
        LifeTime -= Time.deltaTime;

    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        if (moveDir > 0.01f || moveDir < -0.01f)
        {
            //Quaternion.kEpsilon �ٻ簪
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDir * transform.forward); 
            //���� ��ġ���� �ʴ� ���ǵ��� �ӵ���  transform.forward * movedir (������ or ������ or ����)
        }


    }
    void Rotate()
    {
        //Quaternion.Euler() x,y,z������ �󸶸�ŭ ȸ����ų �������� �Ķ���ͷ� ���� �����Է��ϰų� 
        //Quaternion.AngleAxis Ư�� ���� �������� ���ŭ ȸ����ų �������� �Ķ���ͷ� ����
        //Quaternion.FromToRotation : ���۹��⺤�Ϳ��� ��ǥ ���⺤�ͱ��� ȸ���� ������ִ� �Լ�
        //Quaternion.Lerp  ����ȸ������ ��ǥȸ�� ����  �����ϴ� �Լ�
        //Quaternion.Slerp ����ȸ������ ��ǥȸ�� ����  �����ϴ� �Լ�(����� ����)
        //Quaternion.LookRotation  : Ư�������� �ٶ󺸴� ȸ���� ������ִ� �Լ�
        //rigid.MoveRotation(Quaternion.LookRotation(dir));

        //ȸ���� ����ȸ������ �߰�������Ѵ�

        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDir, transform.up); //��ȭ�� ȸ�� ���ϱ�
        rigid.MoveRotation(rigid.rotation * rotate); //����ȸ���� ��ȭ�� ȸ���� ���ؼ� ���簢������  rotate��ŭ �߰��� ȸ���� ��� �����          
    }
    private void OnJump(InputAction.CallbackContext _)
    {
        Jump();

    }
    //transform.position += (Time.deltaTime * speed * dir);
    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        //dir = context.ReadValue<Vector3>(); //��ǲ �� �޾ƿ���
        Vector2 input = context.ReadValue<Vector2>();
        SetInput(input, !context.canceled);
        //anim.SetBool("IsMove", context.performed);
    }

            //Vector2 jumpDirection = new Vector2(0, 7);
            //rigid.AddForce(jumpDirection, ForceMode.Impulse);
    void Jump()
    {
        if (!duringJump && IsJumpCoolEnd)
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
            JumpCoolTime = jumpCoolTimeMax;
            duringJump = true;
        }
        //�ڷ�ƾ�� ���⼭ �����ϸ� ������ ���� ���� ��Ȳ������ �ڷ�ƾ�� ȣ��Ǽ� ������ true�ٲ������ �ȵȴ�
        //rigid.AddExplosionForce ���߹� ���
        //rigid.AddForceAtPosition ������Ʈ�� Ư�������� ���� ���ϰ������
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            duringJump = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlatformBase platformBase = other.gameObject.GetComponent<PlatformBase>();
        if (platformBase != null)
        {
            platformBase.onMove = OnRideMovingObject;//�÷������̽��� ž���ϸ� ��������Ʈ ����. ������ disconect
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        PlatformBase platformBase = other.gameObject.GetComponent<PlatformBase>();
        if (platformBase != null)
        {
            platformBase.onMove = null;
        }
    }
    void OnRideMovingObject(Vector3 delta)// �����̴� ��ü�� ž������ �� ��ü�������� ��������Ʈ�� ó���ϴ� �Լ�
    {
        rigid.MovePosition(rigid.position + delta);// delta��ŭ �߰��� ������
    }
    IEnumerator _JumpCoolTime()
    {
        yield return new WaitForSeconds(3.0f);
        duringJump = false;
    }

    public void Die()
    {
        if (isAlive)
        {
            isAlive= false;
            anim.SetTrigger("Die");
            inputActions.Player.Disable();

            rigid.constraints = RigidbodyConstraints.None; // ������� ���� Ǯ��
            Transform head = transform.GetChild(0);
            rigid.AddForceAtPosition(0.5f * (-transform.forward),head.position, ForceMode.Impulse);
            rigid.AddTorque(transform.up * 1.2f, ForceMode.Impulse);
        }
        //��� �������� ó��
        //�Է� ó�� ����
        // ������� ����Ǯ��
        //��¦ �ڷιо �Ѿ�Ʈ����
        // �������� ������

        //�״� �ִϸ��̼�
        //���ǥ��
    }
    public void SetForceJumpMode()
    {
        duringJump = true;
    }
    public void SetSpeedDebuff(float ratio)//�ӵ� �����
    {
        currentMoveSpeed = moveSpeed * ratio;
    }
    public void RestoreSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }
    void SetInput(Vector2 input, bool isMove)
    {
        rotateDir = input.x;
        moveDir = input.y;

        anim.SetBool(isMoveHash, isMove);
    }
}
