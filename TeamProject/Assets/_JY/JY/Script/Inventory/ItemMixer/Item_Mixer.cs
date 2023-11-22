using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

 public enum ItemMixerState// 강화도중 창 닫기 비활성화
{
    Open,
    SetItem,
    SetItemCanceled,
    Confirm,
    WaitforResult,
    Success,
    Fail,
    ClearItem,
    Close
}
public class Item_Mixer : MonoBehaviour
{
    Mixer_Anim mixer_Anim;
    public Mixer_Anim Mixer_Anim => mixer_Anim;
    Item_Mixing_Table mixing_table;
    public Item_Mixing_Table Mixing_Table => mixing_table;

    Item_Mixer_UI item_Mixer_UI;
    SlotManager slot_Manager;
    public Item_Mixer_UI MixerUI => item_Mixer_UI;

    public Action onOpen;
    public Action<ItemData> onSetItem;
    public Action onSetItemCanceled;
    public Action onWaitforResult;
    public Action<bool> onSuccess;
    public Action onFail;
    public Action onClearItem;
    public Action onClose;
    public Action onConfirmButtonClick;

    Mixer_Slot_Left left_Slot;
    Mixer_Slot_Middle middle_Slot;
    Mixer_Slot_Result result_Slot;
    public Mixer_Slot_Result ResultSlot => result_Slot;

    ItemData leftSlotData = null;
    ItemData middleSlotData = null;
    ItemData tempData = null;// 아이템 제거하기 전 slot에 추가할 아이템을 미리 복사해놓는 용도

    public bool IsCritical { get; set; }
    bool return_To_Inventory = true;
    public bool Return_To_Inventory
    {
        get => return_To_Inventory;
        set
        {
            return_To_Inventory = value;
        }
    }
    
    public Action<ItemData> onLeftSlotDataSet;
    public Action<ItemData> onMiddleSlotDataSet;

    //일단 먼저 할당을 한 후 비교해서 다르면 삭제해야함. 추가할 때부터 비교를해서 하려면 초기에 아이템이 추가가 안되는 문제가 있음
    public ItemData LeftSlotData
    {
        get => leftSlotData;
        set
        {
            if (leftSlotData != value)
            {
                tempData = leftSlotData;
                leftSlotData = value;
                CheckBothSlot();
                onLeftSlotDataSet?.Invoke(leftSlotData);// 슬롯메니저에서 이 델리게이트를 받아서 아이템의 카운트를 더하고 빼줘야한다.null이면 더하고  아니면 빼주고
                if (leftSlotData != null)
                {
                    StartCoroutine(LeftSlotDelay());
                }
                else
                {
                    if (return_To_Inventory)
                    slot_Manager.AddItem(tempData.code);
                }
            }
        }
    }
    IEnumerator LeftSlotDelay()
    {
        yield return new WaitForSeconds(0.01f);
        slot_Manager.RemoveItem(leftSlotData, slot_Manager.Index_JustChange_Slot);
    }
    IEnumerator MiddleSlotDelay()
    {
        yield return new WaitForSeconds(0.01f);
        slot_Manager.RemoveItem(middleSlotData, slot_Manager.Index_JustChange_Slot);
    }
    public ItemData MiddleSlotData
    {
        get => middleSlotData;
        set
        {
            if (middleSlotData != value)
            {
                tempData = middleSlotData;// 데이터가 null 이 되면 인벤토리 슬롯에 아이템을 추가해야하기때문에 null이 되기 전 임시저장

                middleSlotData = value;
                CheckBothSlot();
                onMiddleSlotDataSet?.Invoke(middleSlotData);
                if (middleSlotData != null)
                {
                    StartCoroutine(MiddleSlotDelay());
                }
                else
                {
                    if (return_To_Inventory)
                    slot_Manager.AddItem(tempData.code);
                }
            }
        }
    }
    void CheckBothSlot()
    {
        if (leftSlotData != null && middleSlotData != null)//두 슬롯 모두 셋팅 되었다면 
        {// 조합목록에 있는지 확인 하는 조건 추가해야함
            MixerState = ItemMixerState.SetItem;
        }
        else
        {
            MixerState = ItemMixerState.SetItemCanceled;
        }
    }
    //bool CompareSlotData()
    //{
    //    bool result = false;
    //    if (leftSlotData.ItemType)
    //    return result;
    //}
    ItemMixerState mixerState;
    public ItemMixerState MixerState
    {
        get => mixerState;
        set
        {
            if (mixerState != value)
            {
                mixerState = value;
            }
            switch (mixerState)
            {
                case ItemMixerState.Open:
                    onOpen?.Invoke();
                    break;
                case ItemMixerState.SetItem:
                    SetResultSlot();
                    onSetItem?.Invoke(result_Slot.ItemData);
                    break;
                case ItemMixerState.SetItemCanceled:
                    result_Slot.ItemData = null;
                    onSetItemCanceled?.Invoke();
                    break;
                case ItemMixerState.Confirm:
                    if (result_Slot.ItemData != null)//아이템이 셋팅된 경우만 팝업
                    onConfirmButtonClick?.Invoke();
                    break;
                case ItemMixerState.WaitforResult:
                    onWaitforResult?.Invoke();
                    break;
                case ItemMixerState.Success:
                    onSuccess?.Invoke(IsCritical); // inventory에 Itemdata 리턴하고 EnhancerUI Clear
                    break;
                case ItemMixerState.Fail:
                    onFail?.Invoke(); // inventory에 Itemdata 리턴하고 EnhancerUI Clear
                    break;
                case ItemMixerState.ClearItem:
                    onClearItem?.Invoke();
                    break;
                case ItemMixerState.Close:
                    onClose?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        item_Mixer_UI = GetComponent<Item_Mixer_UI>();
        MixerState = ItemMixerState.Close;
        left_Slot = GetComponentInChildren<Mixer_Slot_Left>();
        middle_Slot = GetComponentInChildren<Mixer_Slot_Middle>();
        result_Slot = GetComponentInChildren<Mixer_Slot_Result>();
        mixer_Anim = FindObjectOfType<Mixer_Anim>();

        left_Slot.onClearLeftSlot += LeftSlot_Canceled;
        middle_Slot.onClearMiddleSlot += MiddleSlot_Canceled;
    }
    private void Start()
    {
        slot_Manager = GameManager.SlotManager;
        mixing_table = GameManager.Mixing_Table;
    }
    void LeftSlot_Canceled()//조합 실패할 경우가 아니라 그냥 클릭해서 취소한 경우
    {
        Return_To_Inventory = true;
        LeftSlotData = null;
    }
    void MiddleSlot_Canceled()
    {
        Return_To_Inventory = true;
        MiddleSlotData = null;
    }
    void SetResultSlot()
    {
        if (mixing_table.ValidData(leftSlotData.code, middleSlotData.code, out ItemCode resultCode))
        {
            result_Slot.ItemData = GameManager.Itemdata[resultCode];
        }
        else
        {
            Debug.Log("불가능한 조합입니다.");
        }
    }
}
