using StructList;
using System;
using System.IO;
using UnityEngine;



/// <summary>
/// 게임의 저장할 데이터를 정의 하는 클래스로 지정한다 .
/// 작성방법 
/// 닥
/// 1. 변수는 기본적으로 private 로 선언후 프로퍼티를 생성한다. [필수적으로 캡슐화]
///   1-1. 직렬화할때 private 하고 속성 [SerializeField] 붙히는거나 public 으로 선언하는거나 작업량은 같다고한다. 
///         public도 내부적으론 직렬화 작업을 거쳐서 에디터에뿌린다고한다. private 하고 [SerializeField] 하는것과 같은방식으로 이루어진다.
///         
/// 2. private 로 선언후 변수명에 속성을 [SerializeField] 로 설정한다  - 유니티 기본 JsonUtility 에서 데이터 입출력시 접근하기위해 사용  
/// 
/// 3. 다중배열(2차원배열이상)은 1차원배열에 구조체를 넣고 구조체안에 다시 1차원배열로 구조를 짜는식으로 구조체를 이용하여 다중배열형식으로 만들고 변수로 넣는다.
/// 3-1. 구조체배열도 안된다? 아니 된다 데이터 제대로읽어온다. 
/// 
/// 4. 상속관계의 파싱은 안된다. 기본적으로 부모A 의 클래스로  jsonUtility 를 이용하면 A 객체의 자식의내용까진 저장한다 하지만 
///         불러올때 jsonUtility는 데이터를 받는 곳의  클래스형으로 파싱하기때문에 데이터(자식클래스데이터) 누락이 발생한다.
///    결론 : 저장파일 하나로 처리하려면 클래스하나에다 처리해야한다 ( 상속 X )
///    - 해당내용은 최종적으로 데이터를 꺼내쓸때의 문제이다  , 메모리상에는 자식의 데이터까지 전부 들어가있다.
/// ex) struct A{B[] b; int i;}; struct B{C[] c; int a;};  struct C{int a;};  
/// [SerializeField]
/// A a ; 
/// public A A => a; 
/// 
/// 4. 저장할때 바뀌지않는 값들을 저장하려면 SaveLoadManager.setDefaultInfo 함수를 참고 기본적으로 프로퍼티를 public 으로 선언한다.
/// 테스트 결과
/// 1. 해당클래스를 상속받아서 사용해보았지만 상속받은 클래스의 맴버변수는 저장은 되나 읽어올때 값이 제대로 들어가지지가 않는다. 
///  1-1. 해결: 함수호출시 상속받은 클래스를 넘기면 제대로 파싱이 된다. 로딩할때도 같은 객체를 사용하여야한다.
/// 2. JsonGameData 를 베이스로 상속받은 클래스를   유니티 싱글톤 객체에 적용하기위해 제네럴값으로 넣어봣지만 컴퍼넌트가없어서 싱글톤생성오류가난다. 
///  2-1. 해당 내용은 다른방법으로 수정을 생각하고있고 테스트중이다.
///  
/// 3. 아직 큐와 리스트 스택은 테스트를 안해보앗다. => 큐 리스트 스택 같은 자료구조는 직렬화 하기 어렵기때문에 데이터로 넣는건 비추천이다.
///  3-1 . 유니티에서의 직렬화는 참조깊이를 제한을 10단계로 제한을두었는데 이는 순환참조가 문제가 되서 그런것이다 
///        큐는 저장하는 자료구조로 적합하지않고 , 리스트의경우 순환참조를 염두해서 구조를 만들어야 제대로된 직렬화가 이루어진다.
///        스택또한 저장하는 자료구조로는 적합하지않다.
///        
///  ************* 캐릭터 변수선언만하고 해당클래스를 상속하여 파싱함수를 제작한다. 
///                 - 이때 순환참조를 염두하고 상속해야하고 참조깊이(상속도포함)를 10단계로 제한하기때문에 조심해야한다.***************
///  
/// MonoBehaviour 는  [Serializable] 를 지원하지않는다.
/// </summary>
///  
///직렬화 : 내부적으로 파싱작업에 필요하다고 한다. 
///
///PlayerPrefs 는 저장이 레지스트리에 저장된다. 데이터가 오픈되있어서 비추천이다. 저장경로바꿀수도없다. 보안에 취약 
///JsonGameData  a = new(); 형식도가능
///
[Serializable]
public class JsonGameData 
{
    //저장데이터 캐싱하기위한 인덱스 번호
    [SerializeField]
    int dataIndex;
    public int DataIndex {
        get => dataIndex;
        set{ 
            dataIndex = value;
        
        }
    }

    /// <summary>
    /// 현재 전투중인 배틀맵 
    /// </summary>
    [SerializeField]
    StageList currentStage = StageList.None;
    public StageList CurrentStage 
    {
        get => currentStage;
        set => currentStage = value;
    }

    /// <summary>
    /// 스테이지 클리어 여부 저장
    /// </summary>
    [SerializeField]
    StageList stageClear;
    public StageList StageClear 
    {
        get => stageClear;
        set => stageClear = value;
    }
    /// <summary>
    /// 마을에서의 시작위치값 저장
    /// </summary>
    [SerializeField]
    Vector3 startPos;
    public Vector3 StartPos 
    {
        get => startPos;
        set => startPos = value;
    }

    /// <summary>
    /// 저장시간 넣어두기 
    /// </summary>
    [SerializeField]
    string saveTime;
    public string SaveTime { 
        get => saveTime;
        set { 
            saveTime = value;
        }
    }
    
    /// <summary>
    /// 불러오기시 사용될 씬정보 
    /// </summary>
    [SerializeField]
    EnumList.SceneName sceanName;
    public EnumList.SceneName SceanName
    {
        get => sceanName;
        set
        {
            sceanName = value;
        }
    }
    [SerializeField]
    Base_Status playerData;
    public Base_Status PlayerData 
    {
        get => playerData;
        set => playerData = value;
    }
    [SerializeField]
    Equipments_Data_Server equipments_Data;
    public Equipments_Data_Server Equipments_Data
    {
        get => equipments_Data;
        set => equipments_Data = value;
    }

    /// <summary>
    /// 장비 슬롯갯수 
    /// </summary>
    [SerializeField]
    int equipSlotLength;
    public int EquipSlotLength 
    {   
        get => equipSlotLength;
        set => equipSlotLength = value;
    }

    /// <summary>
    /// 캐릭터 소지아이템 리스트 -장비
    /// </summary>
    [SerializeField]
    CharcterItems[] equipData;
    public CharcterItems[] EquipData
    {
        get => equipData;
        set => equipData = value;
    }
    /// <summary>
    /// 소모 슬롯갯수 
    /// </summary>
    [SerializeField]
    int consumeSlotLength;
    public int ConsumeSlotLength
    {
        get => consumeSlotLength;
        set => consumeSlotLength = value;
    }
    /// <summary>
    /// 캐릭터 소지아이템 리스트 -소비
    /// </summary>
    [SerializeField]
    CharcterItems[] consumeData;
    public CharcterItems[] ConsumeData
    {
        get => consumeData;
        set => consumeData = value;
    }
    /// <summary>
    /// 기타 슬롯갯수 
    /// </summary>
    [SerializeField]
    int etcSlotLength;
    public int EtcSlotLength
    {
        get => etcSlotLength;
        set => etcSlotLength = value;
    }
    /// <summary>
    /// 캐릭터 소지아이템 리스트 -기타
    /// </summary>
    [SerializeField]
    CharcterItems[] etcData;
    public CharcterItems[] EtcData
    {
        get => etcData;
        set => etcData = value;
    }
    /// <summary>
    /// 조합 슬롯갯수 
    /// </summary>
    [SerializeField]
    int craftSlotLength;
    public int CraftSlotLength
    {
        get => craftSlotLength;
        set => craftSlotLength = value;
    }
    /// <summary>
    /// 캐릭터 소지아이템 리스트 -조합
    /// </summary>
    [SerializeField]
    CharcterItems[] craftData;
    public CharcterItems[] CraftData
    {
        get => craftData;
        set => craftData = value;
    }

    /// <summary>
    /// 중용씨가 만든 클래스 연결 
    /// </summary>
    [SerializeField]
    Save_SkillData[] skillDatas;
    public Save_SkillData[] SkillDatas 
    {
        get => skillDatas;
        set => skillDatas = value;
    }
    /// <summary>
    /// 캐릭터 퀘스트정보 리스트
    /// </summary>
    [SerializeField]
    StructList.CharcterQuest[] questList;
    public StructList.CharcterQuest[] QuestList
    {
        get => questList;
        set
        {
            questList = value;
        }
    }

  
}
