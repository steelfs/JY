using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInputActions actions;
    public Action<Vector2> onMouseClick;
    public Action<Vector2> onMouseMove;
    public Action<float> onMouseWheel;

    private void Awake()
    {
        actions = new();
    }
    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.MouseClick.performed += OnMouseClick;
        actions.Player.MouseMove.performed += OnMouseMove;
        actions.Player.MouseWheel.performed += OnMouseWheel;
    }
    private void OnDisable()
    {
        actions.Player.MouseClick.performed -= OnMouseClick;
        actions.Player.MouseMove.performed -= OnMouseMove;
        actions.Player.MouseWheel.performed -= OnMouseWheel;
        actions.Player.Disable();
    }
    private void OnMouseClick(InputAction.CallbackContext _)
    {
        onMouseClick?.Invoke(Mouse.current.position.ReadValue());
    }

    private void OnMouseMove(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        onMouseMove?.Invoke(obj.ReadValue<Vector2>());
    }

    private void OnMouseWheel(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        onMouseWheel?.Invoke(obj.ReadValue<float>());
    }
    public void ResetBind()
    {
        onMouseClick = null;
        onMouseMove = null;
        onMouseWheel = null;
    }
}
