using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : NpcBase_Gyu
{
    /// <summary>
    /// ���� NPC��  �������� ����Ʈ
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData currentQuest;
    public Gyu_QuestBaseData CurrentQuest => currentQuest;

    /// <summary>
    /// ����Ʈ ����Ʈ �ʱ� ũ�� ���� �����ͻ󿡼� ����
    /// </summary>
    [SerializeField]
    int questCapasity = 4;

    /// <summary>
    /// �̿��Ǿ��� �������ִ� ����Ʈ ����Ʈ
    /// </summary>
    [SerializeField]
    List<Gyu_QuestBaseData> ownQuestList;
    public List<Gyu_QuestBaseData> OwnQuestList => ownQuestList;

    /// <summary>
    /// ����Ʈ �Ŵ��� ��������
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
    /// ���Ǿ��� ��� �־���� ������ 
    /// </summary>
    /// <param name="mainStoryQuestArray">���� ����Ʈ</param>
    /// <param name="killcountQuestArray">��� ����Ʈ</param>
    /// <param name="gatheringQuestArray">���� ����Ʈ</param>
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
