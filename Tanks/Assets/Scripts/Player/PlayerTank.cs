using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    public Color baseColor;
    PlayerInputActions inputActions;
    float moveDir = 0;
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 3;
    Quaternion rotation;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Rotate.performed += OnRotate;
    }

    private void OnRotate(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 mouseScreenPosition = context.ReadValue<Vector2>();
        float distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;//z값은 카메라와 오브젝트간의 거리를 나타냄
        mouseScreenPosition.z = distanceToScreen;//z값에 거리를 넣어준 뒤
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); //월드좌표로 변환
        Debug.Log(mouseWorldPosition);

        rotation = Quaternion.LookRotation(mouseWorldPosition);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<float>();
    }
    private void Update()
    {
        Move();
        Rotate();
    }
    void Move()
    {
        transform.Translate((moveDir * moveSpeed) * Time.deltaTime * transform.forward, Space.World) ;
    }
    void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
    }
}
