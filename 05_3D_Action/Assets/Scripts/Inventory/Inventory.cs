using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory //Inventory 의 내부 구현을 담당하는 클래스 UI 는 다른클래스에서 따로 분리해서 관리한다
{
    public const int DefaultInventorySize = 6;
    public const uint tempSlotIndex = 98989898;// 임시슬롯용 인덱스

    InvenSlot[] slots;
    public InvenSlot this[uint index] => slots[index]; //호출 예 Inventory(this)[0];

    public int SlotCount => slots.Length;// 인벤토리 슬롯의 개수


    InvenSlot tempSlot; //임시슬롯(드래그, 분리, swap 할 때 사용)
    public InvenSlot TempSlot => tempSlot;

    ItemDataManager itemdataManager; //종류별 데이터를 갖고있는 클래스 

    Player owner;//인벤토리 소유자
    public Player Owner => owner;


    public Inventory(Player owner, uint size = DefaultInventorySize)
    {
        slots = new InvenSlot[size];
        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InvenSlot(i);// 슬롯 만들어서 저장
        }
        tempSlot = new InvenSlot(tempSlotIndex);
        itemdataManager = GameManager.Inst.ItemData;//아이템 데이터 매니저 캐싱
        this.owner = owner;                         // 소유자 기록
    }
    //인벤토리에 아이템 추가하는 함수 

    public bool AddItem(ItemCode code)//아이템을 하나 추가하는 함수 
    {
        ItemData data = itemdataManager[code];

        InvenSlot sameDataSlot = FindSameItem(data);
        if (sameDataSlot != null)
        {
            //같은종류의 아이템이 있다.
        }
        else
        {
            //같은종류의 아이템이 없다.
        }

        bool result = false;
        return result;
    }
    public bool AddItem(ItemCode code, uint slotIndex)// 인벤토리의 특정슬롯에 추가하는 함수 
    {
        bool result = false;
        return result;
    }
    //인벤토리에 아이템을 일정개수만큼 제거하는 함수 
    //아이템 삭제하는 함수 
    //인벤토리를 전부 비우는 함수
    //인벤토리의 특정위치에서 다른 위치로 이동시키는 함수
    //인벤토리에서 아이템을 일정량 덜어내어 임시슬롯으로 보내는 함수 
    //인벤토리를 정렬하는 함수 
    InvenSlot FindSameItem(ItemData data)//인벤토리에 파라미터와 같은 종류의 아이템이 들어있는 슬롯을 찾는 함수 
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
