using Cinemachine;
using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
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

    float moveSpeed = 3.0f;
    float rotateSpeed = 180.0f;

    Transform fire_Pos;
    public GameObject bullet;
    public GameObject energy_Orb;
   
    Material bodyMat;
    NetworkVariable<bool> netEffectState = new NetworkVariable<bool>();
    public bool IsEffectOn
    {
        get => netEffectState.Value;
        set
        {
            if (netEffectState.Value != value)
            {
                if (IsServer)
                {
                    netEffectState.Value = value;
                }
                else
                {
                    UpdateEffectStateServerRpc(value);
                }
            }
        }
    }
    [ServerRpc]
    void UpdateEffectStateServerRpc(bool IsEffectOn)
    {
        netEffectState.Value = IsEffectOn;
    }
    //netEffectState true 일때 안면부 빛나기
    // 빛나는 모습은 다른 플레이어에게도 보여야함
    //접속시 이름판에 자신의 이름 설정
    enum PlayerAnimState
    {
        None,
        Idle,
        Walk,
        BackWalk
    }
    PlayerAnimState state = PlayerAnimState.None;
    NetworkVariable<PlayerAnimState> netAnimState = new NetworkVariable<PlayerAnimState>();

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

        Renderer renderer = transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>();
        bodyMat = renderer.material;
        netEffectState.OnValueChanged += OnEffectChange;
        fire_Pos = transform.GetChild(4).transform;
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
            action.Player.Attack01.performed += On_Attack01;
            action.Player.Attack02.performed += On_Attack02;

            SetSpawnPos();

            GameManager.Inst.Vcam.Follow = transform.GetChild(0);
            GameManager.Inst.VirtualPad.onMoveInput += (inputDir) => SetMoveInput(inputDir.y);
            GameManager.Inst.VirtualPad.onMoveInput += (inputDir) => SetRotateInput(inputDir.x);

            GameManager.Inst.VirtualPad.onAttack01Input = Attack01;
            GameManager.Inst.VirtualPad.onAttack02Input = Attack02;

        }
    }

    private void On_Attack01(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Attack01();
    }

    private void On_Attack02(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Attack02();
    }

    public void Attack01()
    {
        RequestSpawnServerRpc();
    }
    public void Attack02()
    {
        RequestSpawnEnergyOrbServerRpc();
    }

 
    public override void OnNetworkDespawn() // 네트워크 오브젝트가 디스폰 됐을 때 실행되는 함수 
    {
        if (IsOwner && action != null)
        {
            if (GameManager.Inst != null && GameManager.Inst.VirtualPad != null)
            {
                GameManager.Inst.VirtualPad.onMoveInput = null;
            }
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
        SetMoveInput(input);
    }

    public void SetMoveInput(float input)
    {
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
        SetRotateInput(rotateInput);
    }

    public void SetRotateInput(float rotateInput)
    {
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
    private void OnEffectChange(bool previousValue, bool newValue)
    {
        if (newValue)
        {
            bodyMat.SetFloat("_Emission_Value", 1.0f);
        }
        else
        {
            bodyMat.SetFloat("_Emission_Value", 0.0f);
        }
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

    [ServerRpc]
    void RequestSpawnServerRpc()
    {
        GameObject bullet_ = Instantiate(bullet, fire_Pos);
        bullet_.transform.position = fire_Pos.position;
        //bullet_.transform.rotation = fire_Pos.rotation;
        NetworkObject netObj = bullet_.GetComponent<NetworkObject>();
        netObj.Spawn();

    }


    [ServerRpc]
    void RequestSpawnEnergyOrbServerRpc()
    {
        GameObject orb = Instantiate(energy_Orb, fire_Pos);
        orb.transform.position = fire_Pos.position;
      //  orb.transform.rotation = fire_Pos.rotation;
        NetworkObject netObj = orb.GetComponent<NetworkObject>();
        netObj.Spawn();

    }

    private void OnPositionChange(Vector3 previousValue, Vector3 newValue) //네트워크 변수  position이 변경되었을 때 실행될 함수  previousValue = 변경되기 전 값
    {
        //controller.transform.position = newValue;
        transform.position = newValue;

    }
}
