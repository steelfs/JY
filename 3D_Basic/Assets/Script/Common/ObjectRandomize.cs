using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomize : MonoBehaviour
{
    public bool check = true;
    private void OnValidate()//�ν�����â���� �����Ͱ� ���������� ����Ǹ� ����Ǵ� �Լ�
    {
       if (check)
        {
            transform.Rotate(0, Random.Range(0, 360.0f), 0); //
            transform.localScale = new Vector3(1 + Random.Range(-0.15f, 0.15f), 1 + Random.Range(-0.15f, 0.15f), 1 + Random.Range(-0.15f, 0.15f));
            check= false;//Ŭ�� �ϸ� ������ ����
        }
    }
}
