using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;// 이 UI가 보여줄 인벤토리
    InvenSlotUI[] slotsUI; //이 인벤토리가 갖고있는 모든 슬롯의 UI

    TempSlotUI tempSlotUI; // 아이템 이동, 분리시 사용함 임시슬롯의 UI

    DetailInfo itemDetailInfo;

    public ItemSpliterUI spliter;// 아이템분리하는 패널

    public Player Owner => inven.Owner; //이 인벤토리의 소유자를 확인하기 위한 프로퍼티 
    public bool IsMoving { get; set; } = false;
    bool isShiftPress = false;

    PlayerInputAction inputActions;

    private void Awake()
    {
        Transform  child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>(); 
        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        itemDetailInfo = GetComponentInChildren<DetailInfo>();

        spliter = GetComponentInChildren<ItemSpliterUI>();
        spliter.onOkClick += OnSpliterOk;

        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Shift.performed += OnShiftPress;
        inputActions.UI.Shift.canceled += OnShiftPress;
    }

    private void OnShiftPress(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isShiftPress = !context.canceled;
    }

    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        //슬롯초기화
        for (uint i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].InitilizeSlot(inven[i]);//UI 와 내부 배열의 slot과 바인딩, 연결 후 델리게이트 바인딩
            slotsUI[i].onDragBegin += OnItemMoveBegin;
            slotsUI[i].onDragEnd += OnItemMoveEnd;
            slotsUI[i].onClick += OnSlotClick;
            slotsUI[i].onPointerEnter += OnItemDetailOn;
            slotsUI[i].onPointerExit += OnItemDetailOff;
            slotsUI[i].onPointerMove += OnSlotPointerMove;
        }

        //임시슬롯 초기화
        tempSlotUI.InitilizeSlot(inven.TempSlot);
        tempSlotUI.onTempSlotOpenClose += OnDetailPause;// tempSlot이 열렸다는건 드레그가 시작됐다는 것 따라서 Detail을 Pause시킨다. Close될땐 False를 매개변수로 호출하고 Open일 때에는 true로 호출

        itemDetailInfo.Close();
        spliter.Close();
        spliter.onCancel += () => itemDetailInfo.IsPause = false;
    }


    private void OnItemMoveBegin(uint index)
    {
        inven.MoveItem(index, tempSlotUI.Index);//시작슬롯에서 임시슬롯으로 아이템 옮기기
        tempSlotUI.Open();//임시슬롯 열기
    }
    private void OnItemMoveEnd(uint index, bool succes)
    {
        inven.MoveItem(tempSlotUI.Index, index);//임시슬롯에서 도착슬롯으로 아이템 옮기기
        if (tempSlotUI.InvenSlot.isEmpty)//비었다면 같은종류의 아이템일때 일부만 들어가는 경우가 있으므로
        {
            tempSlotUI.Close();
        }
        if (succes)
        {
            itemDetailInfo.Open(inven[index].ItemData);// 그래그가 성공적으로 끝나면 상세정보창 열기
        }
    }
    private void OnSlotClick(uint index)
    {
        if (tempSlotUI.InvenSlot.isEmpty)
        {
            if (isShiftPress)
            {
                OnSpliterOpen(index);
            }
            //아이템사용, 장비 등등
        }
        else//임시슬롯에 아이템이있을때 클릭이 되었으면 
        {
            OnItemMoveEnd(index, true);// 
        }
    }
    private void OnSlotPointerMove(Vector2 screenPos)
    {
        itemDetailInfo.MovePosition(screenPos);
    }

    private void OnItemDetailOff(uint index)
    {
        itemDetailInfo.Close();
    }

    private void OnItemDetailOn(uint index)
    {
       itemDetailInfo.Open(slotsUI[index].InvenSlot.ItemData);
    }
    private void OnDetailPause(bool isPause)
    {
        itemDetailInfo.IsPause = isPause;
    }
    private void OnSpliterOk(uint index, uint count) //아이템 분리창에서 ok버튼 눌러졌을 때 실행될 함수 
    {
        inven.SplitItem(index, count);
        tempSlotUI.Open();
    }

    void OnSpliterOpen(uint index)
    {
        InvenSlotUI target = slotsUI[index];
        spliter.transform.position = target.transform.position + Vector3.up * 100;
        spliter.Open(target.InvenSlot);
        itemDetailInfo.IsPause = true;
    }
}
