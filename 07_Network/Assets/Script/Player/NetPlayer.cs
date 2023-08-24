using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class NetPlayer : NetworkBehaviour
{
    PlayerInputAction action;
    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();//�����ڷ� �б�, ���� ������ ������ �� �ִ�.
    //NetworkVariable rotate �����
    Vector3 fixedPos;

    float moveDir;
    float moveSpeed = 3.0f;
    float rotateDir;
    float rotateSpeed = 180.0f;

    private void Awake()
    {
        action = new();
    }
    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.MoveForward.performed += OnMoveForward;
        action.Player.MoveForward.canceled += OnMoveForward;
        action.Player.Rotate.performed += OnRotate;
    }

    private void OnMoveForward(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            moveDir = moveSpeed * context.ReadValue<float>();
        }
        //fixedPos = transform.position + transform.forward * moveDir;
        //position.Value = Time.deltaTime * moveSpeed * fixedPos;
    }
    private void OnRotate(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        rotateDir = rotateSpeed * context.ReadValue<float>();
    }
    private void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            position.Value = Time.deltaTime * transform.forward * moveDir;
        }
        else
        {
            if (IsOwner)
            {
                MovePos_RequestServerRpc();
            }
        }
        transform.position += position.Value;
    }
    

    public override void OnNetworkSpawn()//�� �Ӹ� �ƴ϶� �ٸ� ������Ʈ�� �������� ���� ������ �Ǵ� �Լ� �̱⶧���� Owner���� üũ�� ���� ������ �ٸ� ������Ʈ�� ������� ���� ������ �ȴ�.
    {
     
        if (IsOwner)
        {
            Move();
        }
    }
    private void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Vector3 newPos = UnityEngine.Random.insideUnitCircle * 0.5f;
            newPos.y = 0;
            position.Value = newPos;
            transform.position = newPos;
        }
        else
        {
            submitPos_RequestServerRpc();
        }
    }

    [ServerRpc]// RPC = Remote procedure Call �����Լ� ȣ��
    void submitPos_RequestServerRpc(ServerRpcParams rpcParams = default)// �Լ� �̸��� ���� �ݵ�� ServerRpc �̾�� �Ѵ� �ƴϸ� �����߻�
    {
        Vector3 newPos = UnityEngine.Random.insideUnitCircle * 0.5f;
        newPos.y = 0;
        position.Value = newPos;
    }

    [ServerRpc]
    void MovePos_RequestServerRpc(ServerRpcParams rpcParams = default)
    {
       position.Value = Time.deltaTime * transform.forward * moveDir;
    }
}
