using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState
{
    None,
    Player,
    UI
}
public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;
    Vector3 moveDir = Vector3.zero;
    Vector3 destination;
    Animator anim;

    public float moveSpeed = 10.0f;
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
                case InputState.None:
                    Disable_PlayerInput();
                    Disable_UI_Input();
                    break;
                case InputState.Player:
                    Enable_PlayerInput();
                    Disable_UI_Input();
                    break;
                case InputState.UI:
                    Enable_UI_Input();
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
        inputActions.Player.MoveClick.performed += OnMoveClick;

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
    private void OnMoveClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

        Vector2Int grid = Util.WorldToGrid(world);//클릭한 곳의 그리드상 위치
        CellVisualizer to = GameManager.Visualizer.CellVisualizers[Util.GridToIndex(grid.x, grid.y)];
        destination = to.transform.position;
        Vector2Int currentGridPos = Util.WorldToGrid(transform.position);//현재 캐릭터의 오브젝트 위치
        CellVisualizer from = GameManager.Visualizer.CellVisualizers[Util.GridToIndex(currentGridPos.x, currentGridPos.y)];

        if (GameManager.Visualizer.IsMovable(from, to) && Util.IsNeighbor(currentGridPos, grid, 2))
        {
            this.moveDir = (to.transform.position - transform.position).normalized;
        }
    }
    private void Update()
    {
        float distance = (destination.x - transform.position.x) + (destination.z - transform.position.z);
        if (distance > 0.1f)
        {
            transform.Translate(Time.deltaTime * moveSpeed * moveDir, Space.World);
        }
        else
        {
            moveDir = Vector3.zero;
        }

    }
}
