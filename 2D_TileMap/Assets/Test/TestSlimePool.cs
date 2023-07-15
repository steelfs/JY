using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSlimePool : TestBase
{
    List<GameObject> slimeList = new List<GameObject>(64);
    Slime slimeInstance;

    protected override void Test1(InputAction.CallbackContext context)
    {
        GameObject slime = Factory.Inst.GetObject(Pool_Object_Type.Slime, new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-4.0f, 4.0f))); 
        slimeList.Add(slime);

    }

  
    protected override void Test2(InputAction.CallbackContext context)
    {
        while (slimeList.Count > 0)
        {
            GameObject obj = slimeList[0];
            slimeList.RemoveAt(0);

            Slime slime = obj.GetComponent<Slime>();
            slime.Die();

    
        }
    }
}
//실습
// 슬라임이 풀에서 나올 때 셰이더 프로퍼티 초기화 후 페이즈 실행
// 슬라임의 Die함수 구현하기 