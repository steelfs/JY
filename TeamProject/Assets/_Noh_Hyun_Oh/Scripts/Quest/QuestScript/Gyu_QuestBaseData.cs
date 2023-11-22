using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 세이브 할때 필요한데이터 
/// id , currentCount 이 두개의데이터만 저장한다.
/// </summary>
[CreateAssetMenu(fileName = "New Quest Data", menuName = "Scriptable Object/QuestDatas/Quest", order = 1)]
public class Gyu_QuestBaseData : ScriptableObject
{
    [Header("퀘스트의 기본정보")]
    /// <summary>
    /// 퀘스트 id
    /// </summary>
    int questId = -1;
    public int QuestId
    {
        get => questId;
        set
        {
            //이셋프로퍼티는 맨처음 데이터셋팅할때 한번만 실행하도록 구조를짠다.
            if (questId < 0) // 퀘스트아이디가 -1 즉 초기값일때만 값을 셋팅하도록 한다.
            {
                questId = value;
            }
        }
    }

    /// <summary>
    /// 퀘스트 타입
    /// </summary>
    [SerializeField]
    QuestType type;
    public QuestType QuestType => type;

    /// <summary>
    /// 퀘스트 아이콘
    /// </summary>
    [SerializeField]
    Sprite iconImage;
    public Sprite IconImage => iconImage;

    /// <summary>
    /// 퀘스트 제목
    /// </summary>
    [SerializeField]
    string title;
    public string Title => title;

    /// <summary>
    /// 퀘스트 내용
    /// </summary>
    [SerializeField]
    string description;
    public string Description => description;

    /// <summary>
    /// 퀘스트 목표
    /// </summary>
    [SerializeField]
    string clearObjectives;
    public string ClearObjectives => clearObjectives;

    //--------------------------- 보상관련 변수 
    [Header("퀘스트의 보상 관련 정보")]
    /// <summary>
    /// 퀘스트의 보상 금액
    /// </summary>
    [SerializeField]
    int rewardCoin;
    public int RewardCoin => rewardCoin;

    /// <summary>
    /// 퀘스트의 보상 아이템
    /// </summary> 
    [SerializeField]
    ItemCode[] rewardItem;
    public ItemCode[] RewardItem => rewardItem;

    /// <summary>
    /// 보상아이템의 갯수
    /// 사용시 위의 퀘스트보상 배열과 1:1로 인덱스 매칭시켜야한다.
    /// </summary>
    [SerializeField]
    int[] itemCount;
    public int[] ItemCount => itemCount;



    //------------------------------- 퀘스트 진행관련 변수 

    [Header("퀘스트의 수행에 관련된 정보")]
    /// <summary>
    /// 수집 퀘스트에 필요한 아이템
    /// </summary> 
    [SerializeField]
    ItemCode[] requestItem;
    public ItemCode[] RequestItem => requestItem;

    /// <summary>
    /// 토벌 퀘스트에 필요한 몬스터 목록
    /// </summary>
    [SerializeField]
    Monster_Type[] questMosters;
    public Monster_Type[] QuestMosters => questMosters;


    /// <summary>
    /// 클리어까지 필요한 카운트(갯수)
    /// </summary>
    [SerializeField]
    int[] requiredCount;
    public int[] RequiredCount => requiredCount;

    /// <summary>
    /// 현재 카운트(갯수)
    /// </summary>
    [SerializeField]
    int[] currentCount;
    public int[] CurrentCount => currentCount;

    /// <summary>
    /// 퀘스트에대한 상태 정보
    /// </summary>
    Quest_State quest_State = Quest_State.None;
    public virtual Quest_State Quest_State
    {
        get => quest_State;
        set 
        {
            quest_State = value;
        }
    }


    private void Awake()
    {
       currentCount = new int[RequiredCount.Length];
    }

    /// <summary>
    /// 불러올시 퀘스트 진행도 처리하기위한 함수
    /// 결국 Setter 다.. 
    /// </summary>
    /// <param name="countArray">저장된 진행상황데이터</param>
    /// <param name="state">저장된 상태 데이터</param>
    public void SaveFileDataPasing(int[] countArray,Quest_State state) 
    {
        currentCount = countArray;
        quest_State = state;
    }

    /// <summary>
    /// 수집 퀘스트 아이템 코드를 가지고 퀘스트 진행값을 늘리는 함수
    /// 퀘스트 만들때 중복된 아이템을 가져오라고 하면 버그가발생할것이다
    /// </summary>
    /// <param name="requestItemCode">아이템코드</param>
    /// <param name="requestItemCount">현재 수집한 아이템 갯수</param>
    public void SetCounting( int requestItemCount, ItemCode requestItemCode) 
    {
        for (int i = 0; i < requestItem.Length; i++)
        {
            if (requestItem[i] == requestItemCode) 
            {
                currentCount[i] = requestItemCount;
                break;
            }
        }
    }

    /// <summary>
    /// 토벌 퀘스트 카운팅 증가용 
    /// 미완성 몬스터 가완성되야 될듯하다 
    /// 몬스터 종류에따른 이넘값을 인자로받고 인자를 여기클래스에다가 변수로 따로지정해두고 그것을 비교하는로직필요 
    /// 기본적으로 수집 퀘스트 비교랑 동일하다.
    /// 퀘스트 만들때 중복된 몬스터를을 처리하라고 하면 버그가발생할것이다
    /// </summary>
    /// <param name="monsterType">몬스터 종류</param>
    /// <param name="addCount">추가될 카운트</param>
    public void SetCounting(Monster_Type monsterType, int addCount) 
    {
        for (int i = 0; i < questMosters.Length; i++)
        {
            if (questMosters[i] == monsterType)
            {
                currentCount[i] += addCount; //아니면 카운팅
                break;
            }
        }
    }

    /// <summary>
    /// 퀘스트가 현재 진행중인 퀘스트상황
    /// </summary>
    /// <returns>완료됬거나 퀘스트가 없을땐  true 아니면 false </returns>
    public bool IsSucess() 
    {
        int requestArrayLength = requiredCount.Length;
        for (int i = 0; i < requestArrayLength; i++)
        {
            if (requiredCount[i] > currentCount[i]) //완료 안됬는지 체크해서  
            {
                return false; //클리어 안됬으면 안됬다고 반환
            }
        }
        return true; // 클리어 됬으면 true 로주자
    }

    /// <summary>
    /// 진행상황을 초기화 하는 함수
    /// </summary>
    public void ResetData() 
    {
        int length = currentCount.Length;
        for (int i = 0; i < length; i++)
        {
            currentCount[i] = 0;
        }
        quest_State = Quest_State.None;
    }
}
