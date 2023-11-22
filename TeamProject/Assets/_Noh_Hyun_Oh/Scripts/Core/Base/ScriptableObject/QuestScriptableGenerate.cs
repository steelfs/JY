using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임내에서 한번만 생성되고 퀘스트 의 원본을 관리할 클래스 
/// </summary>
public class QuestScriptableGenerate : BaseScriptableObjectGenerate<Gyu_QuestBaseData>
{
    /// <summary>
    /// 게임시작시 퀘스트에대한 인덱스를 셋팅하기위한 값 
    /// 퀘스트 인덱스 를 정할 변수값
    /// </summary>
    int questIndex = 0;

    /// <summary>
    /// 퀘스트 종류별 아이디로 구분하기위한 인덱스 갭을 준다. 
    /// 퀘스트 종류별로 해당 갭이상으로 만들수없게 하기위한 변수
    /// </summary>
    int questIndexGap = 1000;
    public int QuestIndexGap => questIndexGap;
    
    /// <summary>
    /// 퀘스트의 갯수를 파악할 변수값
    /// </summary>
    //[SerializeField]
    //int questCount = 0;

    /// <summary>
    /// 메인 퀘스트 스크립터블 데이터 원형 배열
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originMainStoryQuestArray;

    /// <summary>
    /// 토벌 퀘스트 스크립터블 데이터 원형 배열
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originKillcountQuestArray;

    /// <summary>
    /// 수집 퀘스트 스크립터블 데이터 원형 배열
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originGatheringQuestArray;


    [Header("게임상의 모든 퀘스트 배열")]
    /// <summary>
    /// 메인 퀘스트 담아둘 배열 
    /// </summary>
    Gyu_QuestBaseData[] mainStoryQuestArray;
    public Gyu_QuestBaseData[] MainStoryQuestArray => mainStoryQuestArray;

    /// <summary>
    /// 토벌 퀘스트 담아둘 배열 
    /// </summary>
    Gyu_QuestBaseData[] killcountQuestArray;
    public Gyu_QuestBaseData[] KillcountQuestArray => killcountQuestArray;

    /// <summary>
    /// 수집 퀘스트 담아둘 배열
    /// </summary>
    Gyu_QuestBaseData[] gatheringQuestArray;
    public Gyu_QuestBaseData[] GatheringQuestArray => gatheringQuestArray;

    private void Awake()
    {
        mainStoryQuestArray = QuestDataGenerate(originMainStoryQuestArray);
        killcountQuestArray = QuestDataGenerate(originKillcountQuestArray);
        gatheringQuestArray = QuestDataGenerate(originGatheringQuestArray);
        SetAllDataIndexing();
    }
    /// <summary>
    /// 게임 초기화시 퀘스트의 인덱스를 셋팅하기위해 실행시킬로직 
    /// </summary>
    private void SetAllDataIndexing() 
    {
        int questTypeLength = Enum.GetValues(typeof(QuestType)).Length;     //이넘 갯수를 찾아서  

        for (int i = 0; i < questTypeLength; i++) 
        {
            questIndex = questIndexGap * i; 
            switch ((QuestType)i)
            {
                case QuestType.Story:
                    ArrayIndexsing(mainStoryQuestArray);
                    break;
                case QuestType.Killcount:
                    ArrayIndexsing(killcountQuestArray);
                    break;
                case QuestType.Gathering:
                    ArrayIndexsing(gatheringQuestArray);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 초기화시 실행되며
    /// 인덱스를 셋팅하기위한 함수
    /// </summary>
    /// <param name="resetDataArray">인덱스 셋팅할 퀘스트 배열</param>
    private void ArrayIndexsing(Gyu_QuestBaseData[] resetDataArray) 
    {

        foreach (Gyu_QuestBaseData questData in resetDataArray)
        {
            questData.QuestId = questIndex;
            questIndex++;
            
        }
    }
    /// <summary>
    /// 원본데이터는 놔두고 진행중인 값들만 전부 초기화 하는 함수 
    /// </summary>
    public void ResetData() 
    {
        ArrayDataReset(mainStoryQuestArray);
        ArrayDataReset(killcountQuestArray);
        ArrayDataReset(gatheringQuestArray);
    }
    /// <summary>
    /// 수정이 안되야 될 값은 놔두고 
    /// 게임진행시 변동되는 데이터만 리셋시키는 함수 
    /// </summary>
    /// <param name="resetDataArray">리셋할 배열</param>
    private void ArrayDataReset(Gyu_QuestBaseData[] resetDataArray)
    {
        foreach (Gyu_QuestBaseData questData in resetDataArray)
        {
            questData.ResetData();
        }
    }

    /// <summary>
    /// 아이템 갯수가 변경되면 실행될 함수 
    /// </summary>
    public void InventoryItemCountSetting(int itemCount, ItemCode itemCode) 
    {
        //모든 퀘스트의 내용을 변경시키자.
        int questTypeLength = Enum.GetValues(typeof(QuestType)).Length;     //이넘 갯수를 찾아서  

        for (int i = 0; i < questTypeLength; i++)
        {
            switch ((QuestType)i)
            {
                case QuestType.Story:
                    QuestCurrentCountingSetting(mainStoryQuestArray);
                    break;
                case QuestType.Killcount:
                    QuestCurrentCountingSetting(killcountQuestArray);
                    break;
                case QuestType.Gathering:
                    QuestCurrentCountingSetting(gatheringQuestArray);
                    break;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="questArray"></param>
    private void QuestCurrentCountingSetting(Gyu_QuestBaseData[] questArray) 
    {

    }
}
