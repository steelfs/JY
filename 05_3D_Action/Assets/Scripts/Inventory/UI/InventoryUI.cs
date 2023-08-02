using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    MoneyPanel moneyPanel;
    SortPanel sortPanel;

    Button closeButton;
    CanvasGroup canvasGroup;

    public Action onInventoryOpen_;
    public Action onInventoryClose_;
    public Action<bool> onInventoryOpen;
    bool isOpen = false;
    public bool IsOpen
    {
        get => isOpen;
        set
        {
            isOpen = value;
            if (isOpen)
            {
                Open();
                onInventoryOpen?.Invoke(false);//인벤토리 열리면 공격하지 마라 신호보내기
            }
            else
            {
                Close();
                onInventoryOpen?.Invoke(true);
            }
           
        }
    }
    private void Awake()
    {
        Transform  child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>(); 
        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        itemDetailInfo = GetComponentInChildren<DetailInfo>();

        spliter = GetComponentInChildren<ItemSpliterUI>();
        spliter.onOkClick += OnSpliterOk;

        inputActions = new PlayerInputAction();

        moneyPanel = GetComponentInChildren<MoneyPanel>();
        sortPanel = GetComponentInChildren<SortPanel>();

        canvasGroup = GetComponent<CanvasGroup>();
        closeButton = transform.GetChild(2).GetComponent<Button>();

        closeButton.onClick.AddListener(ChangeIsOpenProperty);
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Shift.performed += OnShiftPress;
        inputActions.UI.Shift.canceled += OnShiftPress;
        inputActions.UI.Click.canceled += OnItemDrop;
        inputActions.UI.InventoryOnOff.performed += OnInventoryOnOff;
    }

    private void OnInventoryOnOff(InputAction.CallbackContext _)
    {
        //if (canvasGroup.interactable)
        //{
        //    Open();
        //}
        //else
        //{
        //    Close();
        //}
        ChangeIsOpenProperty();
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

        Owner.onMoneyChange += (money) => moneyPanel.Money = money;
        sortPanel.onSortRequest += (ItemSortBy) => inven.SlotSorting(ItemSortBy);


        IsOpen = false;
    }
    void ChangeIsOpenProperty()
    {
        IsOpen = !IsOpen;
    }
    void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true; //block 이 되며 감지가 됨
        onInventoryOpen_?.Invoke();// 공격가능하다고 알리는 대리자
    }
    void Close()
    {
        if (tempSlotUI.InvenSlot.isEmpty)
        {
            OnItemMoveEnd(0, false);
        }
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        onInventoryClose_?.Invoke();
    }
    //open함수실행시 알파 1
    //close함수 실행시 alpha 0
    //i 키 누르면 인벤토리가 열려있으면 닫히고 닫혀있으면 열린다.
    //인벤토리가 열려있을때는 공격 할수없다 playerController. onAttack -=
    private void OnItemMoveBegin(uint index)
    {
        inven.MoveItem(index, tempSlotUI.Index);//시작슬롯에서 임시슬롯으로 아이템 옮기기
        tempSlotUI.Open();//임시슬롯 열기
    }
    private void OnItemMoveEnd(uint index, bool succes) //버리기 시작지점?
    {
        uint finalIndex = index;
        if (!succes)
        {
            if (inven.FindEmptySlotIndex(out uint emptyIndex))//클릭 실패시
            {
                finalIndex = emptyIndex;
            }           
        }
        inven.MoveItem(tempSlotUI.Index, finalIndex);
        if (tempSlotUI.InvenSlot.isEmpty)
        {
            tempSlotUI.Close();
        }
        if (succes)
        {
            itemDetailInfo.Open(inven[finalIndex].ItemData);
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
            else
            {
                inven[index].UseItem(Owner.gameObject);//소유자의 아이템 사용
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
    private void OnItemDrop(UnityEngine.InputSystem.InputAction.CallbackContext _)// 마우스클릭이 떨어졌을 때 // 아이템 드랍용
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        RectTransform rect = (RectTransform)transform;
        Vector2 diff = screenPos - (Vector2)transform.position;
        if (!rect.rect.Contains(diff))
        {
            
            tempSlotUI.OnDrop(screenPos);
        }
    }
}
