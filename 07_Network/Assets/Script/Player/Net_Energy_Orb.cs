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
           // StartCoroutine(Self_DeSpawn());
        }
    }

    IEnumerator Self_DeSpawn()
    {
        yield return new WaitForSeconds(lifeTime);
        if (IsServer)
        {
            this.NetworkObject.Despawn();
        }
    }
    private void OnCollisionEnter(Collision collision)//awake 타이밍 이후 언제든 발동 가능하다
    {
        if (!this.NetworkObject.IsSpawned)
            return;

        vfx.SetFloat("Size", 5);
        if (collision.gameObject.CompareTag("Player"))
        {
            NetPlayer player = collision.gameObject.GetComponent<NetPlayer>();
            player.Die();
        }
        EffectProcessClientRpc();

    }

    [ClientRpc]
    void EffectProcessClientRpc()//서버가 클라이언트에게 로컬 함수실행을 요청
    {
        rb.drag = float.MaxValue;
        rb.angularDrag = float.MaxValue;
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        float elapsedTime = 0.1f;
        while(elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            vfx.SetFloat("Size", elapsedTime * 10);
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
        if (IsServer)
        this.NetworkObject.Despawn();
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
