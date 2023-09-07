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
    NetworkVariable<FixedString512Bytes> chatString = new NetworkVariable<FixedString512Bytes>();//���� ���� ���� ä��
    public NetworkVariable<FixedString32Bytes> nameString = new NetworkVariable<FixedString32Bytes>();

    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();//�÷��̾��� ��ġ�� ������ ����, �����ڷ� �б�, ���� ������ ������ �� �ִ�.
    NetworkVariable<float> netMoveDir = new NetworkVariable<float>(); //�Է¹��� ���� / ���� ����
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
    //netEffectState true �϶� �ȸ�� ������
    // ������ ����� �ٸ� �÷��̾�Ե� ��������
    //���ӽ� �̸��ǿ� �ڽ��� �̸� ����
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
    //�� �� ���� awake - enable - OnNetworkSpawn - start
    //Isowner  true ���Ǵ� ���� -> OnNetworkSpawn
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        position.OnValueChanged += OnPositionChange;//position �� value�� �ٲ� ����Ǵ� ��������Ʈ
        chatString.OnValueChanged += OnChatRecieve;
        anim = GetComponent<Animator>();

        netAnimState.OnValueChanged += OnAnimStateChange;

        Renderer renderer = transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>();
        bodyMat = renderer.material;
        netEffectState.OnValueChanged += OnEffectChange;
        fire_Pos = transform.GetChild(4).transform;
    }


    public override void OnNetworkSpawn()//�� �Ӹ� �ƴ϶� �ٸ� ������Ʈ�� �������� ���� ������ �Ǵ� �Լ� �̱⶧���� Owner���� üũ�� ���� ������ �ٸ� ������Ʈ�� ������� ���� ������ �ȴ�.
    {
        if (IsOwner)
        {
            action = new();//������ ����
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

 
    public override void OnNetworkDespawn() // ��Ʈ��ũ ������Ʈ�� ���� ���� �� ����Ǵ� �Լ� 
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

        if (NetworkManager.Singleton.IsServer)// �����̸� ��Ʈ��ũ���� ���� �ٷ� ����
        {
            netMoveDir.Value = moveDir;
        }
        else
        {
            Move_RequestServerRpc(moveDir);//Ŭ���̾�Ʈ�� RPC�� ���� ������û
        }

        if (moveDir > 0.001f)// ���� 0.001 = float ������ ������ �Ӱ谪
        {
            state = PlayerAnimState.Walk;
            //rotateAngle = Quaternion.LookRotation(dir);
            //anim.SetTrigger(walkHash);
        }
        else if (moveDir < -0.001f)//����
        {
            state = PlayerAnimState.BackWalk;
            //anim.SetBool(isWalkHash, false);
        }
        else//����
        {
            state = PlayerAnimState.Idle;
        }

        //���°� ����Ǹ� ��Ʈ��ũ���µ� ���� ����
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

    public void SendChat(string message)//ä���� �����ϴ� �Լ� 
    {
        if (IsServer)
        {
            chatString.Value = message; // ���� ������ ���� ����
        }
        else
        {
            RequestChat_ServerRpc(message);
        }
    }
    private void OnAnimStateChange(PlayerAnimState previousValue, PlayerAnimState newValue)//netAnimState�� ����Ǿ��� ��
    {
        anim.SetTrigger(newValue.ToString());
    }

    [ServerRpc]
    public void RequestChat_ServerRpc(string text)
    {
        chatString.Value = text;
    }
    [ServerRpc]// RPC = Remote procedure Call �����Լ� ȣ��
    void submitPos_RequestServerRpc(Vector3 newPos)// �Լ� �̸��� ���� �ݵ�� ServerRpc �̾�� �Ѵ� �ƴϸ� �����߻�
    {
        position.Value = newPos;
    }

    [ServerRpc]
    void Move_RequestServerRpc(float move)// �Լ� �̸��� ���� �ݵ�� ServerRpc �̾�� �Ѵ� �ƴϸ� �����߻�
    {
        netMoveDir.Value = move;
    }
    [ServerRpc]
    void Rotate_RequestServerRpc(float rotate)// �Լ� �̸��� ���� �ݵ�� ServerRpc �̾�� �Ѵ� �ƴϸ� �����߻�
    {
        netRotateDir.Value = rotate;
    }

    [ServerRpc]
    void UpdateAnimStateServerRpc(PlayerAnimState newState)// �Լ� �̸��� ���� �ݵ�� ServerRpc �̾�� �Ѵ� �ƴϸ� �����߻�
    {
        netAnimState.Value = newState;
    }

    [ServerRpc]
    void UpdateNameStateServerRpc(string newName)// �Լ� �̸��� ���� �ݵ�� ServerRpc �̾�� �Ѵ� �ƴϸ� �����߻�
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

    private void OnPositionChange(Vector3 previousValue, Vector3 newValue) //��Ʈ��ũ ����  position�� ����Ǿ��� �� ����� �Լ�  previousValue = ����Ǳ� �� ��
    {
        //controller.transform.position = newValue;
        transform.position = newValue;

    }
}
