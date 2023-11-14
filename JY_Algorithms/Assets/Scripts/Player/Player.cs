using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputState
{
    Player,
    UI
}
public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;
    public float moveSpeed = 10.0f;
    Vector3 moveDir = Vector3.zero;

    Animator anim;
    int walkHash = Animator.StringToHash("walk");
    int idleHash = Animator.StringToHash("idle");

    InputState inputState;
    public InputState InputState
    {
        get => inputState;
        set
        {
            inputState = value;
            switch (inputState)
            {
                case InputState.Player:
                    Enable_PlayerInput();
                    break;
                case InputState.UI:
                    Disable_PlayerInput();
                    break;
            }
        }
    }
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MoveForward_BackWard.performed += OnMove_Forward_Backward;
        inputActions.Player.MoveForward_BackWard.canceled += OnMove_Forward_Backward;
        inputActions.Player.MoveRight_Left.performed += OnMove_Right_Left;
        inputActions.Player.MoveRight_Left.canceled += OnMove_Right_Left;

        inputActions.UI.Enable();
        inputActions.UI.CloseQuestionPanel.performed += OnCloseQuestionPanel;
        inputActions.UI.Enter.performed += OnPressEnter;
    }

    private void OnPressEnter(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        GameManager.QuestionPanel.FreeInput_Accepted();
    }

    private void OnCloseQuestionPanel(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        GameManager.Inst.CloseQuestionPanel();
    }

    void Enable_PlayerInput()
    {
        inputActions.Player.Enable();
    }
    void Disable_PlayerInput()
    {
        inputActions.Player.Disable();
    }
    void Enable_UI_Input()
    {
        inputActions.UI.Enable();
    }
    void Disable_UI_Input()
    {
        inputActions.UI.Disable();
    }
    private void OnMove_Right_Left(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            anim.SetTrigger(walkHash);
        }
        else
        {
            anim.SetTrigger(idleHash);
        }
        float dir = context.ReadValue<float>();
        moveDir.x = dir;
        if (dir > 0)
        {
            transform.rotation = Quaternion.Euler(0,90,0);
        }
        else if (dir < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    private void OnMove_Forward_Backward(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            anim.SetTrigger(walkHash);
        }
        else
        {
            anim.SetTrigger(idleHash);
        }
        float dir = context.ReadValue<float>();
        moveDir.z = dir;
        if (dir > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (dir < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * moveDir, Space.World);

    }
}
