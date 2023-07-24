using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    uint slotIndex; //�κ��丮������ �ε���
    public uint Index => slotIndex;
    uint itemCount = 0;//���� ���Կ� ����ִ� �������� ���� 
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if (itemCount != value)//������� ����
            {
                itemCount = value;
                onSlotItemChange?.Invoke(); //UI ��ȣ
            }
        }
    }
    ItemData slotItemData = null;
    public ItemData ItemData// �� ���Կ� ����ִ� �������� ������ Ȯ���ϱ����� ������Ƽ
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)// �������� ����� ����
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();//�˸�
            }          
        }
    }
    public Action onSlotItemChange;//����������, ����, ��񿩺ΰ� ���ϸ� ����Ǵ� delegate

    public bool isEmpty => slotItemData == null;// �󽽷����� Ȯ���ϴ� ������Ƽ

    bool isEquipped = false;//�� ������ �������� ���Ǿ����� ����
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;//IsEquip �Ǽ��� ����� stackOverFlow
            onSlotItemChange?.Invoke();
        }
    }
    public InvenSlot(uint index)
    {
        slotIndex = index;// slotIndex�� ���ķ� ���� ���ϸ� �ȵȴ�.
        ItemCount = 0;
        IsEquipped = false;
        ItemData = null;
    }
    public void AssignSlotItem(ItemData Data, uint count = 1)//�� ���Կ� �������� �����ϴ� �Լ� , param = ������ ����������, ����
    {
        if (Data != null)
        {
            ItemData = Data;
            ItemCount = count;
            IsEquipped = false;
            Debug.Log($"�κ��丮 {slotIndex} ���Կ� \"{ItemData.itemName}\" �������� {itemCount} �� ����");
        }
        else
        {
            ClearSlotItem(); //data�� null�̸� �ش罽���� �ʱ�ȭ
        }
    }
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
        Debug.Log($"�κ��丮 {slotIndex} ��° ������ ���ϴ�.");
     
    }
    public bool IncreaseSlotItem(out  uint overCount, uint increaseCount = 1)//���Կ� ������ ������ �߰��ϴ� �Լ�  overCount = ��¿� // true�� ����, false�� ���ƴ�.
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
            Debug.Log($"�κ��丮 {slotIndex} ���� �󽽷Կ� \"{ItemData.itemName}\" �� �ִ�ġ���� ���� ����  {ItemCount} �� {over} �� ��ħ");
        }
        else
        {
            ItemCount = newCount;
            overCount = 0;
            result = true;
            Debug.Log($"�κ��丮 {slotIndex} ���� �󽽷Կ� \"{ItemData.itemName}\" �� {increaseCount} ��ŭ ���� ����  {ItemCount} ��");
        }

        return result;
    }
    public void DecreaseSlotItem(uint decreaseCount = 1)//���� ����
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount < 1)//������ ������ ��°�� 
        {
            ClearSlotItem();
        }
        else//�������� �����ִ� ��� 
        {
            ItemCount = (uint)newCount;
            Debug.Log($"�κ��丮 {slotIndex} ��° ���Կ� \"{ItemData.itemName}\" �������� {decreaseCount} �� ��ŭ ���� ���� {ItemCount} ��");
        }
    }
 
    public void UseItem(GameObject target)
    {

    }
    public void EquipItem(GameObject target)
    {

    }
}
