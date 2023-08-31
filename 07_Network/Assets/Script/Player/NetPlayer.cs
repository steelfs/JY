using Cinemachine;
using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetPlayer : NetworkBehaviour
{
    PlayerInputAction action;
    CharacterController controller;
    Animator anim;
    public string userName;

    Logger logger;
    NetworkVariable<FixedString512Bytes> chatString = new NetworkVariable<FixedString512Bytes>();//지금 내가 보낸 채팅
    public NetworkVariable<FixedString32Bytes> nameString = new NetworkVariable<FixedString32Bytes>();

    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();//플레이어의 위치를 조정할 변수, 생성자로 읽기, 쓰기 권한을 조정할 수 있다.
    NetworkVariable<float> netMoveDir = new NetworkVariable<float>(); //입력받은 전진 / 후진 정도
    NetworkVariable<float> netRotateDir = new NetworkVariable<float>();
   // NetworkVariable<FixedString32Bytes> = new NetworkVariable<FixedString32Bytes>;

    float moveSpeed = 3.0f;
    float rotateSpeed = 180.0f;

    enum PlayerAnimState
    {
        None,
        Idle,
        Walk,
        BackWalk
    }
    PlayerAnimState state = PlayerAnimState.None;
    NetworkVariable<PlayerAnimState> netAnimState = new NetworkVariable<PlayerAnimState>();
    //PlayerAnimState State// 상태가 바뀔때만 animator의 트리거를 변경해서 특정상태 애니메이션 트리거가 백그라운드에 쌓이는 것을 방지하기 위함
    //{
    //    get => state;
    //    set
    //    {
    //        if (state != value)
    //        {
    //            state = value;
    //            anim.SetTrigger(state.ToString());// Enum.GetName(typeof(PlayerAnimState), value) 이렇게 할 필요 없이 state.ToString()이렇게만 하면 된다.
    //        }
    //    }
    //}



    //float rotateDir;
    //float moveDir;
    //실 행 순서 awake - enable - OnNetworkSpawn - start
    //Isowner  true 가되는 시점 -> OnNetworkSpawn
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        position.OnValueChanged += OnPositionChange;//position 의 value가 바뀔때 실행되는 델리게이트
        chatString.OnValueChanged += OnChatRecieve;
        anim = GetComponent<Animator>();

        netAnimState.OnValueChanged += OnAnimStateChange;
    }

 

    public override void OnNetworkSpawn()//나 뿐만 아니라 다른 오브젝트가 스폰됐을 때도 실행이 되는 함수 이기때문에 Owner인지 체크를 하지 않으면 다른 오브젝트가 실행됐을 때도 실행이 된다.
    {
        if (IsOwner)
        {
            action = new();//오너일 때만
            action.Player.Enable();
            action.Player.MoveForward.performed += OnMove;
            action.Player.MoveForward.canceled += OnMove;
            action.Player.Rotate.performed += OnRotate;
            action.Player.Rotate.canceled += OnRotate;

            SetSpawnPos();

            GameManager.Inst.Vcam.Follow = transform.GetChild(0);
            GameManager.Inst.onUserNameChange += SetOwnName;
        }
    }
    void SetOwnName(string newName)
    {
        if (IsServer)
        {
            nameString.Value = newName;
        }
        else
        {
            UpdateNameStateServerRpc(newName);
        }
    }

    public override void OnNetworkDespawn() // 네트워크 오브젝트가 디스폰 됐을 때 실행되는 함수 
    {
        if (IsOwner && action != null)
        {
            action.Player.MoveForward.performed -= OnMove;
            action.Player.MoveForward.canceled -= OnMove;
            action.Player.Rotate.performed -= OnRotate;
            action.Player.Rotate.canceled -= OnRotate;
            action.Player.Disable();
            action = null;
        }
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        float moveDir = moveSpeed * input;

        if (NetworkManager.Singleton.IsServer)// 서버이면 네트워크변수 직접 바로 변경
        {
            netMoveDir.Value = moveDir;
        }
        else
        {
            Move_RequestServerRpc(moveDir);//클라이언트면 RPC를 통해 수정요청
        }

        if (moveDir > 0.001f)// 전진 0.001 = float 오차를 감안한 임계값
        {
            state = PlayerAnimState.Walk;
            //rotateAngle = Quaternion.LookRotation(dir);
            //anim.SetTrigger(walkHash);
        }
        else if (moveDir < -0.001f)//후진
        {
            state = PlayerAnimState.BackWalk;
            //anim.SetBool(isWalkHash, false);
        }
        else//정지
        {
            state = PlayerAnimState.Idle;
        }

        //상태가 변경되면 네트워크상태도 같이 변경
        if (state != netAnimState.Value)
        {
            if (IsServer)
            {
                netAnimState.Value = state;
            }
            else
            {
                UpdateAnimStateServerRpc(state);
            }
        }
    }
    private void OnRotate(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>();
        float rotateDir = rotateSpeed * rotateInput;
        if (NetworkManager.Singleton.IsServer)
        {
            netRotateDir.Value = rotateDir;
        }
        else
        {
            Rotate_RequestServerRpc(rotateDir);
        }
    }
    private void Update()
    {
       controller.SimpleMove(netMoveDir.Value * transform.forward);
       transform.Rotate(0, Time.deltaTime * netRotateDir.Value, 0, Space.World);
    }
    


    private void SetSpawnPos()
    {
        Vector3 newPos = UnityEngine.Random.insideUnitSphere;
        newPos.y = 0;
        if (NetworkManager.Singleton.IsServer)
        {
            position.Value = newPos;
        }
        else
        {
            submitPos_RequestServerRpc(newPos);
        }
    }
    private void OnChatRecieve(FixedString512Bytes previousValue, FixedString512Bytes newValue)
    {
        GameManager.Inst.Log(newValue.ToString());
    }
    public void SendChat(string message)//채팅을 전송하는 함수 
    {
        if (IsServer)
        {
            chatString.Value = message; // 내가 서버면 직접 수정
        }
        else
        {
            RequestChat_ServerRpc(message);
        }
    }
    private void OnAnimStateChange(PlayerAnimState previousValue, PlayerAnimState newValue)//netAnimState가 변경되었을 때
    {
        anim.SetTrigger(newValue.ToString());
    }

    [ServerRpc]
    public void RequestChat_ServerRpc(string text)
    {
        chatString.Value = text;
    }
    [ServerRpc]// RPC = Remote procedure Call 원격함수 호출
    void submitPos_RequestServerRpc(Vector3 newPos)// 함수 이름의 끝은 반드시 ServerRpc 이어야 한다 아니면 오류발생
    {
        position.Value = newPos;
    }

    [ServerRpc]
    void Move_RequestServerRpc(float move)// 함수 이름의 끝은 반드시 ServerRpc 이어야 한다 아니면 오류발생
    {
        netMoveDir.Value = move;
    }
    [ServerRpc]
    void Rotate_RequestServerRpc(float rotate)// 함수 이름의 끝은 반드시 ServerRpc 이어야 한다 아니면 오류발생
    {
        netRotateDir.Value = rotate;
    }

    [ServerRpc]
    void UpdateAnimStateServerRpc(PlayerAnimState newState)// 함수 이름의 끝은 반드시 ServerRpc 이어야 한다 아니면 오류발생
    {
        netAnimState.Value = newState;
    }

    [ServerRpc]
    void UpdateNameStateServerRpc(string newName)// 함수 이름의 끝은 반드시 ServerRpc 이어야 한다 아니면 오류발생
    {
        nameString.Value = newName;
    }

    private void OnPositionChange(Vector3 previousValue, Vector3 newValue) //네트워크 변수  position이 변경되었을 때 실행될 함수  previousValue = 변경되기 전 값
    {
        //controller.transform.position = newValue;
        transform.position = newValue;

    }
}
