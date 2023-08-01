using System;
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
    int skillHash = Animator.StringToHash("Skill");
    int dieHash = Animator.StringToHash("Die");

    Vector3 rotateAngle;

    public Action onItemPickUp;//������ �ݱ��ư ���� �� ��ȣ ������

    InventoryUI inventoryUI;

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
        action.Player.Skill.performed += Skill_performed;
        action.Player.Die.performed += Die_performed;
        action.Player.PickUp.performed += OnPickUp;
    }

    void SetAttackValid(bool attack) //�κ��丮�� ���� �� InventoryUI���� ȣ��
    {
        if (attack)
        {
            action.Player.Attack.performed += OnAttack;
            action.Player.Skill.performed += Skill_performed;
        }
        else
        {
            action.Player.Attack.performed -= OnAttack;
            action.Player.Skill.performed -= Skill_performed;
        }
    }
    private void OnPickUp(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onItemPickUp?.Invoke();
        
    }

    private void Die_performed(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        anim.SetTrigger(dieHash);
    }

    private void Skill_performed(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        anim.SetTrigger(skillHash);
    }

    private void OnDisable()
    {
        action.Player.Move.performed -= OnMove;
        action.Player.Move.canceled -= OnMove;
        action.Player.ChangeMode.performed -= OnMoveModeChange;
        action.Player.Attack.performed -= OnAttack;
        action.Player.PickUp.performed -= OnPickUp;
        action.Player.Disable();
    }
    private void Start()
    {
        MoveSpeedMode = MoveMode.Walk;
        inventoryUI = GameManager.Inst.InvenUI;
        inventoryUI.onInventoryOpen += SetAttackValid;
        if (inventoryUI != null)
        {
         //   inventoryUI.onInventoryOpen_ += action.Player.Attack.
        }
    }
    private void Update()
    {
       characterController.Move(Time.deltaTime * currentSpeed * inputDir); // ���� ������ ����� ����   
                                                                            //  characterController.SimpleMove(currentSpeed * inputDir); // �ڵ��� ����� ����
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
        anim.SetTrigger(attackHash);
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDir.x = input.x;
        inputDir.z = input.y;
        inputDir.y =  -2.0f;// �ȳ����� ���

        float moveSpeed = inputDir.x != 0 && inputDir.z != 0 ? Mathf.Max(Mathf.Abs(inputDir.x), inputDir.z) : inputDir.x != 0 ? Mathf.Abs(inputDir.x) : inputDir.z != 0 ? Mathf.Abs(inputDir.z) : 0;

        anim.SetFloat(speedHash, moveSpeed);

        Quaternion camYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);// ī�޶��� y�� ȸ���� ��������
        inputDir.y = 0.0f; // �ռ� �ٴ����� �������� �ʴ� ���� ������ Y ���� -2 �߱� ������ ���� �ٽ� �ʱ�ȭ ������ ������ x���� ȸ���ϰ� �ȴ�.
        inputDir = camYRotation * inputDir;// ī�޶��� Y�� ȸ������ �Է� ���⿡ ���ؼ� ���� ȸ����Ű��// ������ �� ����������� �׻� ���� �������� ȸ���ϰ� �ȴ�. 

        if (!context.canceled)
            targetRotation = Quaternion.LookRotation(inputDir);

      //  StartCoroutine(MovingCoroutine());
        //�ڷ�ƾ���� �̵�?
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
    //IEnumerator MovingCoroutine()
    //{
    //    while (inputDir != Vector3.zero)
    //    {
    //        characterController.Move(Time.deltaTime * currentSpeed * inputDir); // ���� ������ ����� ����   
    //        yield return null;
    //    }
    //    yield break;
    //}

}