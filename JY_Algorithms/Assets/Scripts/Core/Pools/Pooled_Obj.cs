using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooled_Obj : MonoBehaviour
{
   //public Action<Pooled_Obj> on_ReturnPool;

    public int poolIndex { get; set; }

    public void ReturnToPool()
    {
        GameManager.Pools.ReturnPool(this);
        gameObject.SetActive(false);
    }
    //Disable���� ��������Ʈ ��ȣ�� ������ ���� �Ҿ����� �ڵ尰�� ���δ�.
    //���� �Լ��� ���� ReturnPool�Լ��� �����Ű�� �״��� ���� Disable�� �����ִ°� �´°� ����.
}
