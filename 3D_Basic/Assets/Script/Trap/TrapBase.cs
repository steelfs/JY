using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    SphereCollider sphereCollider;
    private void OnTriggerEnter(Collider other) //���𰡰� Ʈ���ſ� ������ �ߵ�
    {
        if (other.CompareTag("Player"))
        {
            OnTrapActivate(other.gameObject);
        }
        
    }
    protected virtual void OnTrapActivate(GameObject target) //�浿�� ������Ʈ�� �Ķ���ͷ�  �浹�� ������ �Լ� ȣ��
    {

    }
}
