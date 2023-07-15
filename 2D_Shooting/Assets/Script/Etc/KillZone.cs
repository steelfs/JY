using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PooledObject obj = collision.GetComponent<PooledObject>();
        if (obj != null)
        {
            collision.gameObject.SetActive(false); // PooledObject �� ��ӹ޾� Ǯ�� ���ư����ϴ� ������Ʈ��� ��Ȱ��ȭ
        }
        else
        {
            Destroy(collision.gameObject); //�ƴҰ�� �ı�
        }
    
    }

}
