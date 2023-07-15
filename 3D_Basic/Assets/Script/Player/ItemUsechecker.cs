using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUsechecker : MonoBehaviour
{
    public Action<Iinteractable> onItemUse;
    private void OnTriggerEnter(Collider other)
    {
        //üĿ�� Ʈ���� ������ �ٸ� �ݶ��̴��� ������ ��
        Transform target = other.transform;
        Iinteractable obj = null;
        do
        {
            obj = target.GetComponent<Iinteractable>();//�浹�� ��ü�� Iinteractable ���ִٸ� ã�ƿ���
            target = target.parent;                     // target�� �θ�� ����
        }
        while (obj == null && target != null);// obj�� ã�ų� ���̻� �θ� ������ ����
    

        if (obj != null)
        {
            onItemUse?.Invoke(obj);//Iinteractable �� ��ӹ��� ������Ʈ�� ������ ����
        }
    }
}
