using UnityEngine;

public class PlayerTank : MonoBehaviour
{
    public Color baseColor;
    PlayerInputActions inputActions;
    float moveDir = 0;
    public float moveSpeed = 5.0f;
    public float turretRotateSpeed = 3;
    float rotateDir = 0;
    Quaternion turretRotation;
    Transform turret;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        turret = transform.GetChild(0).GetChild(3).transform;
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.RotateTurret.performed += OnRotate_Turret;
        inputActions.Player.Rotate.performed += On_Rotate;
        inputActions.Player.Rotate.canceled += On_Rotate;
    }

    private void On_Rotate(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        rotateDir = context.ReadValue<float>();
    }

    private void OnRotate_Turret(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 mouseScreenPosition = context.ReadValue<Vector2>();
        float distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;//z���� ī�޶�� ������Ʈ���� �Ÿ��� ��Ÿ��
        mouseScreenPosition.z = distanceToScreen;//z���� �Ÿ��� �־��� ��
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); //������ǥ�� ��ȯ
        Debug.Log(mouseWorldPosition);
        turret.LookAt(mouseWorldPosition);
        //rotation = Quaternion.LookRotation(mouseWorldPosition);
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
        transform.Rotate(0, rotateDir, 0);
    }
}
