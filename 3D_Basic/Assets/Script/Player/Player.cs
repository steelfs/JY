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

    float moveDir = 0.0f;//이동방향 -1 후진, ~1 전진
    float rotateDir = 0.0f;//입력받은 회전방향 -1 좌회전 ~1 우회전
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
    public float LifeTime //수명 시간 변경될 때마다 신호 보내기 
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

    [SerializeField]//객체지향 or 속도(public)
    float jumpCoolTime = 0; //남아있는 쿨타임시간
    
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
    // const a = 10; 컴파일 타임에 값이 결정된다 선언시 값을 반드시 넣어줘야한다.
    // readonly = 런타임에 값이 결정된다. 따라서 값을 선언할때 대입해주지 않아도 된다.
    readonly int isMoveHash = Animator.StringToHash("IsMove"); // IsMove라는 문자열을 숫자로 바꿔서 저장해놓기
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


    private void UseItem(Iinteractable interactable)// 스위치 작동시 호출 
    {
        if (interactable.IsDirectUse)
        {
            interactable.Use();
        }
        
    }

    private void OnEnable()
    {
        inputActions.Player.Enable(); //액션맵 활성화
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
            //Quaternion.kEpsilon 근사값
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDir * transform.forward); 
            //현재 위치에서 초당 스피드의 속도로  transform.forward * movedir (정방향 or 역방향 or 정지)
        }


    }
    void Rotate()
    {
        //Quaternion.Euler() x,y,z축으로 얼마만큼 회전시킬 것인지를 파라미터로 받음 직접입력하거나 
        //Quaternion.AngleAxis 특정 축을 기준으로 몇도만큼 회전시킬 것인지를 파라미터로 받음
        //Quaternion.FromToRotation : 시작방향벡터에서 목표 방향벡터까지 회전을 만들어주는 함수
        //Quaternion.Lerp  시작회전에서 목표회전 으로  보간하는 함수
        //Quaternion.Slerp 시작회전에서 목표회전 으로  보간하는 함수(곡선으로 보간)
        //Quaternion.LookRotation  : 특정방향을 바라보는 회전을 만들어주는 함수
        //rigid.MoveRotation(Quaternion.LookRotation(dir));

        //회전은 현재회전에서 추가해줘야한다

        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDir, transform.up); //변화할 회전 구하기
        rigid.MoveRotation(rigid.rotation * rotate); //현재회전에 변화할 회전을 곱해서 현재각도에서  rotate만큼 추가로 회전한 결과 만들기          
    }
    private void OnJump(InputAction.CallbackContext _)
    {
        Jump();

    }
    //transform.position += (Time.deltaTime * speed * dir);
    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        //dir = context.ReadValue<Vector3>(); //인풋 값 받아오기
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
        //코루틴을 여기서 시작하면 점프가 되지 않은 상황에서도 코루틴이 호출되서 조건을 true바꿔버려서 안된다
        //rigid.AddExplosionForce 폭발물 사용
        //rigid.AddForceAtPosition 오브젝트의 특정지점에 힘을 가하고싶을때
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
            platformBase.onMove = OnRideMovingObject;//플렛폼베이스에 탑승하면 델리게이트 연결. 나가면 disconect
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
    void OnRideMovingObject(Vector3 delta)// 움직이는 물체에 탑승했을 때 물체가보내는 델리게이트를 처리하는 함수
    {
        rigid.MovePosition(rigid.position + delta);// delta만큼 추가로 움직임
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

            rigid.constraints = RigidbodyConstraints.None; // 물리잠금 전부 풀기
            Transform head = transform.GetChild(0);
            rigid.AddForceAtPosition(0.5f * (-transform.forward),head.position, ForceMode.Impulse);
            rigid.AddTorque(transform.up * 1.2f, ForceMode.Impulse);
        }
        //살아 있을때만 처리
        //입력 처리 중지
        // 물리잠금 전부풀기
        //살짝 뒤로밀어서 넘어트리기
        // 데굴데굴 구르기

        //죽는 애니메이션
        //사망표시
    }
    public void SetForceJumpMode()
    {
        duringJump = true;
    }
    public void SetSpeedDebuff(float ratio)//속도 디버프
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
