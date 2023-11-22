using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 저장에필요한데이터는 클리어 커런트 두개다.
/// </summary>
public class PlayerQuest_Gyu : MonoBehaviour
{
    /// <summary>
    /// 수락할수있는 퀘스트의 갯수 
    /// </summary>
    [SerializeField]
    int questMaxLength = 10;
    public int QuestMaxLength => questMaxLength;

    /// <summary>
    /// 캐릭터가 수행중인 퀘스트 목록
    /// </summary>
    [SerializeField]
    List<Gyu_QuestBaseData> currentQuests;
    public List<Gyu_QuestBaseData> CurrentQuests => currentQuests;

    /// <summary>
    /// 캐릭터가 완료한 퀘스트 목록 
    /// </summary>
    [SerializeField]
    List<Gyu_QuestBaseData> clearQuestList;
    public List<Gyu_QuestBaseData> ClearQuestList => clearQuestList;


    private void Awake()
    {
        currentQuests = new List<Gyu_QuestBaseData>(questMaxLength);
        clearQuestList = new List<Gyu_QuestBaseData>(questMaxLength);
    }

    private void Start()
    {
        SpaceSurvival_GameManager.Instance.getPlayerQuest = () => this;
    }

    /// <summary>
    /// 퀘스트 수락시 리스트에 추가하는 함수
    /// </summary>
    /// <param name="addQuest">추가할 퀘스트</param>
    public void AppendQuest(Gyu_QuestBaseData addQuest)
    {
        if (questMaxLength > currentQuests.Count) 
        {
            addQuest.Quest_State = Quest_State.Quest_Start;
            currentQuests.Add(addQuest);
            if (addQuest.QuestType == QuestType.Story) 
            {
                SpaceSurvival_GameManager.Instance.IsBoss = true;
            }
            return;
        }
        Debug.Log("더이상 퀘스트를 수락할수 없습니다.");
    }

    /// <summary>
    /// 퀘스트 취소시 처리할 로직
    /// </summary>
    /// <param name="cancelQuest">취소할 퀘스트</param>
    public void CancelQuest(Gyu_QuestBaseData cancelQuest) 
    {
        if (currentQuests.Contains(cancelQuest))
        {
            cancelQuest.Quest_State = Quest_State.None;
            currentQuests.Remove(cancelQuest);
            return;
        }
        Debug.LogWarning($"취소한 퀘스트 {cancelQuest} 는 캐릭터가 가지고있지않습니다.");
    }

    /// <summary>
    /// 퀘스트 완료시 데이터처리함수
    /// </summary>
    /// <param name="clearQuest">완료한 퀘스트</param>
    public void ClearQuest(Gyu_QuestBaseData clearQuest)
    {
        if (currentQuests.Contains(clearQuest))
        {
            clearQuest.Quest_State = Quest_State.Quest_Complete;
            clearQuestList.Add(clearQuest);
            currentQuests.Remove(clearQuest);
            RewardDataSetting(clearQuest);

            return;
        }
        Debug.LogWarning($"클리어한 퀘스트는 : {clearQuest} , 캐릭터가 가지고있는퀘스트가 아닙니다.");
    }

    /// <summary>
    /// 보상처리용 함수
    /// </summary>
    /// <param name="clearData">클리어한 퀘스트</param>
    private void RewardDataSetting(Gyu_QuestBaseData clearData)
    {
        // 보상처리를위해 캐릭터 인벤토리에 연결을해서 처리해야한다.
        int forSize = clearData.RewardItem.Length;
        int rewardSize = 0;
        for (int i = 0; i < forSize; i++)
        {
            rewardSize = clearData.ItemCount[i];//보상 갯수만큼 
            for (int j = 0; j < rewardSize; j++) 
            {
                GameManager.SlotManager.AddItem(clearData.RewardItem[i]); //하나씩 증가 
            }
        }
        GameManager.PlayerStatus.DarkForce += (uint)clearData.RewardCoin; //다크포스 증가시키기

        /// 퀘스트 아이템 감소 시키기 
        if (clearData.QuestType == QuestType.Gathering)
        {
            forSize = clearData.RequestItem.Length;
            for (int i = 0; i < forSize; i++)
            {
                GameManager.SlotManager.RemoveItem(clearData.RequestItem[i], clearData.RequiredCount[i]);
            }
        }
    }

    public void ResetData() 
    {
        foreach (var quest in currentQuests)
        {
            quest.ResetData();
        }
        currentQuests.Clear();
        foreach (var quest in clearQuestList)
        {
            quest.ResetData();
        }
        clearQuestList.Clear();
    }

   


}
