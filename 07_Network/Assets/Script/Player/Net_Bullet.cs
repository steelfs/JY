using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class Net_Bullet : NetworkBehaviour
{
    public float speed = 10.0f;
    Rigidbody rb;
    public float lifeTime = 10.0f;
    public int reflectCount = 2;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //transform.Translate(speed * Time.deltaTime * transform.forward);
    }
    public override void OnNetworkSpawn()
    {
        rb.velocity = speed * transform.forward;
        if (IsServer)
        {
            StartCoroutine(Self_DeSpawn());
        }
    }

    IEnumerator Self_DeSpawn()
    {
        yield return new WaitForSeconds(lifeTime);
        if (IsOwner)//오너일 때 리퀘스트 보냄 // 오너가 아니면 리퀘스트 불가능
        {
            Request_Despawn_ServerRpc();
        }
    }
    private void OnCollisionEnter(Collision collision)//awake 타이밍 이후 언제든 발동 가능하다
    {
        if (!this.NetworkObject.IsSpawned)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
           

            NetPlayer player = collision.gameObject.GetComponent<NetPlayer>();

            //this.NetworkObjectId; //오브젝트의 ID
            //this.OwnerClientId;   // 이 오브젝트를 가지고 있는 클라이언트의 ID 

            ClientRpcParams rpcParams = new ClientRpcParams 
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] {player.OwnerClientId }//어떤 클라이언트에게 rpc를 보낼 것인가?// 이 배열에 지정된 클라이언트만 rpc를 받는다.
                }
            };
            PlayerDie_ClientRpc(rpcParams);// rpcParams가 없으면 모두에게 보낸다

            if (IsOwner)
            {
                Request_Despawn_ServerRpc();
            }

        }
        else if (reflectCount > 0)
        {
            transform.forward = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
            rb.angularVelocity = Vector3.zero;
            rb.velocity = speed * transform.forward;
            reflectCount--;
        }
        else
        {
            if (IsServer)
                this.NetworkObject.Despawn();//이미 2번이상 튕겼으면
        }
    }

    [ClientRpc]
    void PlayerDie_ClientRpc(ClientRpcParams clientRpcParams = default)
    {
        GameManager.Inst.Player.Die();
    }

    [ServerRpc]// (RequireOwnership = false) 추가시 오너가 아니더라도 RPC 요청 가능
    void Request_Despawn_ServerRpc()
    {
        this.NetworkObject.Despawn();
    }
}
 