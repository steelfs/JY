
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
            go.name = $"{i} ��° ������Ʈ";
            go.AddComponent<TurnBaseObject>();
            tbo[i] = go.GetComponent<TurnBaseObject>();
            //tbo[i] = new TurnBaseObject(); //���۳�Ʈ��ӹ��� Ŭ������ new ������ؼ� ���������ϰ� ���Ƴ��� .
            //�����Ϸ��� ���ӿ�����Ʈ�� ����� AddComponent�� �̿��ؾ��Ѵ�.
            //Debug.Log($"��ü ���̳�? :{tbo[i]}");
            tbo[i].TurnActionValue = UnityEngine.Random.Range(-200, 200);

        }
        return tbo;
    }


    //================ array �� list ���� �⺻�������=====================//
    /// <summary>
    /// ���ı���
    /// �������� = true  �������� = false
    /// </summary>
    public bool isAsc = false;
    protected override void Test3(InputAction.CallbackContext context)
    {
        List<TurnBaseObject> tboList = new List<TurnBaseObject>(TestGeneratorArraData());
        tboList.Sort(SortComparison);//����Ʈ ����
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        TurnBaseObject[] tbo = TestGeneratorArraData();
        Array.Sort(tbo,SortComparison); // ��� ����

    }
    /// <summary>
    /// ����Ʈ�� Array �� �⺻���ı���� �̿��� ���� 
    /// ���ڰ��� �ڷ����� ��������Ѵ� ��ȯ���� int -1 0 1
    /// </summary>
    /// <param name="before">���ǰ�</param>
    /// <param name="after">���ǰ�</param>
    /// <returns>�� ���</returns>
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