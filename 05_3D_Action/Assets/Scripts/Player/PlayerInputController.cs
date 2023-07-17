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
        characterController.Move(Time.deltaTime * currentSpeed * inputDir); // ���� ������ ����� ����
 
      //  characterController.SimpleMove(currentSpeed * inputDir); // �ڵ��� ����� ����
        
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDir.x = input.x;
        inputDir.z = input.y;
        inputDir.y =  -2.0f;// �ȳ����� ���

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

    //1. shiftŰ ���� �� �̵���� ���� OnMoveModeChange 3, 5
    //2. �̵� �ӵ��� ���� ����Ǵ� �ִϸ��̼� ���� blendTree -> Speed�� ����

    //3. W ������ ī�޶� ���� ���ʹ������� �̵�
        //A - ī�޶� ����
        //D - ������
        
        

}
