using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Tank : MonoBehaviour
{
    PlayerInputActions inputActions;

    float moveDir = 0;
    public float moveSpeed = 5.0f;
    float rotateDir = 0;
    public float rotateSpeed = 180.0f;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        //inputActions.Player.Rotate.performed += OnRotate;
        //inputActions.Player.Rotate.canceled += OnRotate;
        inputActions.Player.Rotate_Mouse.performed += Rotate_Mouse;
    }

    private void Rotate_Mouse(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPosition = context.ReadValue<Vector2>();
        float distanceFromCameraToObjects = Camera.main.nearClipPlane;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceFromCameraToObjects));
        Debug.Log(mouseWorldPosition);
        transform.LookAt(mouseWorldPosition);

        //transform.rotation = Quaternion.LookRotation()
    }

    private void OnRotate(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        rotateDir = context.ReadValue<float>();
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<float>();
    }


    private void Update()
    {
        Rotate();
        Move();
    }
    void Move()
    {
        transform.Translate((moveDir * moveSpeed) * Time.deltaTime * Vector3.forward, Space.Self);
    }
    void Rotate()
    {
        transform.Rotate(0,rotateDir * rotateSpeed * Time.deltaTime,0);
    }
}
