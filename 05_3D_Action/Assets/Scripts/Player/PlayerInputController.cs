using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerInputController : MonoBehaviour
{
    PlayerInputAction action;
    Animator anim;

    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    float currentSpeed = 3.0f;



    int speedHash = Animator.StringToHash("Speed");
    int attackHash = Animator.StringToHash("Attack");

    Vector3 rotateAngle;

    enum MoveMode
    {
        Walk = 0,
        Run
    }

    MoveMode moveSpeedMode = MoveMode.Run;
    MoveMode MoveSpeedMode
    {
        get => moveSpeedMode;
        set
        {
            moveSpeedMode = value;
            switch (moveSpeedMode)
            {
                case MoveMode.Run:
                    currentSpeed = runSpeed;
                //   anim.SetFloat(speedHash, 1.0f);
                    break;
                case MoveMode.Walk:
                    currentSpeed = walkSpeed;
                //    anim.SetFloat(speedHash, 0.3f);
                    break;
                default:
                    currentSpeed = walkSpeed;
                    break;
            }
        }
    }

    Vector3 inputDir = Vector3.zero;
    Quaternion targetRotation = Quaternion.identity;
    public float turnSpeed = 5.0f;

    CharacterController characterController;
    private void Awake()
    {
        action = new PlayerInputAction();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += OnMove;
        action.Player.Move.canceled += OnMove;
        action.Player.ChangeMode.performed += OnMoveModeChange;
        action.Player.Attack.performed += OnAttack;
    }



    private void OnDisable()
    {
        action.Player.Move.performed -= OnMove;
        action.Player.Move.canceled -= OnMove;
        action.Player.ChangeMode.performed -= OnMoveModeChange;
        action.Player.Attack.performed -= OnAttack;
        action.Player.Disable();
    }
    private void Start()
    {
        MoveSpeedMode = MoveMode.Run;
    }
    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDir); // 비교적 수동에 가까운 느낌   
                                                                            //  characterController.SimpleMove(currentSpeed * inputDir); // 자동에 가까운 느낌
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);        
    }
    private void OnMoveModeChange(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        switch (MoveSpeedMode)
        {
            case MoveMode.Run:
                MoveSpeedMode = MoveMode.Walk;
                break;
            case MoveMode.Walk:
                MoveSpeedMode = MoveMode.Run;
                break;
        }
    }
    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if (currentSpeed != runSpeed)
        anim.SetTrigger("Attack");
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDir.x = input.x;
        inputDir.z = input.y;
        inputDir.y =  -2.0f;// 안내려갈 경우

        float moveSpeed = inputDir.x != 0 && inputDir.z != 0 ? Mathf.Max(Mathf.Abs(inputDir.x), inputDir.z) : inputDir.x != 0 ? Mathf.Abs(inputDir.x) : inputDir.z != 0 ? Mathf.Abs(inputDir.z) : 0;

        anim.SetFloat(speedHash, moveSpeed);

        Quaternion camYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);// 카메라의 y축 회전값 가져오기
        inputDir.y = 0.0f; // 앞서 바닥으로 내려오지 않는 현상 때문에 Y 값을 -2 했기 때문에 값을 다시 초기화 해주지 않으면 x축이 회전하게 된다.
        inputDir = camYRotation * inputDir;// 카메라의 Y축 회전값을 입력 방향에 곱해서 같이 회전시키기// 생략할 시 월드기준으로 항상 같은 방향으로 회전하게 된다. 

        if (!context.canceled)
            targetRotation = Quaternion.LookRotation(inputDir);

    
        //if (input != Vector2.zero)
        //{
        //    Vector3 rotateDir = transform.position + inputDir;
        //    rotateDir.y = 0;
        //    transform.LookAt(rotateDir);

        //    //if (inputDir.x != 0)
        //    //    transform.rotation = Quaternion.Euler(0, inputDir.x * 90, 0);
        //    //else if (inputDir.z == -1)
        //    //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //    //else if (inputDir.z == 1)
        //    //    transform.rotation = Quaternion.Euler(0, 0, 0);
        //}

    }

        
}
