using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PooledObject
{
    public float initialSpeed = 20.0f;
    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
    }
    private void OnEnable()
    {
        rigid.angularVelocity = Vector3.zero;// 초기화 안할 시 재소환됐을 때 떨어진 후 앞쪽으로 (z축)구른다
        rigid.velocity = initialSpeed * transform.forward;
        StartCoroutine(LifeOver(10.0f));
    }
    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(2.0f));
    }
}
