using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestDelegate : MonoBehaviour
{
    //���θ���� ��������Ʈ Ÿ���� �̸��� TestDelegate
    // �� ��������Ʈ�� ������ �� �ִ� �Լ���  �Ķ��Ƽ�ʹ� �ϳ��� ���� ����Ÿ����void
    public delegate void Test_Delegate();
    Test_Delegate aaa;

    public delegate void TestDelegate2(int a, float b);
    TestDelegate2 bbb;

    //Action�� C#�� �������� ������ ���� void �Լ��� ������ �� �ִ� ��������Ʈ
    Action ccc; //�Ķ���Ͱ� ���� ����Ÿ�� void �� ��������Ʈ (C#�� �̸� �������� ��)
    Action<int> ddd; // �Ķ���Ͱ� int�ϳ��̰� ����Ÿ���� void


    // func : C#�� �������� ������ �ִ� �Լ��� ������ �� �ִ� ��������Ʈ(����Ÿ���� ������ ���׸�)
    Func<int> eee; //�Ķ���Ͱ� ���� ����Ÿ���� int�� ��������Ʈ
    Func<int, float> fff; //�Ķ���Ͱ� int �ϳ��̰� ����Ÿ���� float �� ��������Ʈ


    UnityEvent ggg; // ����Ƽ �������� �����ϴ� �Լ����� ���

    private void Start()
    {
        aaa += DelTest1;
        bbb += DelTest2;

        ggg.AddListener(DelTest1);

        aaa += () => DelTest2(10, 5.0f); //DelTest3�� ���� ������ �Լ��̴�.

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
