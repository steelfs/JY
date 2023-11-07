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
        float distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;//z���� ī�޶�� ������Ʈ���� �Ÿ��� ��Ÿ��
        mouseScreenPosition.z = distanceToScreen;//z���� �Ÿ��� �־��� ��
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); //������ǥ�� ��ȯ
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
