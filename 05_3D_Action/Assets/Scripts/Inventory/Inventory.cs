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
    
}
