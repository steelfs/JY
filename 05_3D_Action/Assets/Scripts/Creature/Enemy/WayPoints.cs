using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    Transform[] children;//�� ��������Ʈ�� �迭
    int index = 0;//���� ã�ư� �ε���

    public Transform Current => children[index];
    private void Awake()
    {
        children = new Transform[transform.childCount];
        for (int  i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);    //�ڽ� ã�Ƽ� �迭�� �Ҵ�
        }
    }
    public Transform MoveNext()
    {
        index++;
        index %= children.Length;
        return children[index];
    }
}
