using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// ���忡�ʿ��ѵ����ʹ� Ŭ���� Ŀ��Ʈ �ΰ���.
/// </summary>
public class PlayerQuest_Gyu : MonoBehaviour
{
    /// <summary>
    /// �����Ҽ��ִ� ����Ʈ�� ���� 
    /// </summary>
    [SerializeField]
    int questMaxLength = 10;
    public int QuestMaxLength => questMaxLength;

    /// <summary>
    /// ĳ���Ͱ� �������� ����Ʈ ���
    /// </summary>
    [SerializeField]
    List<Gyu_QuestBaseData> currentQuests;
    public List<Gyu_QuestBaseData> CurrentQuests => currentQuests;

    /// <summary>
    /// ĳ���Ͱ� �Ϸ��� ����Ʈ ��� 
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
    /// ����Ʈ ������ ����Ʈ�� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="addQuest">�߰��� ����Ʈ</param>
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
        Debug.Log("���̻� ����Ʈ�� �����Ҽ� �����ϴ�.");
    }

    /// <summary>
    /// ����Ʈ ��ҽ� ó���� ����
    /// </summary>
    /// <param name="cancelQuest">����� ����Ʈ</param>
    public void CancelQuest(Gyu_QuestBaseData cancelQuest) 
    {
        if (currentQuests.Contains(cancelQuest))
        {
            cancelQuest.Quest_State = Quest_State.None;
            currentQuests.Remove(cancelQuest);
            return;
        }
        Debug.LogWarning($"����� ����Ʈ {cancelQuest} �� ĳ���Ͱ� �����������ʽ��ϴ�.");
    }

    /// <summary>
    /// ����Ʈ �Ϸ�� ������ó���Լ�
    /// </summary>
    /// <param name="clearQuest">�Ϸ��� ����Ʈ</param>
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
        Debug.LogWarning($"Ŭ������ ����Ʈ�� : {clearQuest} , ĳ���Ͱ� �������ִ�����Ʈ�� �ƴմϴ�.");
    }

    /// <summary>
    /// ����ó���� �Լ�
    /// </summary>
    /// <param name="clearData">Ŭ������ ����Ʈ</param>
    private void RewardDataSetting(Gyu_QuestBaseData clearData)
    {
        // ����ó�������� ĳ���� �κ��丮�� �������ؼ� ó���ؾ��Ѵ�.
        int forSize = clearData.RewardItem.Length;
        int rewardSize = 0;
        for (int i = 0; i < forSize; i++)
        {
            rewardSize = clearData.ItemCount[i];//���� ������ŭ 
            for (int j = 0; j < rewardSize; j++) 
            {
                GameManager.SlotManager.AddItem(clearData.RewardItem[i]); //�ϳ��� ���� 
            }
        }
        GameManager.PlayerStatus.DarkForce += (uint)clearData.RewardCoin; //��ũ���� ������Ű��

        /// ����Ʈ ������ ���� ��Ű�� 
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
