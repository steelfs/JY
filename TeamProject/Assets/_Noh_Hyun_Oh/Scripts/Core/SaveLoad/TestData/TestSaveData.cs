using System;
using System.Collections.Generic;
using UnityEngine;
/*
 JsonUtility.ToJson  다중배열 지원안됨 . 
 
 테스트용 코드 이파일에 대해서는 참고만 하시면됩니다.
 
 
 
 */
/// <summary>
/// 테스트 클래스 참고용
/// </summary>
[Serializable]
public struct dumyData
{
    public string[][] charcterName;//다중배열 저장안됨
    public int age;
    public float height;
    public double weight;
    public short min;
    public long money;
    public ulong score;
    public string[] toStirngs;
    public ulong[] ulongs;
    public long[][] longs;//저장안됨

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
[Serializable] //필수로 해야 직렬화작업을거쳐서 json파싱이된다. 클래스나 구조체의 선언부 위에속성을 붙혀주면된다.
public struct stringArray
{
    [SerializeField] //private 선언시 이속성이있으면 json 파싱이된다
    string[] values ;
    public string[] Values { 
        get => values ;
        set => values = value;
    }
    public int[] tempInt; // 속성추가안할시 public 으로해도된다
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
/// 클래스나 구조체 등 직렬화 할때 순환참조를 고려해야한다 
/// 순환참조란  A B C 클래스가있고 
///            A클래스에는  B맴버가  
///            B클래스에는  C맴버가 
///            C 클래스에는 A맴버가 있는경우 같이 
///            클래스안에 다른 클래스를 맴버로 가질경우 
///            
/// 직렬화할때 A 클래스의 B 클래스로 파고들고 
///            B클래스에서는 C클래스로 파고들고 
///            C클래스에서는 다시 A클래스로 파고드는데 
///            여기서 다시 A클래스는 B클래스를 파고 들면서 
///            무한히 직렬화 할 가능성이 존재한다 
/// 이를 방지하기위해 유니티에서는 파고드는 깊이를 10단계까지만 제한을 두어 무한루프일때의 상황을대처하였다 
/// 파싱데이터 구조에 Vector 종류의 데이터타입 배열을 선언해서 사용해보면 바로 무한루프 가능성있다고 알려주는 경고 메세지가 나온다.
/// 
/// 클래스,구조체 의 배열이나 리스트는 데이터로 만들때 순환참조를 고려해야한다.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class TestSaveData<T> : JsonGameData  // 상속받은 것도 같이 json으로 파싱이된다. 제네릭 도 같이 파싱이 된다.
{
    //public MyClass otherInstance;//순환 참조 테스트용 클래스
    /// <summary>0
    /// 데이터 테스트 Vector를빼고 경고메세지 안나오는것을 확인
    /// </summary>
    public void TestFunc() {

        //base.CharcterInfo = new StructList.CharcterInfo[100];
        //for (int i = 0; i < base.CharcterInfo.Length; i++)
        //{
        //    base.CharcterInfo[i].Level = 8 * i;
        //    base.CharcterInfo[i].CharcterName = $"{i} 번째 홍길동";
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
         * 해당코드 실행시 순한 참조가 발생하여 아래에러를 발생하여 직렬화가 안된다 
         * Serialization depth limit 10 exceeded at 'MyClass.otherInstance'. There may be an object composition cycle in one or more of your serialized classes.
         * 유니티에서는 참조깊이를 10번으로 제한하고있다 이는 순환 참조를 방지하기위해서이다.
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
    /// 순환 참조 테스트용 클래스선언
    /// </summary>
    [Serializable]
    public class MyClass
    {
        public MyClass otherInstance;
    }

  
}
