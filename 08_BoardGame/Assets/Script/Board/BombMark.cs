using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        GameObject prefab = isSuccess ? successPrefab : failurePrefab;
        GameObject instance = Instantiate(prefab, transform);
        world.y = transform.position.y;
        instance.transform.position = world;

        //if (isSuccess)
        //{
        //    GameObject obj = Instantiate(successPrefab);
        //    obj.transform.position = world;
        //    bombMarkPrefab.Add(obj);
        //}
        //else
        //{
        //    GameObject obj = Instantiate(failurePrefab);
        //    obj.transform.position = world;
        //    bombMarkPrefab.Add(obj);
        //}
        //isSuccess  ���� ������ ����
    }

    //��� ��źǥ�ø� �����ϴ� �Լ�
    public void ResetBombMArk()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }
}
