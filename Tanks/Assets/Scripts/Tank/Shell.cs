using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shell : MonoBehaviour
{
    //�����Ǹ� ��� ������ ������.
    //��ź�� �ƴ� �ٸ��Ͱ� �΋H���� ����
    // �ֺ��� ���߷��� ����
    // ������ ����Ʈ
    // ������ ȿ�� VFX�� �����
    // ��ź, ��������Ʈ�� ���丮, ������ƮǮ�� ����
    //

    Rigidbody rb;
    public float force = 50.0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        Shoot();
    }
    void Shoot()
    {
        Vector3 force_ = transform.forward * force;
        force_.y = 5;
        rb.AddForce(force_, ForceMode.Impulse);
    }
}
