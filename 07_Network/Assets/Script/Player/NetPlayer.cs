using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetPlayer : NetworkBehaviour
{
    PlayerInputAction action;
    CharacterController controller;

    

    Logger logger;
    NetworkVariable<FixedString512Bytes> chatString = new NetworkVariable<FixedString512Bytes>();//지금 내가 보낸 채팅

    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();//플레이어의 위치를 조정할 변수, 생성자로 읽기, 쓰기 권한을 조정할 수 있다.
    NetworkVariable<float> netMoveDir = new NetworkVariable<float>(); //입력받은 전진 / 후진 정도
    NetworkVariable<float> netRotateDir = new NetworkVariable<float>();
   // NetworkVariable<FixedString32Bytes> = new NetworkVariable<FixedString32Bytes>;

    float moveSpeed = 3.0f;
    float rotateSpeed = 180.0f;

    //float rotateDir;
    //float moveDir;
    //실 행 순서 awake - enable - OnNetworkSpawn - start
    //Isowner  true 가되는 시점 -> OnNetworkSpawn
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        position.OnValueChanged += OnPositionChange;//position 의 value가 바뀔때 실행되는 델리게이트
        chatString.OnValueChanged += OnChatRecieve;
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

    private void OnPositionChange(Vector3 previousValue, Vector3 newValue) //네트워크 변수  position이 변경되었을 때 실행될 함수  previousValue = 변경되기 전 값
    {
        //controller.transform.position = newValue;
        transform.position = newValue;

    }
}
