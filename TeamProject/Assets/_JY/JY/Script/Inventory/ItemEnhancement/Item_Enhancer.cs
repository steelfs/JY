using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum EnhancerState// 강화도중 창 닫기 비활성화
{
    Open,
    SetItem,
    Confirm,
    WaitforResult,
    Success,
    Fail,
    ClearItem,
    Close
}
public class Item_Enhancer : MonoBehaviour
{
    Item_Enhancer_UI item_Enhancer_UI;

    public Item_Enhancer_UI EnhancerUI => item_Enhancer_UI;

    public Action onOpen;
    public Action<ItemData_Enhancable> onSetItem;
    public Action onWaitforResult;
    public Action onSuccess;
    public Action onFail;
    public Action onClearItem;
    public Action onClose;
    public Action onConfirmButtonClick;


    ItemData_Enhancable itemData;


    public ItemData_Enhancable ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                //ClearState 대리자 
                if (itemData != null)
                {
                    EnhancerState = EnhancerState.SetItem;
                    EnhancerUI.onTriggerLevelUp = itemData.LevelUpItemStatus;
                }
                else
                {
                    EnhancerState = EnhancerState.ClearItem;
                    EnhancerUI.onTriggerLevelUp = null;
                }
            }
        }
    }
     EnhancerState enhancerState;
    public  EnhancerState EnhancerState
    {
        get => enhancerState;
        set
        {
            if (enhancerState != value)
            {
                enhancerState = value;
            }
            switch (enhancerState)
            {
                case EnhancerState.Open:
                    onOpen?.Invoke();
                    break;
                case EnhancerState.SetItem:
                    onSetItem?.Invoke(ItemData);
                    break;
                case EnhancerState.Confirm:
                    if (ItemData != null)
                    {
                        onConfirmButtonClick?.Invoke();
                    }
                    break;
                case EnhancerState.WaitforResult:
                    if (itemData != null)
                    onWaitforResult?.Invoke();
                    break;
                case EnhancerState.Success:
                    onSuccess?.Invoke(); // inventory에 Itemdata 리턴하고 EnhancerUI Clear
                    break;
                case EnhancerState.Fail:
                    onFail?.Invoke(); // inventory에 Itemdata 리턴하고 EnhancerUI Clear
                    break;
                case EnhancerState.ClearItem:
                    onClearItem?.Invoke();
                    break;
                case EnhancerState.Close:
                    onClose?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        item_Enhancer_UI = GetComponent<Item_Enhancer_UI>();
        EnhancerState = EnhancerState.Close;
    }
 


}
