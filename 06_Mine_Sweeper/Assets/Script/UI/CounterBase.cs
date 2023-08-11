using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ImageNumber))]//�� ��ũ��Ʈ�� ����ϱ� ���� �ʼ������� �ʿ��� ������Ʈ�� ������ �ڵ����� �߰���
public class CounterBase : MonoBehaviour
{
    ImageNumber imageNumber;
    protected virtual void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    
    protected virtual void Refresh(int count)//��������Ʈ�� ������ �Լ� 
    {
        imageNumber.Number = count;
    }
}
