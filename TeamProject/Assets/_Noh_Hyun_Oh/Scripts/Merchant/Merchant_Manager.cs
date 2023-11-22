using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using static QuestReward;

/// <summary>
/// 상점 상태 제어용 이넘값
/// </summary>
public enum Merchant_Selected
{
    Buy,
    Sell,

}
public class Merchant_Manager : MonoBehaviour
{
  
    int defaultBuyCount = 1;

    /// <summary>
    /// 마지막에 창을 열고있던 NPC 인덱스
    /// </summary>
    int currentNpcIndex = 0;
    public int CurrentNpcIndex => currentNpcIndex;


    /// <summary>
    /// 엔피씨 대화목록 관리하는 클래스 
    /// </summary>
    TalkData_Gyu talkData;
    public TalkData_Gyu TalkData => talkData;


    /// <summary>
    /// 구입인지 판매인지 셋팅 값
    /// </summary>
    Merchant_Selected selected;
    public Merchant_Selected Selected
    {
        get => selected;
        set
        {
            selected = value;
        }
    }

    Inventory_Tab merchant_State;
    public Inventory_Tab Merchant_State 
    {
        get => merchant_State;
        set 
        {
            merchant_State = value;
        }
    }


    /// <summary>
    /// 상인이 판매중인 아이템목록 
    /// </summary>
    [SerializeField]
    ItemData[] merchantItemArray;

    /// <summary>
    /// 장비판매목록
    /// </summary>
    List<ItemData> equipItems;
    public List<ItemData> EquipItems => equipItems;
    /// <summary>
    /// 장비판매목록의 갯수
    /// </summary>
    int[] equipItemsCount; 
    public int[] EquipItemsCount => equipItemsCount;

    /// <summary>
    /// 소비판매목록
    /// </summary>
    List<ItemData> consumeItems;
    public List<ItemData> ConsumeItems => consumeItems;
    /// <summary>
    /// 소비 판매목록의 갯수
    /// </summary>
    int[] consumeItemsCount;
    public int[] ConsumeItemsCount => consumeItemsCount;

    /// <summary>
    /// 기타 판매목록
    /// </summary>
    List<ItemData> etcItems;
    public List<ItemData> EtcItems => etcItems;
    /// <summary>
    /// 기타 판매목록의 갯수
    /// </summary>
    int[] etcItemsCount;
    public int[] EtcItemsCount => etcItemsCount;

    /// <summary>
    /// 제작 판매목록 
    /// </summary>
    List<ItemData> craftItems;
    public List<ItemData> CraftItems => craftItems;
    /// <summary>
    /// 제작판매목록의 갯수 
    /// </summary>
    int[] craftItemsCount;
    public int[] CraftItemsCount => craftItemsCount;


    /// <summary>
    /// 상인이 해당물품을 얼마나 싸게 혹은 비싸게 셋팅할지에대한 최종값을 저장할 배열
    /// merchantItemArray 와 인덱스가 같게 셋팅할 예정 
    /// </summary>
    //int[] merchantItemCoinValue;
    //public int[] MerchantItemCoinValue => merchantItemCoinValue;


    /// <summary>
    /// 상인이 구입한 아이템 목록 
    /// 잘못판매한거 다시사는용도로 사용 
    /// </summary>
    List<ItemData> sellsItemArray;
    public List<ItemData> SellsItemArray => sellsItemArray;


    /// <summary>
    /// 재구입 용 아이템 갯수 
    /// sellsItemArray 와 인덱스를 같게 셋팅할 예정이다.
    /// </summary>
    int[] merchantSellCountArray;
    public int[] MerchantSellCountArray => merchantSellCountArray;

    /// <summary>
    /// 대화 로직
    /// </summary>
    [SerializeField]
    NpcTalkController talkController;
    public NpcTalkController NpcTalkController => talkController;

    MerchantNPC[] merchantNPCs;
    

    Merchant_UI_Manager merchant_UI_Manager;
    public Merchant_UI_Manager Merchant_UI_Manager => merchant_UI_Manager;

    InteractionUI actionUI;
    public InteractionUI ActionUI => actionUI;
    /// <summary>
    /// 기능활성화 여부
    /// </summary>
    bool isActionActive = false;
    public bool IsActionActive => isActionActive;
    private void Awake()
    {
        int capacity = (int)(merchantItemArray.Length * 0.25f);

        merchant_UI_Manager = GetComponent<Merchant_UI_Manager>();
        talkController = FindObjectOfType<NpcTalkController>();
        equipItems = new List<ItemData>(capacity);
        consumeItems = new List<ItemData>(capacity);
        etcItems = new List<ItemData>(capacity);
        craftItems = new List<ItemData>(capacity);


        foreach (ItemData item in merchantItemArray)
        {
            switch (item.ItemType)
            {
                case ItemType.Equip:
                    equipItems.Add(item);
                    break;
                case ItemType.Consume:
                    consumeItems.Add(item);
                    break;
                case ItemType.Etc:
                    etcItems.Add(item);
                    break;
                case ItemType.Craft:
                    craftItems.Add(item);
                    break;
            }
        }
        equipItemsCount = new int[equipItems.Count];
        int forSize = equipItemsCount.Length;
        for (int i = 0; i < forSize; i++)
        {
            equipItemsCount[i] = defaultBuyCount;
        }
        consumeItemsCount = new int[consumeItems.Count];
        forSize = consumeItemsCount.Length;
        int consumeSize = defaultBuyCount * 100;
        for (int i = 0; i < forSize; i++)
        {
            consumeItemsCount[i] = consumeSize;
        }

        etcItemsCount= new int[etcItems.Count];
        forSize = etcItemsCount.Length;
        for (int i = 0; i < forSize; i++)
        {
            etcItemsCount[i] = defaultBuyCount;
        }

        craftItemsCount = new int[craftItems.Count];
        forSize = craftItemsCount.Length;
        for (int i = 0; i < forSize; i++)
        {
            craftItemsCount[i] = defaultBuyCount;
        }

    }

    private void Start()
    {
        merchantNPCs = FindObjectsOfType<MerchantNPC>(true);
        InputSystemController.InputSystem.Player.Action.performed += (_) => {
            if (isActionActive)
            {
                talkController.Talk(0);     //대화창열기
                actionUI.invisibleUI?.Invoke();   //F키 닫기
            }
        };
        Selected = Merchant_Selected.Buy;
        Merchant_State = Inventory_Tab.Equip;
        merchant_UI_Manager.ReFresh_Merchant_Item();
    }

    /// <summary>
    /// 아이템이 판매및 취소 선택이 됬을때 처리되는 함수
    /// </summary>
    /// <param name="item">처리할 아이템</param>
    /// <param name="itemCount">처리될 갯수</param>
    public void ItemClick(ItemData item, uint itemCount,Slot slot = null) 
    {
        if (selected == Merchant_Selected.Buy)
        {
            if (GameManager.PlayerStatus.Base_Status.Base_DarkForce > item.price*0.1*itemCount) //구입가능하면  
            {
                for (int i = 0; i < itemCount; i++)
                {
                    if (GameManager.SlotManager.AddItem(item.code)) //추가가능하면 
                    {
                        GameManager.PlayerStatus.Base_Status.Base_DarkForce -= (uint)(item.price * 0.1f); //금액깍는다
                    }
                }
                return;
            }
            Debug.Log($"보유금액{GameManager.PlayerStatus.Base_Status.Base_DarkForce} 필요 금액 : {item.price * 0.1 * itemCount}");
        } 
        else if (selected == Merchant_Selected.Sell) 
        {
            if (slot != null)
            {
                GameManager.SlotManager.RemoveItem(slot.ItemData, slot.Index, itemCount);
                merchant_UI_Manager.ReFresh_Merchant_Item();
                GameManager.PlayerStatus.Base_Status.Base_DarkForce += (uint)(item.price * 0.1f * itemCount);
                return;
            }
            Debug.Log("슬롯이 왜없냐? 파는데 ");
        }
    }



    /// <summary>
    /// 함선시작시 시작할 초기화함수
    /// </summary>
    public void InitDataSetting()
    {

        actionUI = FindObjectOfType<InteractionUI>(true);
        // 팩토리로 할시 엔피씨 위치를 몇개 후보지역두고 랜덤으로 변경시키는게 더간단할거같다.
        // 초기화 하는것은 여기말고 다른곳으로 빼서 해야될거같다 .. 팩토리 로 생성시킨뒤에 껏다켯다하면 될거같긴한데.. 
        MerchantNPC[] array_NPC = FindObjectsOfType<MerchantNPC>(true);   //씬에있는 엔피씨 찾아서 담아두고 ( 찾는 순서가 바뀔수도있으니 다른방법을 찾아보자.)
        for (int i = 0; i < array_NPC.Length; i++)
        {
            //위치와 모양을 변경시키면 될거같기도한데.. 일단 고민좀해보자..
            array_NPC[i].InitData(i); //npc 를 초기화 시킨다.
            array_NPC[i].onTalkDisableButton += NpcTalkDisable;
            array_NPC[i].onTalkEnableButton += (npcId) =>
            {
                NpcTalkActive(npcId);
                talkController.onTalkClick = () => array_NPC[currentNpcIndex];
                talkController.getTalkDataArray = (talkIndex) =>
                {
                    return talkController.TalkData.GetTalk(array_NPC[currentNpcIndex].TalkType, talkIndex);
                };
                talkController.LogManager.getLogTalkDataArray = (talkIndex) => {
                    return talkController.TalkData.GetLog(array_NPC[currentNpcIndex].TalkType, talkIndex);
                };
            };

        }


    }

    /// <summary>
    /// NPC 근처에 갔을때 대화 할수있는상태로 만들기위한 사전작업
    /// </summary>
    private void NpcTalkDisable()
    {
        talkController.ResetData();
        talkController.openTalkWindow = null;
        talkController.closeTalkWindow = null;
        talkController.onTalkClick = null;
        talkController.getTalkDataArray = null;
        talkController.LogManager.getLogTalkDataArray = null;
        actionUI.invisibleUI?.Invoke();
        talkController.IsTalking = true;
        isActionActive = false;
    }

    /// <summary>
    /// NPC 근처에서 벗어났을때 대화할수 없는상태로 만들기위한 작업
    /// </summary>
    /// <param name="npcId"></param>
    private void NpcTalkActive(int npcId)
    {
        talkController.ResetData();
        talkController.openTalkWindow = merchant_UI_Manager.OpenWindow;
        talkController.closeTalkWindow = merchant_UI_Manager.CloseWindow;
        currentNpcIndex = npcId;
        actionUI.visibleUI?.Invoke();
        talkController.IsTalking = false;
        isActionActive = true;
    }

}
