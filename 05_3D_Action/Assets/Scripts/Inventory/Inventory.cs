using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory //Inventory �� ���� ������ ����ϴ� Ŭ���� UI �� �ٸ�Ŭ�������� ���� �и��ؼ� �����Ѵ�
{
    public const int DefaultInventorySize = 6;
    public const uint tempSlotIndex = 98989898;// �ӽý��Կ� �ε���

    InvenSlot[] slots;
    public InvenSlot this[uint index] => slots[index]; //ȣ�� �� Inventory(this)[0];

    public int SlotCount => slots.Length;// �κ��丮 ������ ����


    InvenSlot tempSlot; //�ӽý���(�巡��, �и�, swap �� �� ���)
    public InvenSlot TempSlot => tempSlot;

    ItemDataManager itemdataManager; //������ �����͸� �����ִ� Ŭ���� 

    Player owner;//�κ��丮 ������
    public Player Owner => owner;


    public Inventory(Player owner, uint size = DefaultInventorySize)
    {
        slots = new InvenSlot[size];
        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InvenSlot(i);// ���� ���� ����
        }
        tempSlot = new InvenSlot(tempSlotIndex);
        itemdataManager = GameManager.Inst.ItemData;//������ ������ �Ŵ��� ĳ��
        this.owner = owner;                         // ������ ���
    }
    //�κ��丮�� ������ �߰��ϴ� �Լ� 

    public bool AddItem(ItemCode code)//�������� �ϳ� �߰��ϴ� �Լ� 
    {
        bool result = false;
        ItemData data = itemdataManager[code];

        InvenSlot sameDataSlot = FindSameItem(data);
        if (sameDataSlot != null)
        {
            //���������� �������� �ִ�.
            result = sameDataSlot.IncreaseSlotItem(out uint _);//��ġ�� ���� �ǹ̾�� ���� ���� ����
        }
        else
        {
            InvenSlot emptySlot = FindEnptySlot();
            if (emptySlot != null)
            {
                emptySlot.AssignSlotItem(data); //�󽽷��� ������ �Ҵ�
                result = true;
            }
            else
            {
                // �� ������ ����.
                Debug.Log("������ �߰� ���� , �κ��丮�� ������ �ֽ��ϴ�.");
            }
        }

       
        return result;
    }
    public bool AddItem(ItemCode code, uint slotIndex)// �κ��丮�� Ư�����Կ� �߰��ϴ� �Լ� 
    {
        bool result = false;
        if (IsValidIndex(slotIndex))
        {
            ItemData data = itemdataManager[code];
            InvenSlot targetSlot = slots[slotIndex];
            if (targetSlot.isEmpty)
            {
                targetSlot.AssignSlotItem(data);
            }
            else
            {
                if (targetSlot.ItemData == data)
                {
                    result = targetSlot.IncreaseSlotItem(out _);
                }
                else
                {
                    Debug.Log($"�ش� ���Կ� �������� �߰��� �� �����ϴ�.{slotIndex} ��° �ٸ� �������� ����ֽ��ϴ�.");
                }
            }
        }
        else
        {
            Debug.Log("�ش� ���Կ� �������� �߰��� �� �����ϴ�.");
        }
        return result;
    }
    public void MoveItem(uint from, uint to)//�������� from index���� to index�� �ű��.
    {
        if ((from != to) &&IsValidIndex(from) && IsValidIndex(to))//from�� to�� �ٸ��鼭 ��� ��ȿ�� �ε����϶�
        {
            InvenSlot fromSlot = (from == tempSlotIndex)? tempSlot : slots[from]; //�ӽ�����뽽�� �����ؼ� ���׿����� ó��
            if (!fromSlot.isEmpty)
            {
                InvenSlot toSlot = (to == tempSlotIndex) ? tempSlot : slots[to];
                if (fromSlot.ItemData == toSlot.ItemData)//���������� �������̸�
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);//
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);//��ģ��ŭ fromSlot�� ���� ����
                    Debug.Log($"{from} ���Կ��� {to}�������� ������ ��ħ");
                }
                else
                {
                    ItemData itemData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
                    toSlot.AssignSlotItem(itemData, tempCount);
                    Debug.Log($"{from}�� ���԰� {to}�� ������ ������ ��ü");
                }
            }
        }
    }
    public void SplitItem(uint slotIndex, uint count)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            tempSlot.AssignSlotItem(slot.ItemData, count);
            slot.DecreaseSlotItem(count);
        }
    }
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)//isAcending = true�� ��������
    {
        List<InvenSlot> beforeSlots = new List<InvenSlot>(slots);// slots�迭�� �̿��ؼ� ����Ʈ �����
        //foreach(InvenSlot slot in slots)
        //{
        //    beforeSlots.Add(slot);
        //}
        switch (sortBy)// sort �Լ����Ķ���ͷ� �� �����Լ��� �ٸ��� �ۼ�
        {
            case ItemSortBy.Code:
                beforeSlots.Sort((x, y) => // x, y�� ���� ���� 2�� beforeslots �� ����ִ� 2��
                {
                    if (x.ItemData == null) // itemData�� ������� �� ������ ��������� ����ִ°��� �������� ����
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);// ���������϶� CompareTo �Լ��� �� 
                    }
                    else
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);
                    }
                });
                break;
            case ItemSortBy.Name:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.itemName.CompareTo(y.ItemData.itemName);
                    }
                    else
                    {
                        return y.ItemData.itemName.CompareTo(x.ItemData.itemName);
                    }
                });
                break;
            case ItemSortBy.Price:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.price.CompareTo(y.ItemData.price);
                    }
                    else
                    {
                        return y.ItemData.price.CompareTo(x.ItemData.price);
                    }
                });
                break;
        }
        //beforeSlots�� �� �������� ������ ���ؿ� ���� ���� �Ϸ�

        //List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(SlotCount);//������ ������ ������ ���� �����ϱ�
        //foreach(var slot in beforeSlots)
        //{
        //    sortedData.Add((slot.ItemData, slot.ItemCount));
        //}

        ////���Կ� ������ ������ ������ ������� �Ҵ��ϱ�
        //int index = 0;
        //foreach(var data in sortedData)
        //{
        //    slots[index].AssignSlotItem(data.Item1, data.Item2);
        //}
        slots = beforeSlots.ToArray();//�� �ڵ�� �������� �ֱ�
        RefreshInventory();
    }

    void RefreshInventory()//��罽���� ����Ǿ����� �˸��� �Լ� 
    {
        foreach (var slot in slots)
        {
            slot.onSlotItemChange?.Invoke();
        }
    }
    public void RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
        }
        else
        {
            Debug.Log($"���� ���� : {slotIndex} �� ���� �ε��� �Դϴ�.");
        }
    }
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
        }
        else
        {
            Debug.Log($"���� ���� : {slotIndex} �� ���� �ε��� �Դϴ�.");
        }
    }
    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }
    //�κ��丮�� �������� ����������ŭ �����ϴ� �Լ� 
    //������ �����ϴ� �Լ� 
    //�κ��丮�� ���� ���� �Լ�
    //�κ��丮�� Ư����ġ���� �ٸ� ��ġ�� �̵���Ű�� �Լ�
    //�κ��丮���� �������� ������ ����� �ӽý������� ������ �Լ� 
    //�κ��丮�� �����ϴ� �Լ� 
    InvenSlot FindSameItem(ItemData data)//�κ��丮�� �Ķ���Ϳ� ���� ������ �������� ����ִ� ������ ã�� �Լ� 
    {
        InvenSlot findSlot = null;
        foreach (InvenSlot slot in slots)
        {
            if (slot.ItemData == data && slot.ItemCount < slot.ItemData.maxStackCount)
            {
                findSlot = slot;
                break;
            }
        }
   
        return findSlot;
    }
    InvenSlot FindEnptySlot()//����ִ� ù��° ����
    {
        InvenSlot invenSlot = null;
        foreach (InvenSlot slot in slots)
        {
            if (slot.isEmpty)
            {
                invenSlot = slot;
                break;
            }
        }
        return invenSlot;
    }
    bool IsValidIndex(uint index) => (index < SlotCount) || (index == tempSlotIndex); //�� ���� �� �ϳ��� ������ �ؾ� valid�� �ε���

    public void PrintInventory()
    {
        string printText = "";
        for (int i = 0; i < SlotCount - 1; i++)
        {
            if (!slots[i].isEmpty)
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount})";
            }
            else
            {
                printText += "(��ĭ)";
            }
            printText += ",  ";
        }

        InvenSlot last = slots[slots.Length - 1];
        if (!last.isEmpty)
        {
            printText += $"{last.ItemData.itemName}({last.ItemCount}/{last.ItemData.maxStackCount})";
        }
        else
        {
            printText += "(��ĭ)";
        }
        Debug.Log(printText);
    }
}
