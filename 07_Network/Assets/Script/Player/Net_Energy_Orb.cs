using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Net_Energy_Orb : NetworkBehaviour
{
    public float speed = 5.0f;
    public float lifeTime = 5.0f;

    public float maxSize = 5.0f;

    float expandSpeed = 20.0f;
    Rigidbody rb;
    VisualEffect vfx;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        vfx = GetComponentInChildren<VisualEffect>();
       
    }

    public override void OnNetworkSpawn()
    {
        transform.Rotate(-30.0f,0,0);
        rb.velocity = speed * transform.forward;
        if (IsServer)
        {
            StartCoroutine(Self_DeSpawn());
        }
    }

    IEnumerator Self_DeSpawn()
    {
        yield return new WaitForSeconds(lifeTime);

        if (IsOwner)
        {
            Request_Despawn_ServerRpc();
        }
    }
    private void OnCollisionEnter(Collision collision)//awake 타이밍 이후 언제든 발동 가능하다
    {
        if (!this.NetworkObject.IsSpawned)
            return;

        //범위안에 들어온 적 모두 Die 실행

        Collider[] result = (Physics.OverlapSphere(transform.position, maxSize, LayerMask.GetMask("Player")));
        
        if (result.Length > 0)
        {
            List<ulong> targets = new List<ulong>(result.Length);
            foreach (Collider coll in result)
            {
                NetPlayer netPlayer = coll.gameObject.GetComponent<NetPlayer>();
                targets.Add(netPlayer.OwnerClientId);

                ClientRpcParams rpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = targets.ToArray() //어떤 클라이언트에게 rpc를 보낼 것인가?// 이 배열에 지정된 클라이언트만 rpc를 받는다.

                    }
                };
                Request_PlayerDie_ClientRpc();
            }

        }
        EffectProcessClientRpc();



        //if (collision.gameObject.CompareTag("Player"))
        //{

        //    NetPlayer[] netPlayers = collision.gameObject.GetComponents<NetPlayer>();
        //    foreach (var netPlayer in netPlayers)
        //    {
        //        ClientRpcParams rpcParams = new ClientRpcParams
        //        {
        //            Send = new ClientRpcSendParams
        //            {
        //                TargetClientIds = new ulong[] { netPlayer.OwnerClientId }//어떤 클라이언트에게 rpc를 보낼 것인가?// 이 배열에 지정된 클라이언트만 rpc를 받는다.
        //            }
        //        };
        //        Request_PlayerDie_ClientRpc(rpcParams);
        //    }
        //}
    }

    [ClientRpc]
    void Request_PlayerDie_ClientRpc(ClientRpcParams clientRpcParams = default)
    {
        GameManager.Inst.Player.Die();
    }

    [ClientRpc]
    void EffectProcessClientRpc()//서버가 클라이언트에게 로컬 함수실행을 요청
    {
        rb.drag = float.MaxValue;
        rb.angularDrag = float.MaxValue;
        StartCoroutine(Effect());
    }

    [ServerRpc]
    void Request_Despawn_ServerRpc()
    {
        this.NetworkObject.Despawn();
    }

    IEnumerator Effect()
    {
        float elapsedTime = 0.1f;
        while(elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            vfx.SetFloat("Size", elapsedTime * 5);
            yield return null;
        }
        elapsedTime = 1.0f;
        while(elapsedTime > 0.0f)
        {
            elapsedTime -= Time.deltaTime;
            vfx.SetFloat("Size",elapsedTime * 5);
            yield return null;
        }
        vfx.SendEvent("EffectFinish");//파티클 생성중지 이벤트 실행
        while(vfx.aliveParticleCount > 0)//살아있는 파티클이 없으면 Despawn
        {
            yield return null;
        }
        if (IsOwner)
        {
            Request_Despawn_ServerRpc();
        }
    }

    //IEnumerator Explosion()
    //{
    //    float time = 0.1f;
    //    while (time < 2.1f)
    //    {
    //        time += expandSpeed * Time.deltaTime;
    //        vfx.SetFloat("Size", time);
    //        yield return null;
    //    }
    //    while (time > 0.0001f)
    //    {
    //        time -= expandSpeed * Time.deltaTime;
    //        vfx.SetFloat("Size", time);
    //        yield return null;
    //    }
    //    if (IsServer)
    //        this.NetworkObject.Despawn();
    //}
}
