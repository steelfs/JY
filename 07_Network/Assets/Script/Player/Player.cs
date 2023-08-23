using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInputAction action;
    Vector3 dir;
    Quaternion rotateAngle;
    CharacterController characterController;
    Animator anim;

    int walkHash = Animator.StringToHash("Walk");
    int isWalkHash = Animator.StringToHash("IsWalk");

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;
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
    private void Update()
    {
        characterController.Move(Time.deltaTime * moveSpeed * dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateAngle, Time.deltaTime * rotateSpeed);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 dir_ = context.ReadValue<Vector2>();
        dir.z = dir_.y;
        dir.x = dir_.x;
        if (dir != Vector3.zero)
        {
           // anim.SetBool(isWalkHash, true);
            rotateAngle = Quaternion.LookRotation(dir);
            anim.SetTrigger(walkHash);
        }
        else
        {
            //anim.SetBool(isWalkHash, false);
        }
    }
}
