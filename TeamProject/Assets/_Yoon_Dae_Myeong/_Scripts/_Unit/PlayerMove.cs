using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerMove : MonoBehaviour
{
	int isWalkingHash = Animator.StringToHash("IsWalking");
	int runHash = Animator.StringToHash("Run");
	int jumpHash = Animator.StringToHash("Jump");

	 Animator anim;
	Camera mainCamera;

	Vector3 moveDirection;
	/// <summary>
	/// 외부에서 수정할값
	/// </summary>
	Quaternion lookDir = Quaternion.identity;
	/// <summary>
	/// 이동할 값 
	/// </summary>
	Quaternion moveDir = Quaternion.identity;

	public float defaultMoveSpeed = 2.0f;
	public float runSpeed = 5.0f;

	bool running = false;
	bool Running
	{
		get => running;
		set
		{
			running = value;
			if (running)
			{
				moveSpeed = runSpeed;
			}
			else
			{
				moveSpeed = defaultMoveSpeed;
			}
		}
	}
	bool enable_KeyBoard_Move = true;
	bool Enable_KeyBoard_Move
	{
		get => enable_KeyBoard_Move;
		set
		{
			if (enable_KeyBoard_Move != value)
			{
                enable_KeyBoard_Move = value;
                switch (value)
                {
                    case true:
                        inputAction.Player.Move.performed += OnMove;
                        inputAction.Player.Move.canceled += OnMove;
						rb.isKinematic = false;
                        break;
                    case false:
                        inputAction.Player.Move.performed -= OnMove;
                        inputAction.Player.Move.canceled -= OnMove;
                        rb.isKinematic = true;
                        break;
                }
            }
		}
	}


    float moveSpeed = 2.0f;
	//float rotateSpeed = 10.0f;
	//BoxCollider target = null;
	
	InputKeyMouse inputAction;
	Rigidbody rb;

	WaitForSeconds jump_Duration;

	private void Awake()
	{
		inputAction = new InputKeyMouse();
		anim = GetComponent<Animator>();
		jump_Duration = new WaitForSeconds(0.9f);
		rb = GetComponent<Rigidbody>();
		Enable_KeyBoard_Move = false;
    }
	private void OnEnable()
	{
		inputAction.Player.Enable();
        inputAction.Player.MoveMode_Change.performed += On_MoveMode_Change;
        inputAction.Player.Run.performed += Run;
        inputAction.Player.Jump.performed += Jump;
        //inputClick.Test.Test3.performed += onUnitDie;
        CameraOriginTarget battleFollowCamera = FindObjectOfType<CameraOriginTarget>(true); //회전값 받아오기위해 찾기 
        if (battleFollowCamera != null)
        {
            battleFollowCamera.cameraRotation += SetCameraRotaion; //회전값받아오기위해 연결
        }
    }

    private void Jump(InputAction.CallbackContext _)
    {
		anim.SetTrigger(jumpHash);
		rb.AddForce(5 * transform.up, ForceMode.Impulse);
    }

    private void Run(InputAction.CallbackContext _)
    {
		Running = !Running;
    }

    private void On_MoveMode_Change(InputAction.CallbackContext _)
    {
        Enable_KeyBoard_Move = !Enable_KeyBoard_Move;
    }

    private void OnDisable()
    {
        CameraOriginTarget battleFollowCamera = FindObjectOfType<CameraOriginTarget>(true); //회전값 받아오기위해 찾기
        if (battleFollowCamera != null)
        {
            battleFollowCamera.cameraRotation -= SetCameraRotaion;//회전값받아오기위해 연결
        }
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Disable();
        //inputClick.Test.Test3.performed -= onUnitDie;
    }
    /// <summary>
    /// 배틀맵에서 카메라 돌아가면 캐릭터 방향도 같이 수정한다.
    /// </summary>
    /// <param name="quaternion">카메라 회전방향 </param>
    private void SetCameraRotaion(Quaternion quaternion)
    {
		lookDir = quaternion;
		//Debug.Log($"카메라가 움직였네 값은: {quaternion}");
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void OnMove(InputAction.CallbackContext context)
    {
		Vector3 dir = context.ReadValue<Vector2>();
        dir.z = dir.y;
        dir.y = 0.0f;
		moveDirection = lookDir * dir; //이동방향설정
		
		if (!context.canceled)
		{
            moveDir = Quaternion.LookRotation(lookDir * dir); //카메라 방향에 맞춰서 방향을 결정한다.
            if (running)
			{
				anim.SetBool(runHash, true);
			}
			else
			{
                anim.SetBool(isWalkingHash, true);
            }
        }
		else
		{
            anim.SetBool(isWalkingHash, false);
			anim.SetBool(runHash, false);
        }

    }

 //   private void FixedUpdate()
	//{
	//	//MoveByKeyBoard();
 //   }
	//void MoveByKeyBoard()//update 호출
	//{
 //       transform.Translate(Time.fixedDeltaTime * moveSpeed * moveDirection, Space.World);
	//	transform.rotation = Quaternion.Slerp(transform.rotation, moveDir, Time.fixedDeltaTime * rotateSpeed);
	//	//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetObj.position - transform.position), Time.fixedDeltaTime * rotateSpeed);


	//}

}
