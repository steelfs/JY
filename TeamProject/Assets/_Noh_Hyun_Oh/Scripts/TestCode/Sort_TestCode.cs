
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class Sort_TestCode : TestBase
{


    protected override void Awake()
    {
        base.Awake();
    }

    LinkedList<ITurnBaseData> turnObjectList;

    protected override void Test1(InputAction.CallbackContext context)
    {
        //TurnManager.Instance.InitTurnData();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {

    }

    private TurnBaseObject[] TestGeneratorArraData()
    {
        TurnBaseObject[] tbo = new TurnBaseObject[10];
        for (int i = 0; i < tbo.Length; i++)
        {
            GameObject go = new GameObject();
            go.name = $"{i} 번째 오브젝트";
            go.AddComponent<TurnBaseObject>();
            tbo[i] = go.GetComponent<TurnBaseObject>();
            //tbo[i] = new TurnBaseObject(); //컴퍼넌트상속받은 클래스는 new 를사용해서 생성을못하게 막아놨다 .
            //생성하려면 게임오브젝트를 만들고 AddComponent를 이용해야한다.
            //Debug.Log($"객체 널이냐? :{tbo[i]}");
            tbo[i].TurnActionValue = UnityEngine.Random.Range(-200, 200);

        }
        return tbo;
    }


    //================ array 와 list 정렬 기본기능제공=====================//
    /// <summary>
    /// 정렬기준
    /// 오름차순 = true  내림차순 = false
    /// </summary>
    public bool isAsc = false;
    protected override void Test3(InputAction.CallbackContext context)
    {
        List<TurnBaseObject> tboList = new List<TurnBaseObject>(TestGeneratorArraData());
        tboList.Sort(SortComparison);//리스트 정렬
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        TurnBaseObject[] tbo = TestGeneratorArraData();
        Array.Sort(tbo,SortComparison); // 어레이 정렬

    }
    /// <summary>
    /// 리스트와 Array 의 기본정렬기능을 이용한 정렬 
    /// 인자값의 자료형은 맞춰줘야한다 반환값은 int -1 0 1
    /// </summary>
    /// <param name="before">앞의값</param>
    /// <param name="after">뒤의값</param>
    /// <returns>비교 결과</returns>
    private int SortComparison(TurnBaseObject before , TurnBaseObject after) {
        if (before.TurnActionValue < after.TurnActionValue)
        {
            return isAsc ? -1 : 1; 
        }
        else if (before.TurnActionValue > after.TurnActionValue)
        {
            return isAsc ?  1 : -1;
        }
        else 
        {
            return 0;
        }
    }
}