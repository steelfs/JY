using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    uint slotIndex;
    public uint Index => slotIndex;
    uint itemCount = 0;
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if (itemCount != value)
            {
                itemCount = value;
                onSlotItemChange?.Invoke(); //UI ½ÅÈ£
            }
        }
    }
    ItemData slotItemData = null;
    public ItemData ItemData
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();
            }          
        }
    }
    public Action onSlotItemChange;

    public bool isEmpty => slotItemData == null;

    bool isEquipped = false;
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;
            onSlotItemChange?.Invoke();
        }
    }
    public InvenSlot(uint index)
    {
        slotIndex = index;
        ItemCount = 0;
        IsEquipped = false;
    }
    public void AssignSlotItem(ItemData itemData, uint count = 1)
    {

    }
    public void ClearSlotItem()
    {
       
     
    }
    public bool IncreaseSlotItem(out int overCount, uint increaseCount = 1)
    {
        bool result;
        result = false;
        overCount = 1;

        return result;
    }
    public void DecreaseSlotItem(uint decreaseCount = 1)
    {

    }
    public void UseItem(GameObject target)
    {

    }
    public void EquipItem(GameObject target)
    {

    }
}
