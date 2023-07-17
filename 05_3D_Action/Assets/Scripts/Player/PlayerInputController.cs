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
    float currentSpeed = 5.0f;

    int speedHash = Animator.StringToHash("Speed");

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
                    break;
                case MoveMode.Walk:
                    currentSpeed = walkSpeed;
                    break;
            }
        }
    }

    Vector3 inputDir;

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

    private void OnDisable()
    {
        action.Player.Move.performed -= OnMove;
        action.Player.Move.canceled -= OnMove;
        action.Player.Disable();
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDir); // 비교적 수동에 가까운 느낌
 
      //  characterController.SimpleMove(currentSpeed * inputDir); // 자동에 가까운 느낌
        
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDir.x = input.x;
        inputDir.z = input.y;
        inputDir.y =  -2.0f;// 안내려갈 경우

        float moveSpeed = 0.0f;
        if (inputDir.x != 0 && inputDir.z != 0)
        {
            moveSpeed = Mathf.Abs((inputDir.x + inputDir.z)  * 0.5f);
        }
        else if (inputDir.x != 0 )
        {
            moveSpeed = Mathf.Abs(inputDir.x);
        }
        else if (inputDir.z != 0)
        {
            moveSpeed = Mathf.Abs(inputDir.z);
        }
        anim.SetFloat(speedHash, moveSpeed); 

       
    }

    //1. shift키 누를 시 이동모드 변경 OnMoveModeChange 3, 5
    //2. 이동 속도에 따라 재생되는 애니메이션 변경 blendTree -> Speed값 조정

    //3. W 누르면 카메라 기준 앞쪽방향으로 이동
        //A - 카메라 왼쪽
        //D - 오른쪽
        
        

}
