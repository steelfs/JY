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
        bool result = false;
        ItemData data = itemdataManager[code];

        InvenSlot sameDataSlot = FindSameItem(data);
        if (sameDataSlot != null)
        {
            //같은종류의 아이템이 있다.
            result = sameDataSlot.IncreaseSlotItem(out uint _);//넘치는 개수 의미없어서 따로 받지 않음
        }
        else
        {
            InvenSlot emptySlot = FindEnptySlot();
            if (emptySlot != null)
            {
                emptySlot.AssignSlotItem(data); //빈슬롯이 있으면 할당
                result = true;
            }
            else
            {
                // 빈 슬롯이 없다.
                Debug.Log("아이템 추가 실패 , 인벤토리가 가득차 있습니다.");
            }
        }

       
        return result;
    }
    public bool AddItem(ItemCode code, uint slotIndex)// 인벤토리의 특정슬롯에 추가하는 함수 
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
                    Debug.Log($"해당 슬롯에 아이템을 추가할 수 없습니다.{slotIndex} 번째 다른 아이템이 들어있습니다.");
                }
            }
        }
        else
        {
            Debug.Log("해당 슬롯에 아이템을 추가할 수 없습니다.");
        }
        return result;
    }
    public void MoveItem(uint from, uint to)//아이템을 from index에서 to index로 옮긴다.
    {
        if ((from != to) &&IsValidIndex(from) && IsValidIndex(to))//from과 to가 다르면서 모두 유효한 인덱스일때
        {
            InvenSlot fromSlot = (from == tempSlotIndex)? tempSlot : slots[from]; //임시저장용슬롯 감안해서 삼항연산자 처리
            if (!fromSlot.isEmpty)
            {
                InvenSlot toSlot = (to == tempSlotIndex) ? tempSlot : slots[to];
                if (fromSlot.ItemData == toSlot.ItemData)//같은종류의 아이템이면
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);//
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);//넘친만큼 fromSlot의 개수 감소
                    Debug.Log($"{from} 슬롯에서 {to}슬롯으로 아이템 합침");
                }
                else
                {
                    ItemData itemData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
                    toSlot.AssignSlotItem(itemData, tempCount);
                    Debug.Log($"{from}번 슬롯과 {to}번 슬롯의 아이템 교체");
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
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)//isAcending = true면 오름차순
    {
        List<InvenSlot> beforeSlots = new List<InvenSlot>(slots);// slots배열을 이용해서 리스트 만들기
        //foreach(InvenSlot slot in slots)
        //{
        //    beforeSlots.Add(slot);
        //}
        switch (sortBy)// sort 함수의파라미터로 들어갈 람다함수를 다르게 작성
        {
            case ItemSortBy.Code:
                beforeSlots.Sort((x, y) => // x, y는 서로 비교할 2개 beforeslots 에 들어있는 2개
                {
                    if (x.ItemData == null) // itemData는 비어있을 수 있으니 비어있으면 비어있는것이 뒤쪽으로 설정
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);// 오름차순일때 CompareTo 함수로 비교 
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
        //beforeSlots는 이 시점에서 정해진 기준에 따하 정렬 완료

        //List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(SlotCount);//아이템 종류와 개수를 따로 저장하기
        //foreach(var slot in beforeSlots)
        //{
        //    sortedData.Add((slot.ItemData, slot.ItemCount));
        //}

        ////슬롯에 아이템 종류와 개수를 순서대로 할당하기
        //int index = 0;
        //foreach(var data in sortedData)
        //{
        //    slots[index].AssignSlotItem(data.Item1, data.Item2);
        //}
        slots = beforeSlots.ToArray();//위 코드는 수동으로 넣기
        RefreshInventory();
    }

    void RefreshInventory()//모든슬롯이 변경되었음을 알리는 함수 
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
            Debug.Log($"감소 실패 : {slotIndex} 는 없는 인덱스 입니다.");
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
            Debug.Log($"삭제 실패 : {slotIndex} 는 없는 인덱스 입니다.");
        }
    }
    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }
    //인벤토리에 아이템을 일정개수만큼 제거하는 함수 
    //아이템 삭제하는 함수 
    //인벤토리를 전부 비우는 함수
    //인벤토리의 특정위치에서 다른 위치로 이동시키는 함수
    //인벤토리에서 아이템을 일정량 덜어내어 임시슬롯으로 보내는 함수 
    //인벤토리를 정렬하는 함수 
    InvenSlot FindSameItem(ItemData data)//인벤토리에 파라미터와 같은 종류의 아이템이 들어있는 슬롯을 찾는 함수 
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
    InvenSlot FindEnptySlot()//비어있는 첫번째 슬롯
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
    bool IsValidIndex(uint index) => (index < SlotCount) || (index == tempSlotIndex); //두 조건 중 하나라도 만족을 해야 valid한 인덱스

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
                printText += "(빈칸)";
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
            printText += "(빈칸)";
        }
        Debug.Log(printText);
    }
}
