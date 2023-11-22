using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataParsing : MonoBehaviour
{
    /// <summary>
    /// json형식으로 저장할 객체
    /// </summary>
    JsonGameData saveData; 

    /// <summary>
    /// 저장할 아이템 데이터 위치
    /// </summary>
    SlotManager slotManager;

    /// <summary>
    /// 진행중인 퀘스트 정보 위치
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
    /// 저장 하기전에 저장데이터 만드는 함수
    /// </summary>
    public void SaveParsing()
    {
        saveData = new JsonGameData();                              //저장할 객체 생성
        saveData.SkillDatas = playerSkill.SaveSkillData();
        saveData.PlayerData = player_Status.Base_Status;
        saveData.Equipments_Data = GameManager.EquipBox.Save_EquipmentsData();
        SaveInvenDataParsing();                                     //인벤토리 에서 데이터 가져오기 
        SaveDataSetting();                                          //퀘스트 캐릭터한테 퀘스트 데이터 가져오기


        if (SpaceSurvival_GameManager.Instance.PlayerStartPos)
        {
            saveData.StartPos = SpaceSurvival_GameManager.Instance.PlayerStartPos.position;
        }
        else 
        {
            saveData.StartPos = SpaceSurvival_GameManager.Instance.ShipStartPos;
        }

        saveData.StageClear = SpaceSurvival_GameManager.Instance.StageClear; //스테이지 클리어 정보 저장
        saveData.CurrentStage = SpaceSurvival_GameManager.Instance.CurrentStage; //현재 전투중인 스테이지 정보 저장
        SaveLoadManager.Instance.GameSaveData = saveData;           //저장로직에사용될 객체에 담기
    }
    /// <summary>
    /// 로드 시 데이터 셋팅하는 함수
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
    /// 게임저장시 인벤토리내용 접근해서 데이터로 만드는 작업 
    /// </summary>
    private void SaveInvenDataParsing() 
    {

        int defaultSlotLength = 10; //초기화시 기본슬롯수
        
        List<Slot> temp = slotManager.slots[Inventory_Tab.Equip]; //저장탭 슬롯내용 가져와서
        
        List<CharcterItems> tempList = new(); // 저장데이터 만들고 

        CharcterItems tempData = new CharcterItems(); // 슬롯하나당 데이터 셋팅할 객체만들고 

        ItemData_Enhancable enhanceItem; // 강화된 장비인지 체크할 변수만들고 

        foreach (Slot slot in temp) //슬롯 내용 싹다 돌면서 
        {
            if (slot.ItemData != null) //아이템이 있는것들은 
            {
                enhanceItem = slot.ItemData as ItemData_Enhancable; //강화 되는건지 체크해서 

                if (enhanceItem != null) //강화되는거면 
                {
                    tempData.ItemEnhanceValue = enhanceItem.itemLevel; //강화된 레벨을 담아두고  0강 = 1  1강은 = 2 값
                }

                tempData.ItemIndex = slot.ItemData.code;            // 공통된부분 아이템이무엇인지 코드로 담고

                tempData.Values = slot.ItemCount;                   // 저장된 갯수도 담고
                
                tempData.SlotIndex = slot.Index;                    // 저장된 슬롯 위치도 담고
                
                tempList.Add(tempData);                             //데이터 셋팅됬으면 리스트에 추가를 해둔다
            }
        }

        saveData.EquipSlotLength = temp.Count - defaultSlotLength; // 장비창이 슬롯추가됬는지 확인하기위해 현재 갯수와 초기값을 빼서 저장해둔다

        saveData.EquipData = tempList.ToArray();                    //리스트는 jsonUtil에서 권장하지않음으로 배열로 다시 바꿔서 저장될 클래스에 입력

        tempList.Clear();                                           //리스트 내용 비우고 재사용 

        temp = slotManager.slots[Inventory_Tab.Consume];  //소비창 데이터

        SetTempData(temp,tempList);                                 //데이터 가져와서 담아두기

        saveData.ConsumeSlotLength = temp.Count - defaultSlotLength;//슬롯 갯수 변동값 저장하기 

        saveData.ConsumeData = tempList.ToArray();                  //최종 저장할데이터 배열로 변환

        tempList.Clear();                                           //리스트 내용 비우고 재사용 


        temp = slotManager.slots[Inventory_Tab.Etc];      //기타창 데이터

        SetTempData(temp,tempList);                                 //데이터 가져와서 담아두기
        
        saveData.EtcSlotLength = temp.Count - defaultSlotLength;    //슬롯 갯수 변동값 저장하기 
        
        saveData.EtcData = tempList.ToArray();                      //최종 저장할데이터 배열로 변환
        
        tempList.Clear();                                           //리스트 내용 비우고 재사용 



        temp = slotManager.slots[Inventory_Tab.Craft];    //조합 데이터
        
        SetTempData(temp,tempList);                                 //데이터 가져와서 담아두기
        
        saveData.CraftSlotLength = temp.Count - defaultSlotLength;  //슬롯 갯수 변동값 저장하기 
        
        saveData.CraftData = tempList.ToArray();                    //최종 저장할데이터 배열로 변환
        
    }

    /// <summary>
    /// 임시 리스트에 가져온데이터 담는 함수
    /// </summary>
    /// <param name="invenTabData">인벤창 슬롯에대한 정보 탭별로</param>
    /// <param name="saveData">데이터가 저장될 리스트</param>
    private void SetTempData(List<Slot> invenTabData, List<CharcterItems> saveData) 
    {
        CharcterItems tempData = new CharcterItems(); // 슬롯하나당 데이터 셋팅할 객체만들고 
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
    /// 저장파일 로드시 저장된내용 가지고 데이터 파싱작업
    /// </summary>
    /// <param name="data">저장파일에서 가져온데이터 </param>
    private void LoadInvenDataParsing(JsonGameData data) 
    {
      
        Slot temp = null; //슬롯내용물셋팅할 임시변수
        ItemData_Enhancable tempEnchan; //인첸장비인지 체크할 임시변수

        List<Slot> slots = slotManager.slots[Inventory_Tab.Equip]; //장비일경우

        for (int slotIndex = 0; slotIndex < data.EquipSlotLength; slotIndex++)
        {
            slotManager.Make_Slot(Inventory_Tab.Equip); //슬롯갯수 추가할것이있으면 추가해두고 
        }

        foreach (CharcterItems equipData in data.EquipData) //포문돌면서 데이터셋팅
        {
            temp = slots[(int)equipData.SlotIndex];
            temp.ItemData = GameManager.Itemdata[equipData.ItemIndex];
            temp.ItemCount = equipData.Values;
            if (equipData.ItemEnhanceValue > 0) //인첸내용이있으면 
            {
                tempEnchan = temp.ItemData as ItemData_Enhancable; //인첸 클래스로 변경후 
                if (tempEnchan != null)
                {
                    for (int i = 1; i < equipData.ItemEnhanceValue; i++) // 인첸무기는 기본값이 1부터 시작임으로 초기값 1로셋팅
                    {
                        tempEnchan.LevelUpItemStatus(temp); //인첸한만큼 추가로 인첸데이터셋팅

                    }
                }

            }
        }
        //장비와비슷하게 셋팅한다 밑에는 반복작업

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


    //----------------------------- 퀘스트 저장및 불러오기 ------------------------------------
    /// <summary>
    /// 저장할 퀘스트 데이터 셋팅 하는 함수
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
    /// 로드시 퀘스트 데이터 파싱관련 함수
    /// </summary>
    private void LoadQuestDataParsing(CharcterQuest[] quests)
    {
        //저장한데이터 셋팅하기
        int loadDataQuestSize = quests.Length;
        for (int i = 0; i < loadDataQuestSize; i++) //저장된 데이터만큼 돌리고 
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
    /// 퀘스트 데이터와 불러온 데이터를 비교해서 완료 퀘스트리스트 나 진행중인 퀘스트리스트에 넣는 함수
    /// </summary>
    /// <param name="questArray">원본 퀘스트</param>
    /// <param name="checkIndex">파일에서 불러온 퀘스트 인덱스값</param>
    /// <param name="setValues">파일에서 불러온 퀘스트 진행도값</param>
    private void SetQuestData(Gyu_QuestBaseData[] questArray, int checkIndex, int[] setValues, Quest_State questState)
    {
        int questSize = questArray.Length;
        Gyu_QuestBaseData tempData;
        for (int i = 0; i < questSize; i++) // 현재 퀘스트 종류 내에 존재하는 배열 을 전부 돌린다 .
        {
            tempData = questArray[i];
            if (checkIndex == tempData.QuestId)    // 퀘스트 목록중에 아이디가 같은게있는지 체크해서 
            {
                tempData.SaveFileDataPasing(setValues,questState); //데이터 셋팅하고 
                if (questState == Quest_State.Quest_Complete) //완료 된거면 
                {
                    playerQuest.ClearQuestList.Add(tempData);       //완료 퀘스트 쪽에 넣는다.
                }
                else  //완료 안된거면 
                {
                    playerQuest.CurrentQuests.Add(tempData);                // 진행중인 퀘스트쪽으로 넣고 
                }
                return;      //함수를 끝낸다. 
            }
        }

    }



    /// <summary>
    /// 게임 로드시 데이터를 초기화 시키는 함수 
    /// 초기화된 데이터에다가 로드한 데이터를 입력해야한다.
    /// </summary>
    private void ResetData()
    {
        slotManager.SaveFileLoadedResetSlots(); //기존데이터 싹다날리고 초기값으로 셋팅
        GameManager.EquipBox.ClearEquipBox();                       // 장비 초기화 
        playerQuest.ResetData();                //퀘스트 데이터 날리기
    }
    /// <summary>
    /// 데이터가 갱신은 됬으나 UI 에 연결안되는것들을 한번에 처리하는 함수 
    /// 데이터 처리 끝난뒤에 진행 
    /// </summary>
    private void RefreshData()
    {
        //퀘스트 현재진행중인 값이 연결안되서 연결시켜줘야함.
    }

}
