using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    uint slotIndex; //인벤토리에서의 인덱스
    public uint Index => slotIndex;
    uint itemCount = 0;//현재 슬롯에 들어있는 아이템의 개수 
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if (itemCount != value)//변경됐을 때만
            {
                itemCount = value;
                onSlotItemChange?.Invoke(); //UI 신호
            }
        }
    }
    ItemData slotItemData = null;
    public ItemData ItemData// 이 슬롯에 들어있는 아이템의 종류를 확인하기위한 프로퍼티
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)// 아이템이 변경될 때만
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();//알림
            }          
        }
    }
    public Action onSlotItemChange;//아이템종류, 개수, 장비여부가 변하면 실행되는 delegate

    public bool isEmpty => slotItemData == null;// 빈슬롯인지 확인하는 프로퍼티

    bool isEquipped = false;//이 슬롯의 아이템이 장비되었는지 여부
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;//IsEquip 실수로 변경시 stackOverFlow
            onSlotItemChange?.Invoke();
        }
    }
    public InvenSlot(uint index)
    {
        slotIndex = index;// slotIndex는 이후로 절대 변하면 안된다.
        ItemCount = 0;
        IsEquipped = false;
        ItemData = null;
    }
    public void AssignSlotItem(ItemData Data, uint count = 1)//이 슬롯에 아이템을 설정하는 함수 , param = 설정할 아이템종류, 개수
    {
        if (Data != null)
        {
            ItemData = Data;
            ItemCount = count;
            IsEquipped = false;
            Debug.Log($"인벤토리 {slotIndex} 슬롯에 \"{ItemData.itemName}\" 아이템이 {itemCount} 개 설정");
        }
        else
        {
            ClearSlotItem(); //data가 null이면 해당슬롯은 초기화
        }
    }
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
        Debug.Log($"인벤토리 {slotIndex} 번째 슬롯을 비웁니다.");
     
    }
    public bool IncreaseSlotItem(out  uint overCount, uint increaseCount = 1)//슬롯에 아이템 개수를 추가하는 함수  overCount = 출력용 // true면 성공, false면 넘쳤다.
    {
        bool result;
        int over;
        uint newCount = ItemCount + increaseCount;
        over = (int)newCount - (int)ItemData.maxStackCount;

        if (over > 0)
        {
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            Debug.Log($"인벤토리 {slotIndex} 번재 빈슬롯에 \"{ItemData.itemName}\" 이 최대치까지 증가 현재  {ItemCount} 개 {over} 개 넘침");
        }
        else
        {
            ItemCount = newCount;
            overCount = 0;
            result = true;
            Debug.Log($"인벤토리 {slotIndex} 번재 빈슬롯에 \"{ItemData.itemName}\" 이 {increaseCount} 만큼 증가 현재  {ItemCount} 개");
        }

        return result;
    }
    public void DecreaseSlotItem(uint decreaseCount = 1)//개수 감소
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount < 1)//슬롯이 완전히 비는경우 
        {
            ClearSlotItem();
        }
        else//아이템이 남아있는 경우 
        {
            ItemCount = (uint)newCount;
            Debug.Log($"인벤토리 {slotIndex} 번째 슬롯에 \"{ItemData.itemName}\" 아이템이 {decreaseCount} 개 만큼 감소 현재 {ItemCount} 개");
        }
    }
 
    public void UseItem(GameObject target)
    {

    }
    public void EquipItem(GameObject target)
    {

    }
}
