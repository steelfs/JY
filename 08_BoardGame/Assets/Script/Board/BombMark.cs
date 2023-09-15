using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMark : MonoBehaviour
{
    public GameObject successPrefab;
    public GameObject failurePrefab;

    public void SetBombMark(Vector3 world, bool isSuccess)// world = 공격받은 그리드 가운데 위치 
    {
        //isSuccess  따라 프리팹 생성
    }

    //모든 폭탄표시를 제거하는 함수
    public void ResetBombMArk()
    {

    }
}
