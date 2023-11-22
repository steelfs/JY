using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : NpcBase_Gyu
{
    /// <summary>
    /// 현재 NPC가  진행중인 퀘스트
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData currentQuest;
    public Gyu_QuestBaseData CurrentQuest => currentQuest;

    /// <summary>
    /// 퀘스트 리스트 초기 크기 잡기용 에디터상에서 설정
    /// </summary>
    [SerializeField]
    int questCapasity = 4;

    /// <summary>
    /// 이엔피씨가 가지고있는 퀘스트 리스트
    /// </summary>
    [SerializeField]
    List<Gyu_QuestBaseData> ownQuestList;
    public List<Gyu_QuestBaseData> OwnQuestList => ownQuestList;

    /// <summary>
    /// 퀘스트 매니저 가져오기
    /// </summary>
    Gyu_QuestManager questManager;

 


    private void Start()
    {
        questManager = WindowList.Instance.Gyu_QuestManager;
        questManager.onChangeQuest += (value) =>
        {
            currentQuest = value;
            if (currentQuest != null)
            {
                switch (currentQuest.QuestType)
                {
                    case QuestType.Story:
                        talkType = TalkType.Story;
                        break;
                    case QuestType.Killcount:
                        talkType = TalkType.KillCount;
                        break;
                    case QuestType.Gathering:
                        talkType = TalkType.Gathering;
                        break;
                }
            }
            else 
            {
                talkType = TalkType.Comunication;
            }
        };
    }
    /// <summary>
    /// 엔피씨가 들고 있어야할 데이터 
    /// </summary>
    /// <param name="mainStoryQuestArray">메인 퀘스트</param>
    /// <param name="killcountQuestArray">토벌 퀘스트</param>
    /// <param name="gatheringQuestArray">수집 퀘스트</param>
    public void InitQuestData(Gyu_QuestBaseData[] mainStoryQuestArray, Gyu_QuestBaseData[] killcountQuestArray, Gyu_QuestBaseData[] gatheringQuestArray)
    {
        ownQuestList = new(questCapasity);
        SetMainStoryQuest(mainStoryQuestArray);
        SetKillcountQuest(killcountQuestArray);
        SetGatheringQuest(gatheringQuestArray);
    }


    private void SetGatheringQuest(Gyu_QuestBaseData[] gatheringQuestArray)
    {
        foreach (var item in gatheringQuestArray)
        {
            ownQuestList.Add(item);
        }
    }
    private void SetKillcountQuest(Gyu_QuestBaseData[] killcountQuestArray)
    {
        foreach (var item in killcountQuestArray)
        {
            ownQuestList.Add(item);
        }
    }
    private void SetMainStoryQuest(Gyu_QuestBaseData[] mainStoryQuestArray)
    {
        foreach (var item in mainStoryQuestArray)
        {
            ownQuestList.Add(item);
        }
    }
}
