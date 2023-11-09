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
    public float explosionRadius = 2.0f;
    public float explosionForce = 10.0f;

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
           // Time.timeScale = 0;
            isExplode = true;
            Vector3 pos = collision.contacts[0].point;
            Vector3 normal = collision.contacts[0].normal;
            GameObject obj =  Instantiate(explosionPrefab, pos, Quaternion.LookRotation(normal));
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            ps.Play();

            Collider[] colls = Physics.OverlapSphere(pos, explosionRadius, LayerMask.GetMask("ExplosionTarget", "Players"));
            if (colls.Length > 0)
            {
                foreach(Collider coll in colls)
                {
                    Rigidbody targetRigid = coll.GetComponent<Rigidbody>();
                    if (targetRigid != null)
                    {
                        targetRigid.AddExplosionForce(explosionForce, pos, explosionRadius);
                    }
                }
            }
        }
    }
}
