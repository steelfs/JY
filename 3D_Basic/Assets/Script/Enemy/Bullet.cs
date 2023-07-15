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
        rigid.angularVelocity = Vector3.zero;// �ʱ�ȭ ���� �� ���ȯ���� �� ������ �� �������� (z��)������
        rigid.velocity = initialSpeed * transform.forward;
        StartCoroutine(LifeOver(10.0f));
    }
    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(2.0f));
    }
}
