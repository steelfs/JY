using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;
    public float moveSpeed = 5.0f;
    Vector2 moveDir = Vector2.zero;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
        moveDir *= Time.deltaTime * moveSpeed;
        Debug.Log($"X_{moveDir.x}__Y_{moveDir.y}");
    }
    private void Update()
    {
        transform.Translate(moveDir.x, 0, moveDir.y, Space.World);
    }
}
