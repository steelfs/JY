using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum PlayerAnimState
    {
        Idle,
        Walk,
        BackWalk
    }
    PlayerAnimState state = PlayerAnimState.BackWalk;
    PlayerAnimState State// ���°� �ٲ𶧸� animator�� Ʈ���Ÿ� �����ؼ� Ư������ �ִϸ��̼� Ʈ���Ű� ��׶��忡 ���̴� ���� �����ϱ� ����
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                anim.SetTrigger(state.ToString());
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
        action.Player.Move.performed += OnMove;
        action.Player.Move.canceled += OnMove;
    }
    private void OnDisable()
    {
        action.Player.Move.performed -= OnMove;
        action.Player.Move.canceled -= OnMove;
        action.Player.Disable();
    }
    private void Update()
    {
        //characterController.Move(Time.deltaTime * moveSpeed * dir);
        //transform.localRotation = Quaternion.Slerp(transform.rotation, rotateAngle, Time.deltaTime * rotateSpeed);
        characterController.SimpleMove(dir * transform.forward);//transform.forward �� ���ؼ� �׻� ���� �������� �̵��ϰ� �Ѵ�
        transform.Rotate(0, rotate * Time.deltaTime, 0, Space.World);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 dir_ = context.ReadValue<Vector2>();
        dir = dir_.y * moveSpeed; // �� �� ������ �̵��ӵ� ���ϱ�

        //dir.z = dir_.y;
        //dir.x = dir_.x;
        if (dir > 0.001f)// ���� 0.001 = float ������ ������ �Ӱ谪
        {
            State = PlayerAnimState.Walk;
            //rotateAngle = Quaternion.LookRotation(dir);
            //anim.SetTrigger(walkHash);
        }
        else if (dir < -0.001f)//����
        {
            State = PlayerAnimState.BackWalk;
            //anim.SetBool(isWalkHash, false);
        }
        else//����
        {
            State = PlayerAnimState.Idle;
        }
        rotate = dir_.x * rotateSpeed;// ��, ��  ȸ�� �ӵ� ���ؼ� ����

    }
}
