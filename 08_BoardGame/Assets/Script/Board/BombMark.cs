using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMark : MonoBehaviour
{
    public GameObject successPrefab;
    public GameObject failurePrefab;

    List<GameObject> bombMarkPrefab;
    private void Start()
    {
        bombMarkPrefab = new List<GameObject>();
    }
    public void SetBombMark(Vector3 world, bool isSuccess)// world = ���ݹ��� �׸��� ��� ��ġ 
    {
        if (isSuccess)
        {
            GameObject obj = Instantiate(successPrefab);
            obj.transform.position = world;
            bombMarkPrefab.Add(obj);
        }
        else
        {
            GameObject obj = Instantiate(failurePrefab);
            obj.transform.position = world;
            bombMarkPrefab.Add(obj);
        }
        //isSuccess  ���� ������ ����
    }

    //��� ��źǥ�ø� �����ϴ� �Լ�
    public void ResetBombMArk()
    {
        foreach (var prefab in bombMarkPrefab)
        {
            Destroy(prefab);
        }
    }
}
