using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTank : MonoBehaviour
{
    public Color baseColor;
    PlayerInputActions inputActions;
    Vector2 moveDir = Vector2.zero;
    public float moveSpeed = 5.0f;
    public float turretRotateSpeed = 3;
    public float rotateSpeed = 180;
    float rotateDir = 0;
    Quaternion turretRotation;
    Quaternion lookTarget = Quaternion.identity;
    Transform turret;

    Renderer bodyRenderer;
    Renderer turretRanderer;
    Rigidbody rb;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        turret = transform.GetChild(0).GetChild(3).transform;
        bodyRenderer = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        turretRanderer = transform.GetChild(0).GetChild(3).GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        lookTarget = turret.rotation;
    }
    private void Start()
    {

        bodyRenderer.material.SetColor("_BaseColor", baseColor);
       // turretRanderer.material.color = baseColor;
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        //inputActions.Player.RotateTurret.performed += OnRotate_Turret;
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
        float distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;//z값은 카메라와 오브젝트간의 거리를 나타냄
        mouseScreenPosition.z = distanceToScreen;//z값에 거리를 넣어준 뒤
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); //월드좌표로 변환
        turret.LookAt(mouseWorldPosition);
        Debug.Log(mouseWorldPosition);
        //rotation = Quaternion.LookRotation(mouseWorldPosition);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
    private void Update()
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screen);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, LayerMask.GetMask("Ground")))
        {
            Vector3 lookDir = hitInfo.point - turret.position;
            lookDir.y = 0;
            lookTarget = Quaternion.LookRotation(lookDir, Vector3.up);
            //turret.rotation = Quaternion.Slerp();
        }
        turret.rotation = Quaternion.Slerp(turret.rotation, lookTarget, Time.deltaTime * turretRotateSpeed);
       
    }
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (Time.deltaTime * moveSpeed * moveDir.y) * transform.forward);
        rb.MoveRotation(Quaternion.Euler(0,Time.deltaTime * rotateSpeed * moveDir.x, 0) * transform.rotation);
    }
 
}
