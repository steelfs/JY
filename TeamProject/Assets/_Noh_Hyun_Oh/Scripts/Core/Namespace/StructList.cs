
using System;
using UnityEngine;
/// <summary>
/// 게임의 저장할 데이터를 정의 하는 클래스로 지정한다 .
/// 작성방법 
/// 1. 변수는 기본적으로 private 로 선언후 프로퍼티를 생성한다. [필수적으로 캡슐화]
/// 2. private 로 선언후 변수명에 속성을 [SerializeField] 로 설정한다  - 유니티 기본 JsonUtility 에서 데이터 입출력시 접근하기위해 사용  
///  2-1. 유니티에서는 public 선언된 변수를 기본적으로 직렬화 작업을한다고한다 그러므로 private 로 [SerializeField] 로 선언한거랑 작업량은 같다.
///         - https://docs.unity3d.com/ScriptReference/SerializeField.html 참조
/// 3. 다중배열(2차원배열이상)은 1차원배열에 구조체를 넣고 구조체안에 다시 1차원배열로 구조를 짜는식으로 구조체를 이용하여 다중배열형식으로 만들고 변수로 넣는다. 테스트코드는 
/// ex) struct A{B[] b; int i;}; struct B{C[] c; int a;};  struct C{int a;};  
/// A a ; 
/// public A A => a; 
/// 4. JsonUtility 에서는 자료의 깊이를 10단계로 정하고있다 이이상되는것은 파싱이 안된다.
///     ex ) A 구조체 안에 맴버 B 구조체   B구조체 안에 맴버 C 구조체 이런식으로  참조해서들어갓는데 다시참조해서 접근해야되는 깊이를 말한다.
///     기본적으로 Vector3 도 구조체안에서 사용하면 깊이가 10이넘어갈 가능성이 있어서 에러를 발생한다.
///     그래서 필요한 값만 저장하도록하자 
/// 테스트 결과
/// 1. 해당클래스를 상속받아서 사용해보았지만 상속받은 클래스의 맴버변수는 저장은 되나 읽어올때 값이 제대로 들어가지지가 않는다. 
///  1-1. 해결: 함수호출시 상속받은 클래스를 넘기면 제대로 파싱이 된다. 로딩할때도 같은 객체를 사용하여야한다.
/// 2. JsonGameData 를 베이스로 상속받은 클래스를   유니티 싱글톤 객체에 적용하기위해 제네럴값으로 넣어봣지만 컴퍼넌트가없어서 싱글톤생성오류가난다. 
///  2-1. 해당 내용은 다른방법으로 수정을 생각하고있고 테스트중이다.
///  
/// 3. 아직 큐와 리스트 스택은 테스트를 안해보앗다.
/// 4. 
///  ************* 캐릭터 변수선언만하고 해당클래스를 상속하여 파싱함수를 제작한다.***************
/// 
/// MonoBehaviour 는  [Serializable] 를 지원하지않는다.
/// </summary>
namespace StructList {
    /// <summary>
    /// 캐릭터 가소지중인 아이템 담을 구조체 내용필요하면 여기에추가
    /// </summary>
    [Serializable]
    public struct CharcterItems
    {
        /// <summary>
        /// 아이템수량
        /// </summary>
        [SerializeField]
        uint values; 
        public uint Values
        {
            get => values;
            set
            {
                values = value;
            }
        }

        /// <summary>
        /// 아이템 고유 키값
        /// </summary>
        [SerializeField]
        ItemCode itemIndex;
        public ItemCode ItemIndex
        {
            get => itemIndex;
            set
            {
                itemIndex = value;
            }
        }
        /// <summary>
        /// 아이템 슬롯번호 
        /// </summary>
        [SerializeField]
        uint slotIndex;
        public uint SlotIndex 
        {
            get=> slotIndex;
            set 
            {
                slotIndex = value;
            }
        }
        /// <summary>
        /// 아이템 강화 값
        /// </summary>
        [SerializeField]
        byte itemEnhanceValue;
        public byte ItemEnhanceValue 
        {
            get => itemEnhanceValue;
            set => itemEnhanceValue = value;    
        }
    }

    /// <summary>
    /// 캐릭터 가소지중인 스킬 담을 구조체 내용필요하면 여기에추가
    /// </summary>
    [Serializable]
    public struct CharcterSkills
    {
        /// <summary>
        /// 스킬 관련 변수 
        /// </summary>
        [SerializeField]
        int levelValue;
        public int LevelValue
        {
            get => levelValue;
            set
            {
                levelValue = value;
            }
        }
        /// <summary>
        /// 스킬 고유번호 
        /// </summary>
        [SerializeField]
        int skillIndex;
        public int SkillIndex
        {
            get => skillIndex;
            set
            {
                skillIndex = value;
            }
        }
    }


    /// <summary>
    /// 캐릭터 의 정보 여기에 추가
    /// </summary>
    [Serializable]
    public struct CharcterInfo
    {
        /// <summary>
        /// 캐릭터이름
        /// </summary>
        [SerializeField]
        string charcterName;
        public String CharcterName
        {
            get => charcterName;
            set
            {
                charcterName = value;
            }
        }
        /// <summary>
        /// 캐릭터의 레벨
        /// </summary>
        [SerializeField]
        int level;
        public int Level
        {
            get => level;
            set
            {
                level = value;
            }
        }
       
        /// <summary>
        /// 캐릭터의 경험치
        /// </summary>
        [SerializeField]
        float exp;
        public float EXP
        {
            get => exp;
            set
            {
                exp = value;
            }
        }

        /// <summary>
        /// 캐릭터 소지금액
        /// </summary>
        [SerializeField]
        long money;
        public long Money
        {
            get => money;
            set
            {
                money = value;
            }
        }
        [SerializeField]
        int[] flagList;
        public int[] FlagList 
        {
            get => flagList;
            set => flagList = value;
        }
        /// <summary>
        /// 캐릭터 저장맵에서 좌표값 X 월드 좌표가 좋을거같다
        /// </summary>
        [SerializeField]
        float sceanPositionX;
        public float SceanPositionX 
        {
            get => sceanPositionX;
            set => sceanPositionX = value;
        }
        /// <summary>
        /// 캐릭터 저장맵에서 좌표값  Y 월드 좌표가 좋을거같다
        /// </summary>
        [SerializeField]
        float sceanPositionY;
        public float SceanPositionY 
        { 
            get => sceanPositionY;
            set => sceanPositionY = value;  
        }
        /// <summary>
        /// 캐릭터 저장맵에서 좌표값 Z 월드 좌표가 좋을거같다
        /// </summary>
        [SerializeField]
        float sceanPositionZ;
        public float SceanPositionZ 
        {
            get => sceanPositionZ; 
            set => sceanPositionZ = value;
        }

    }


    /// <summary>
    /// 캐릭터 가 진행중인 이벤트(퀘스트) 정보
    /// </summary>
    [Serializable]
    public struct CharcterQuest
    {
        /// <summary>
        /// 퀘스트 저장화면에보일 정보
        /// </summary>
        [SerializeField]
        string questInfo;
        public string QuestInfo 
        {
            get => questInfo;
            set => questInfo = value;
        }
        /// <summary>
        /// 이벤트 고유번호 
        /// </summary>
        [SerializeField]
        int questIndex;
        public int QuestIndex
        {
            get => questIndex;
            set => questIndex = value;
        }
        
        /// <summary>
        /// 퀘스트 타입 파싱을 조금더 간단하게 하기위해저장
        /// </summary>
        [SerializeField]
        QuestType questType;
        public QuestType QuestType 
        {
            get => questType;
            set => questType = value;
        }
        /// <summary>
        /// 퀘스트 상태 저장
        /// </summary>
        [SerializeField]
        Quest_State questState;
        public Quest_State QuestState 
        {
            get => questState;
            set => questState = value;
        }
        /// <summary>
        /// 이벤트 진행도
        /// </summary>
        [SerializeField]
        int[] questIProgress;
        public int[] QuestIProgress
        {
            get => questIProgress;
            set => questIProgress = value;
        }


    }

}