using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class Net_Bullet : NetworkBehaviour
{
    public float speed = 10.0f;
    Rigidbody rb;
    public float lifeTime = 5.0f;
    public int crash = 0;

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
        else
        {
            Vector3 incoming = rb.velocity.normalized;
            Vector3 normal = collision.GetContact(0).normal;

            Vector3 reflect = Vector3.Reflect(incoming, normal);

            rb.velocity = reflect * speed;
            crash++;
        }
        if (crash > 2)
        {
            if (IsServer)
                this.NetworkObject.Despawn();
        }
        //this.NetworkObject.Despawn();
    }
}
 