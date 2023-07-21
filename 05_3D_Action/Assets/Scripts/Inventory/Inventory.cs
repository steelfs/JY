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
    
}
