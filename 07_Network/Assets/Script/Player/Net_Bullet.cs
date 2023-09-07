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
        this.NetworkObject.Despawn();
    }
    private void OnCollisionEnter(Collision collision)//awake 타이밍 이후 언제든 발동 가능하다
    {
        if (!this.NetworkObject.IsSpawned)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsServer)
            this.NetworkObject.Despawn();
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
}
 