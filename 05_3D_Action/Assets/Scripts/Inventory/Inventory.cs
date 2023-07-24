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
        ItemData data = itemdataManager[code];

        InvenSlot sameDataSlot = FindSameItem(data);
        if (sameDataSlot != null)
        {
            //���������� �������� �ִ�.
        }
        else
        {
            //���������� �������� ����.
        }

        bool result = false;
        return result;
    }
    public bool AddItem(ItemCode code, uint slotIndex)// �κ��丮�� Ư�����Կ� �߰��ϴ� �Լ� 
    {
        bool result = false;
        return result;
    }
    //�κ��丮�� �������� ����������ŭ �����ϴ� �Լ� 
    //������ �����ϴ� �Լ� 
    //�κ��丮�� ���� ���� �Լ�
    //�κ��丮�� Ư����ġ���� �ٸ� ��ġ�� �̵���Ű�� �Լ�
    //�κ��丮���� �������� ������ ����� �ӽý������� ������ �Լ� 
    //�κ��丮�� �����ϴ� �Լ� 
    InvenSlot FindSameItem(ItemData data)//�κ��丮�� �Ķ���Ϳ� ���� ������ �������� ����ִ� ������ ã�� �Լ� 
    {
        InvenSlot _slot = null;
        foreach(InvenSlot slot in slots)
        {
            if (slot.ItemData == data)
            {
                slot.IncreaseSlotItem(out uint overCount, 1);
                _slot = slot;
            }
        }
        foreach(InvenSlot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.AssignSlotItem(data, 1);
                _slot = slot;
            }
        }
        return _slot;
    }
}
