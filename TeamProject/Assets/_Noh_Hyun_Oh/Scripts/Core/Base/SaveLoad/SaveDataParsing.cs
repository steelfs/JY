using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataParsing : MonoBehaviour
{
    /// <summary>
    /// json�������� ������ ��ü
    /// </summary>
    JsonGameData saveData; 

    /// <summary>
    /// ������ ������ ������ ��ġ
    /// </summary>
    SlotManager slotManager;

    /// <summary>
    /// �������� ����Ʈ ���� ��ġ
    /// </summary>
    PlayerQuest_Gyu playerQuest;

    SkillBox playerSkill;

    Player_Status player_Status;
    private void Awake()
    {

        slotManager = FindObjectOfType<SlotManager>();
        playerQuest = FindObjectOfType<PlayerQuest_Gyu>();
        playerSkill = FindObjectOfType<SkillBox>();
        player_Status = FindObjectOfType<Player_Status>();
    }

    /// <summary>
    /// ���� �ϱ����� ���嵥���� ����� �Լ�
    /// </summary>
    public void SaveParsing()
    {
        saveData = new JsonGameData();                              //������ ��ü ����
        saveData.SkillDatas = playerSkill.SaveSkillData();
        saveData.PlayerData = player_Status.Base_Status;
        saveData.Equipments_Data = GameManager.EquipBox.Save_EquipmentsData();
        SaveInvenDataParsing();                                     //�κ��丮 ���� ������ �������� 
        SaveDataSetting();                                          //����Ʈ ĳ�������� ����Ʈ ������ ��������


        if (SpaceSurvival_GameManager.Instance.PlayerStartPos)
        {
            saveData.StartPos = SpaceSurvival_GameManager.Instance.PlayerStartPos.position;
        }
        else 
        {
            saveData.StartPos = SpaceSurvival_GameManager.Instance.ShipStartPos;
        }

        saveData.StageClear = SpaceSurvival_GameManager.Instance.StageClear; //�������� Ŭ���� ���� ����
        saveData.CurrentStage = SpaceSurvival_GameManager.Instance.CurrentStage; //���� �������� �������� ���� ����
        SaveLoadManager.Instance.GameSaveData = saveData;           //������������� ��ü�� ���
    }
    /// <summary>
    /// �ε� �� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="data"></param>
    public void LoadParsing(JsonGameData data)
    {
        ResetData();
        SpaceSurvival_GameManager.Instance.ShipStartPos = data.StartPos;
        LoadInvenDataParsing(data);
        LoadQuestDataParsing(data.QuestList);
        playerSkill.LoadSkillData_In_QuickSlot(data.SkillDatas);
        player_Status.Base_Status.LoadData(data.PlayerData);
        GameManager.EquipBox.Load_EquipmentsData(data.Equipments_Data);
        GameManager.PlayerStatus.Reset_Status();
        SpaceSurvival_GameManager.Instance.StageClear = data.StageClear;
        SpaceSurvival_GameManager.Instance.CurrentStage = data.CurrentStage;
        SpaceSurvival_GameManager.Instance.IsBattleMapClear = false;
        RefreshData();
    }


    /// <summary>
    /// ��������� �κ��丮���� �����ؼ� �����ͷ� ����� �۾� 
    /// </summary>
    private void SaveInvenDataParsing() 
    {

        int defaultSlotLength = 10; //�ʱ�ȭ�� �⺻���Լ�
        
        List<Slot> temp = slotManager.slots[Inventory_Tab.Equip]; //������ ���Գ��� �����ͼ�
        
        List<CharcterItems> tempList = new(); // ���嵥���� ����� 

        CharcterItems tempData = new CharcterItems(); // �����ϳ��� ������ ������ ��ü����� 

        ItemData_Enhancable enhanceItem; // ��ȭ�� ������� üũ�� ��������� 

        foreach (Slot slot in temp) //���� ���� �ϴ� ���鼭 
        {
            if (slot.ItemData != null) //�������� �ִ°͵��� 
            {
                enhanceItem = slot.ItemData as ItemData_Enhancable; //��ȭ �Ǵ°��� üũ�ؼ� 

                if (enhanceItem != null) //��ȭ�Ǵ°Ÿ� 
                {
                    tempData.ItemEnhanceValue = enhanceItem.itemLevel; //��ȭ�� ������ ��Ƶΰ�  0�� = 1  1���� = 2 ��
                }

                tempData.ItemIndex = slot.ItemData.code;            // ����Ⱥκ� �������̹������� �ڵ�� ���

                tempData.Values = slot.ItemCount;                   // ����� ������ ���
                
                tempData.SlotIndex = slot.Index;                    // ����� ���� ��ġ�� ���
                
                tempList.Add(tempData);                             //������ ���É����� ����Ʈ�� �߰��� �صд�
            }
        }

        saveData.EquipSlotLength = temp.Count - defaultSlotLength; // ���â�� �����߰������ Ȯ���ϱ����� ���� ������ �ʱⰪ�� ���� �����صд�

        saveData.EquipData = tempList.ToArray();                    //����Ʈ�� jsonUtil���� ���������������� �迭�� �ٽ� �ٲ㼭 ����� Ŭ������ �Է�

        tempList.Clear();                                           //����Ʈ ���� ���� ���� 

        temp = slotManager.slots[Inventory_Tab.Consume];  //�Һ�â ������

        SetTempData(temp,tempList);                                 //������ �����ͼ� ��Ƶα�

        saveData.ConsumeSlotLength = temp.Count - defaultSlotLength;//���� ���� ������ �����ϱ� 

        saveData.ConsumeData = tempList.ToArray();                  //���� �����ҵ����� �迭�� ��ȯ

        tempList.Clear();                                           //����Ʈ ���� ���� ���� 


        temp = slotManager.slots[Inventory_Tab.Etc];      //��Ÿâ ������

        SetTempData(temp,tempList);                                 //������ �����ͼ� ��Ƶα�
        
        saveData.EtcSlotLength = temp.Count - defaultSlotLength;    //���� ���� ������ �����ϱ� 
        
        saveData.EtcData = tempList.ToArray();                      //���� �����ҵ����� �迭�� ��ȯ
        
        tempList.Clear();                                           //����Ʈ ���� ���� ���� 



        temp = slotManager.slots[Inventory_Tab.Craft];    //���� ������
        
        SetTempData(temp,tempList);                                 //������ �����ͼ� ��Ƶα�
        
        saveData.CraftSlotLength = temp.Count - defaultSlotLength;  //���� ���� ������ �����ϱ� 
        
        saveData.CraftData = tempList.ToArray();                    //���� �����ҵ����� �迭�� ��ȯ
        
    }

    /// <summary>
    /// �ӽ� ����Ʈ�� �����µ����� ��� �Լ�
    /// </summary>
    /// <param name="invenTabData">�κ�â ���Կ����� ���� �Ǻ���</param>
    /// <param name="saveData">�����Ͱ� ����� ����Ʈ</param>
    private void SetTempData(List<Slot> invenTabData, List<CharcterItems> saveData) 
    {
        CharcterItems tempData = new CharcterItems(); // �����ϳ��� ������ ������ ��ü����� 
        foreach (Slot slot in invenTabData)
        {
            if (slot.ItemData != null)
            {

                tempData.ItemIndex = slot.ItemData.code;
                tempData.Values = slot.ItemCount;
                tempData.SlotIndex = slot.Index;
                saveData.Add(tempData);
            }
        }
    }


    /// <summary>
    /// �������� �ε�� ����ȳ��� ������ ������ �Ľ��۾�
    /// </summary>
    /// <param name="data">�������Ͽ��� �����µ����� </param>
    private void LoadInvenDataParsing(JsonGameData data) 
    {
      
        Slot temp = null; //���Գ��빰������ �ӽú���
        ItemData_Enhancable tempEnchan; //��þ������� üũ�� �ӽú���

        List<Slot> slots = slotManager.slots[Inventory_Tab.Equip]; //����ϰ��

        for (int slotIndex = 0; slotIndex < data.EquipSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Inventory_Tab.Equip); //���԰��� �߰��Ұ��������� �߰��صΰ� 
        }

        foreach (CharcterItems equipData in data.EquipData) //�������鼭 �����ͼ���
        {
            temp = slots[(int)equipData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[equipData.ItemIndex];
            temp.ItemCount = equipData.Values;
            if (equipData.ItemEnhanceValue > 0) //��þ������������ 
            {
                tempEnchan = temp.ItemData as ItemData_Enhancable; //��þ Ŭ������ ������ 
                if (tempEnchan != null)
                {
                    for (int i = 1; i < equipData.ItemEnhanceValue; i++) // ��þ����� �⺻���� 1���� ���������� �ʱⰪ 1�μ���
                    {
                        tempEnchan.LevelUpItemStatus(temp); //��þ�Ѹ�ŭ �߰��� ��þ�����ͼ���

                    }
                }

            }
        }
        //���ͺ���ϰ� �����Ѵ� �ؿ��� �ݺ��۾�

        slots = slotManager.slots[Inventory_Tab.Consume];

        for (int slotIndex = 0; slotIndex < data.ConsumeSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Inventory_Tab.Consume);
        }

        foreach (CharcterItems consumeData in data.ConsumeData)
        {
            temp = slots[(int)consumeData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[consumeData.ItemIndex];
            temp.ItemCount = consumeData.Values;

            ItemData_Potion potion = temp.ItemData as ItemData_Potion;
            if (temp.BindingSlot != null && potion != null)
            {

            }
         
        }



        slots = slotManager.slots[Inventory_Tab.Etc];

        for (int slotIndex = 0; slotIndex < data.EtcSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Inventory_Tab.Etc);
        }

        foreach (CharcterItems etcData in data.EtcData)
        {
            temp = slots[(int)etcData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[etcData.ItemIndex];
            temp.ItemCount = etcData.Values;
        }

        slots = slotManager.slots[Inventory_Tab.Craft];

        for (int slotIndex = 0; slotIndex < data.CraftSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Inventory_Tab.Craft);
        }

        foreach (CharcterItems craftData in data.CraftData)
        {
            temp = slots[(int)craftData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[craftData.ItemIndex];
            temp.ItemCount = craftData.Values;
        }


    }


    //----------------------------- ����Ʈ ����� �ҷ����� ------------------------------------
    /// <summary>
    /// ������ ����Ʈ ������ ���� �ϴ� �Լ�
    /// </summary>
    private void SaveDataSetting()
    {
        int saveQuestSize = playerQuest.CurrentQuests.Count + playerQuest.ClearQuestList.Count;
        CharcterQuest[] saveDataSetting = new CharcterQuest[saveQuestSize];
        int arrayIndex = 0;
        foreach (Gyu_QuestBaseData questData in playerQuest.CurrentQuests)
        {
            saveDataSetting[arrayIndex].QuestIndex = questData.QuestId;
            saveDataSetting[arrayIndex].QuestIProgress = questData.CurrentCount;
            saveDataSetting[arrayIndex].QuestType = questData.QuestType;
            saveDataSetting[arrayIndex].QuestState = questData.Quest_State;
            saveDataSetting[arrayIndex].QuestInfo = questData.Title;
            arrayIndex++;
        }
        foreach (Gyu_QuestBaseData questData in playerQuest.ClearQuestList)
        {
            saveDataSetting[arrayIndex].QuestIndex = questData.QuestId;
            saveDataSetting[arrayIndex].QuestIProgress = questData.CurrentCount;
            saveDataSetting[arrayIndex].QuestType = questData.QuestType;
            saveDataSetting[arrayIndex].QuestState= questData.Quest_State;
            saveDataSetting[arrayIndex].QuestInfo = questData.Title;
            arrayIndex++;
        }
        saveData.QuestList = saveDataSetting;
    }
    /// <summary>
    /// �ε�� ����Ʈ ������ �Ľ̰��� �Լ�
    /// </summary>
    private void LoadQuestDataParsing(CharcterQuest[] quests)
    {
        //�����ѵ����� �����ϱ�
        int loadDataQuestSize = quests.Length;
        for (int i = 0; i < loadDataQuestSize; i++) //����� �����͸�ŭ ������ 
        {
            switch (quests[i].QuestType)
            {
                case QuestType.Story:
                    SetQuestData(DataFactory.Instance.QuestScriptableGenerate.MainStoryQuestArray, quests[i].QuestIndex, quests[i].QuestIProgress, quests[i].QuestState);
                    break;
                case QuestType.Killcount:
                    SetQuestData(DataFactory.Instance.QuestScriptableGenerate.KillcountQuestArray, quests[i].QuestIndex, quests[i].QuestIProgress, quests[i].QuestState);
                    break;
                case QuestType.Gathering:
                    SetQuestData(DataFactory.Instance.QuestScriptableGenerate.GatheringQuestArray, quests[i].QuestIndex, quests[i].QuestIProgress, quests[i].QuestState);
                    break;
            }
        }

    }

    /// <summary>
    /// ����Ʈ �����Ϳ� �ҷ��� �����͸� ���ؼ� �Ϸ� ����Ʈ����Ʈ �� �������� ����Ʈ����Ʈ�� �ִ� �Լ�
    /// </summary>
    /// <param name="questArray">���� ����Ʈ</param>
    /// <param name="checkIndex">���Ͽ��� �ҷ��� ����Ʈ �ε�����</param>
    /// <param name="setValues">���Ͽ��� �ҷ��� ����Ʈ ���൵��</param>
    private void SetQuestData(Gyu_QuestBaseData[] questArray, int checkIndex, int[] setValues, Quest_State questState)
    {
        int questSize = questArray.Length;
        Gyu_QuestBaseData tempData;
        for (int i = 0; i < questSize; i++) // ���� ����Ʈ ���� ���� �����ϴ� �迭 �� ���� ������ .
        {
            tempData = questArray[i];
            if (checkIndex == tempData.QuestId)    // ����Ʈ ����߿� ���̵� �������ִ��� üũ�ؼ� 
            {
                tempData.SaveFileDataPasing(setValues,questState); //������ �����ϰ� 
                if (questState == Quest_State.Quest_Complete) //�Ϸ� �ȰŸ� 
                {
                    playerQuest.ClearQuestList.Add(tempData);       //�Ϸ� ����Ʈ �ʿ� �ִ´�.
                }
                else  //�Ϸ� �ȵȰŸ� 
                {
                    playerQuest.CurrentQuests.Add(tempData);                // �������� ����Ʈ������ �ְ� 
                }
                return;      //�Լ��� ������. 
            }
        }

    }



    /// <summary>
    /// ���� �ε�� �����͸� �ʱ�ȭ ��Ű�� �Լ� 
    /// �ʱ�ȭ�� �����Ϳ��ٰ� �ε��� �����͸� �Է��ؾ��Ѵ�.
    /// </summary>
    private void ResetData()
    {
        slotManager.SaveFileLoadedResetSlots(); //���������� �ϴٳ����� �ʱⰪ���� ����
        GameManager.EquipBox.ClearEquipBox();                       // ��� �ʱ�ȭ 
        playerQuest.ResetData();                //����Ʈ ������ ������
    }
    /// <summary>
    /// �����Ͱ� ������ ������ UI �� ����ȵǴ°͵��� �ѹ��� ó���ϴ� �Լ� 
    /// ������ ó�� �����ڿ� ���� 
    /// </summary>
    private void RefreshData()
    {
        //����Ʈ ������������ ���� ����ȵǼ� ������������.
    }

}
