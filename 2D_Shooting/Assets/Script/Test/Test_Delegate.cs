using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestDelegate : MonoBehaviour
{
    //새로만드는 델리게이트 타입의 이름은 TestDelegate
    // 이 델리게이트에 저장할 수 있는 함수는  파라미티터는 하나도 없고 리턴타입은void
    public delegate void Test_Delegate();
    Test_Delegate aaa;

    public delegate void TestDelegate2(int a, float b);
    TestDelegate2 bbb;

    //Action은 C#이 만들어놓은 리턴이 없는 void 함수를 저장할 수 있는 델리게이트
    Action ccc; //파라미터가 없고 리턴타입 void 인 델리게이트 (C#이 미리 만들어놓은 것)
    Action<int> ddd; // 파라미터가 int하나이고 리턴타입이 void


    // func : C#이 만들어놓은 리턴이 있는 함수를 저장할 수 있는 델리게이트(리턴타입은 마지막 제네릭)
    Func<int> eee; //파라미터가 없고 리턴타입이 int인 델리게이트
    Func<int, float> fff; //파라미터가 int 하나이고 리턴타입이 float 인 델리게이트


    UnityEvent ggg; // 유니티 엔진에서 제공하는 함수저장 기능

    private void Start()
    {
        aaa += DelTest1;
        bbb += DelTest2;

        ggg.AddListener(DelTest1);

        aaa += () => DelTest2(10, 5.0f); //DelTest3과 같은 형식의 함수이다.

        //Test_Delegate ccc = () => Test();
    }
    void DelTest1()
    {
        Debug.Log("Test1");
    }
    void DelTest2(int i, float f)
    {
        Debug.Log($"Test2 {i}, {f}");
    }
    void DelTest3()
    {
        DelTest2(10, 5.0f);
    }
}
