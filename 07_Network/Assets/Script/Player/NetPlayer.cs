using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class NetPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();//생성자로 읽기, 쓰기 권한을 조정할 수 있다.

    public override void OnNetworkSpawn()
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

    [ServerRpc]// RPC = Remote procedure Call 원격함수 호출
    void submitPos_RequestServerRpc(ServerRpcParams rpcParams = default)// 함수 이름의 끝은 반드시 ServerRpc 이어야 한다 아니면 오류발생
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
