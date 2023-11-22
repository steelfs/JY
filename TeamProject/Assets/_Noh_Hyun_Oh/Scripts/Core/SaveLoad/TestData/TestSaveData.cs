using System;
using System.Collections.Generic;
using UnityEngine;
/*
 JsonUtility.ToJson  ���߹迭 �����ȵ� . 
 
 �׽�Ʈ�� �ڵ� �����Ͽ� ���ؼ��� ���� �Ͻø�˴ϴ�.
 
 
 
 */
/// <summary>
/// �׽�Ʈ Ŭ���� �����
/// </summary>
[Serializable]
public struct dumyData
{
    public string[][] charcterName;//���߹迭 ����ȵ�
    public int age;
    public float height;
    public double weight;
    public short min;
    public long money;
    public ulong score;
    public string[] toStirngs;
    public ulong[] ulongs;
    public long[][] longs;//����ȵ�

    public dumyData(
        string[][] charcterName ,
        int age                 ,
        float height            ,
        double weight           , 
        short min               ,
        long money              ,
        ulong score             ,
        string[] toStirngs      ,
        ulong[] ulongs          ,
        long[][] longs          
        ) { 
        this.charcterName   = charcterName;
        this.toStirngs      = toStirngs;
        this.age            = age;
        this.height         = height;
        this.weight         = weight;
        this.min            = min;
        this.money          = money;
        this.score          = score;
        this.longs          = longs;
        this.ulongs         = ulongs;
    }

}
[Serializable] //�ʼ��� �ؾ� ����ȭ�۾������ļ� json�Ľ��̵ȴ�. Ŭ������ ����ü�� ����� �����Ӽ��� �����ָ�ȴ�.
public struct stringArray
{
    [SerializeField] //private ����� �̼Ӽ��������� json �Ľ��̵ȴ�
    string[] values ;
    public string[] Values { 
        get => values ;
        set => values = value;
    }
    public int[] tempInt; // �Ӽ��߰����ҽ� public �����ص��ȴ�
    public stringArray(string[] values, int[] tempInt) { 
        this.values = values;
        this.tempInt = tempInt;
    }
}
[Serializable]
public struct stringDoubleArray {
    [SerializeField]
    stringArray[] values;
    public stringArray[] Values { 
        get => values;
        set => values = value;  
    }
    public float[] yami;
    public stringDoubleArray(stringArray[] values, float[] yami) {
        this.values = values;
        this.yami = yami;
    }
}
[Serializable]
public struct stringTripleArray
{
    [SerializeField]
    stringDoubleArray[] values;
    
    public stringTripleArray(stringDoubleArray[] values) {
        this.values = values;
    }
}

/// <summary>
/// Ŭ������ ����ü �� ����ȭ �Ҷ� ��ȯ������ ����ؾ��Ѵ� 
/// ��ȯ������  A B C Ŭ�������ְ� 
///            AŬ��������  B�ɹ���  
///            BŬ��������  C�ɹ��� 
///            C Ŭ�������� A�ɹ��� �ִ°�� ���� 
///            Ŭ�����ȿ� �ٸ� Ŭ������ �ɹ��� ������� 
///            
/// ����ȭ�Ҷ� A Ŭ������ B Ŭ������ �İ��� 
///            BŬ���������� CŬ������ �İ��� 
///            CŬ���������� �ٽ� AŬ������ �İ��µ� 
///            ���⼭ �ٽ� AŬ������ BŬ������ �İ� ��鼭 
///            ������ ����ȭ �� ���ɼ��� �����Ѵ� 
/// �̸� �����ϱ����� ����Ƽ������ �İ��� ���̸� 10�ܰ������ ������ �ξ� ���ѷ����϶��� ��Ȳ����ó�Ͽ��� 
/// �Ľ̵����� ������ Vector ������ ������Ÿ�� �迭�� �����ؼ� ����غ��� �ٷ� ���ѷ��� ���ɼ��ִٰ� �˷��ִ� ��� �޼����� ���´�.
/// 
/// Ŭ����,����ü �� �迭�̳� ����Ʈ�� �����ͷ� ���鶧 ��ȯ������ ����ؾ��Ѵ�.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class TestSaveData<T> : JsonGameData  // ��ӹ��� �͵� ���� json���� �Ľ��̵ȴ�. ���׸� �� ���� �Ľ��� �ȴ�.
{
    //public MyClass otherInstance;//��ȯ ���� �׽�Ʈ�� Ŭ����
    /// <summary>0
    /// ������ �׽�Ʈ Vector������ ���޼��� �ȳ����°��� Ȯ��
    /// </summary>
    public void TestFunc() {

        //base.CharcterInfo = new StructList.CharcterInfo[100];
        //for (int i = 0; i < base.CharcterInfo.Length; i++)
        //{
        //    base.CharcterInfo[i].Level = 8 * i;
        //    base.CharcterInfo[i].CharcterName = $"{i} ��° ȫ�浿";
        //    base.CharcterInfo[i].EXP = i * 99;
        //    base.CharcterInfo[i].SceanPositionZ = 99.9f *i;
        //    base.CharcterInfo[i].SceanPositionY = 199.9f *i;
        //    base.CharcterInfo[i].SceanPositionX = 199.9f * i;
        //    base.CharcterInfo[i].Money = i * 5012;
        //    base.CharcterInfo[i].FlagList = new int[100];
        //    for (int ij = 0; ij < base.CharcterInfo[i].FlagList.Length; ij++)
        //    {
        //        base.CharcterInfo[i].FlagList[ij] = ij * 857;
        //    }
        //}
        base.QuestList = new StructList.CharcterQuest[500];
        for (int i = 0; i < base.QuestList.Length; i++)
        {
            //base.QuestList[i].QuestIProgress = i * 5;
            base.QuestList[i].QuestIndex = i ;
        }
      
        /*
         * �ش��ڵ� ����� ���� ������ �߻��Ͽ� �Ʒ������� �߻��Ͽ� ����ȭ�� �ȵȴ� 
         * Serialization depth limit 10 exceeded at 'MyClass.otherInstance'. There may be an object composition cycle in one or more of your serialized classes.
         * ����Ƽ������ �������̸� 10������ �����ϰ��ִ� �̴� ��ȯ ������ �����ϱ����ؼ��̴�.
        MyClass obj1 = new MyClass();
        MyClass obj2 = new MyClass();
        obj1.otherInstance = obj2;
        obj2.otherInstance = obj1;
        */
    }
    public JsonGameData SetSaveData() {
        TestSaveData<T> sd = new();

        sd.TestFunc();
        return sd;
    }

    public void SaveDataParsing(JsonGameData OriginData) {
        int a =  OriginData.DataIndex;
        EnumList.SceneName o = OriginData.SceanName;
        string time = OriginData.SaveTime;
       

    }

    /// <summary>
    /// ��ȯ ���� �׽�Ʈ�� Ŭ��������
    /// </summary>
    [Serializable]
    public class MyClass
    {
        public MyClass otherInstance;
    }

  
}
