using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Net_Bullet : NetworkBehaviour
{
    public float speed = 10.0f;
    Rigidbody rb;
    public float lifeTime = 5.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
   
}
