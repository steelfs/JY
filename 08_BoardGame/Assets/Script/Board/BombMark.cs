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
    public void SetBombMark(Vector3 world, bool isSuccess)// world = 공격받은 그리드 가운데 위치 
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
        //isSuccess  따라 프리팹 생성
    }

    //모든 폭탄표시를 제거하는 함수
    public void ResetBombMArk()
    {
        foreach (var prefab in bombMarkPrefab)
        {
            Destroy(prefab);
        }
    }
}
