using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shell : PooledObject
{
    //�����Ǹ� ��� ������ ������.
    //��ź�� �ƴ� �ٸ��Ͱ� �΋H���� ����
    // �ֺ��� ���߷��� ����
    // ������ ����Ʈ
    // ������ ȿ�� VFX�� �����
    // ��ź, ��������Ʈ�� ���丮, ������ƮǮ�� ����
    //
    bool isExplode = false;
    Rigidbody rb;
    public GameObject explosionPrefab;
    public float movePower = 20.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        rb.velocity = transform.forward * movePower;
        rb.angularVelocity = Vector3.zero;
        //Shoot();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isExplode)
        {
            Time.timeScale = 0.1f;
            isExplode = true;
            Vector3 pos = collision.contacts[0].point;
            Vector3 normal = collision.contacts[0].normal;
            Instantiate(explosionPrefab, pos, Quaternion.LookRotation(normal));
        }
    }
}
