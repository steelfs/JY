using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    PlayerInputAction action;
    private void Awake()
    {
        action = new PlayerInputAction();
    }
    private void OnEnable()
    {
        action.Effect.Enable();
        action.Effect.PointerMove.performed += OnPointerMove;            
    }

    private void OnDisable()
    {
        action.Effect.PointerMove.performed -= OnPointerMove;
        action.Effect.Disable();
    }

    private void OnPointerMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 mousePos = context.ReadValue<Vector2>();
        mousePos.z = 10.0f;

        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = target;
    }
}
