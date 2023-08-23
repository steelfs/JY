using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum PlayerAnimState
    {
        None,
        Idle,
        Walk,
        BackWalk
    }
    PlayerAnimState state = PlayerAnimState.None;
    PlayerAnimState State// 상태가 바뀔때만 animator의 트리거를 변경해서 특정상태 애니메이션 트리거가 백그라운드에 쌓이는 것을 방지하기 위함
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                anim.SetTrigger(state.ToString());// Enum.GetName(typeof(PlayerAnimState), value) 이렇게 할 필요 없이 state.ToString()이렇게만 하면 된다.
            }
        }
    }

    PlayerInputAction action;
    float dir;
    Quaternion rotateAngle;
    float rotate;

    CharacterController characterController;
    Animator anim;

    int idleHash = Animator.StringToHash("Idle");
    int WalkHash = Animator.StringToHash("Walk");
    int backWalkHash = Animator.StringToHash("WalkBack");

    public float moveSpeed = 3.0f;
    public float rotateSpeed = 90.0f;
    private void Awake()
    {
        action = new PlayerInputAction();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.MoveForward.performed += OnMove;
        action.Player.MoveForward.canceled += OnMove;
        action.Player.Rotate.performed += Rotate;
        action.Player.Rotate.canceled += Rotate;
    }



    private void OnDisable()
    {
        action.Player.MoveForward.performed -= OnMove;
        action.Player.MoveForward.canceled -= OnMove;
        action.Player.Rotate.performed -= Rotate;
        action.Player.Rotate.canceled -= Rotate;
        action.Player.Disable();
    }
    private void Update()
    {
        //characterController.Move(Time.deltaTime * moveSpeed * dir);
        //transform.localRotation = Quaternion.Slerp(transform.rotation, rotateAngle, Time.deltaTime * rotateSpeed);
        characterController.SimpleMove(dir * transform.forward);//transform.forward 를 곱해서 항상 나를 기준으로 이동하게 한다
        transform.Rotate(0, rotate * Time.deltaTime, 0, Space.World);
    }
    private void Rotate(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>();
        rotate = rotateInput * rotateSpeed;
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();
        dir = moveInput * moveSpeed; // 앞 뒤 정지와 이동속도 곱하기

        //dir.z = dir_.y;
        //dir.x = dir_.x;
        if (dir > 0.001f)// 전진 0.001 = float 오차를 감안한 임계값
        {
            State = PlayerAnimState.Walk;
            //rotateAngle = Quaternion.LookRotation(dir);
            //anim.SetTrigger(walkHash);
        }
        else if (dir < -0.001f)//후진
        {
            State = PlayerAnimState.BackWalk;
            //anim.SetBool(isWalkHash, false);
        }
        else//정지
        {
            State = PlayerAnimState.Idle;
        }
    }
}
