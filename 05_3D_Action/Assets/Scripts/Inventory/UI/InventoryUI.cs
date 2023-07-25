using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;// 이 UI가 보여줄 인벤토리
    InvenSlotUI[] slotsUI; //이 인벤토리가 갖고있는 모든 슬롯의 UI

    TempSlotUI tempSlotUI; // 아이템 이동, 분리시 사용함 임시슬롯의 UI

    public Player Owner => inven.Owner; //이 인벤토리의 소유자를 확인하기 위한 프로퍼티 

    private void Awake()
    {
        Transform  child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>(); 
        tempSlotUI = GetComponentInChildren<TempSlotUI>();
    }

    void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        //슬롯초기화
        //임시슬롯 초기화
    }
}
