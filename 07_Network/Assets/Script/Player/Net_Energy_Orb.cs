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
    private void OnCollisionEnter(Collision collision)//awake Ÿ�̹� ���� ������ �ߵ� �����ϴ�
    {
        if (!this.NetworkObject.IsSpawned)
            return;

        //�����ȿ� ���� �� ��� Die ����

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
                        TargetClientIds = targets.ToArray() //� Ŭ���̾�Ʈ���� rpc�� ���� ���ΰ�?// �� �迭�� ������ Ŭ���̾�Ʈ�� rpc�� �޴´�.

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
        //                TargetClientIds = new ulong[] { netPlayer.OwnerClientId }//� Ŭ���̾�Ʈ���� rpc�� ���� ���ΰ�?// �� �迭�� ������ Ŭ���̾�Ʈ�� rpc�� �޴´�.
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
    void EffectProcessClientRpc()//������ Ŭ���̾�Ʈ���� ���� �Լ������� ��û
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
        vfx.SendEvent("EffectFinish");//��ƼŬ �������� �̺�Ʈ ����
        while(vfx.aliveParticleCount > 0)//����ִ� ��ƼŬ�� ������ Despawn
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
