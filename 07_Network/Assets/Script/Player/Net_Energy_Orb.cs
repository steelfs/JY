using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            StartCoroutine(Self_DeSpawn());
        }
    }

    IEnumerator Self_DeSpawn()
    {
        yield return new WaitForSeconds(lifeTime);
        this.NetworkObject.Despawn();
    }
    private void OnCollisionEnter(Collision collision)//awake 타이밍 이후 언제든 발동 가능하다
    {
        if (!this.NetworkObject.IsSpawned)
            return;

        StartCoroutine(Explosion());
    }
    IEnumerator Explosion()
    {
        float time = 0.1f;
        while (time < 2.1f)
        {
            time += expandSpeed * Time.deltaTime;
            vfx.SetFloat("Size", time);
            yield return null;
        }
        while (time > 0.0001f)
        {
            time -= expandSpeed * Time.deltaTime;
            vfx.SetFloat("Size", time);
            yield return null;
        }
        if (IsServer)
            this.NetworkObject.Despawn();
    }
}
