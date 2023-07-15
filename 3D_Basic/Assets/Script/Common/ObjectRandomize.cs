using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomize : MonoBehaviour
{
    public bool check = true;
    private void OnValidate()//인스펙터창에서 데이터가 성공적으로 변경되면 실행되는 함수
    {
       if (check)
        {
            transform.Rotate(0, Random.Range(0, 360.0f), 0); //
            transform.localScale = new Vector3(1 + Random.Range(-0.15f, 0.15f), 1 + Random.Range(-0.15f, 0.15f), 1 + Random.Range(-0.15f, 0.15f));
            check= false;//클릭 하면 스케일 변경
        }
    }
}
