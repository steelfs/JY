using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class NetPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();//�����ڷ� �б�, ���� ������ ������ �� �ִ�.

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

    private void Update()
    {
        transform.position = position.Value;
    }
}
