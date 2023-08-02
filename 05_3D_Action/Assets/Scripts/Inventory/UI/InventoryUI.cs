using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;// �� UI�� ������ �κ��丮
    InvenSlotUI[] slotsUI; //�� �κ��丮�� �����ִ� ��� ������ UI

    TempSlotUI tempSlotUI; // ������ �̵�, �и��� ����� �ӽý����� UI

    DetailInfo itemDetailInfo;

    public ItemSpliterUI spliter;// �����ۺи��ϴ� �г�

    public Player Owner => inven.Owner; //�� �κ��丮�� �����ڸ� Ȯ���ϱ� ���� ������Ƽ 
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
                onInventoryOpen?.Invoke(false);//�κ��丮 ������ �������� ���� ��ȣ������
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

        //�����ʱ�ȭ
        for (uint i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].InitilizeSlot(inven[i]);//UI �� ���� �迭�� slot�� ���ε�, ���� �� ��������Ʈ ���ε�
            slotsUI[i].onDragBegin += OnItemMoveBegin;
            slotsUI[i].onDragEnd += OnItemMoveEnd;
            slotsUI[i].onClick += OnSlotClick;
            slotsUI[i].onPointerEnter += OnItemDetailOn;
            slotsUI[i].onPointerExit += OnItemDetailOff;
            slotsUI[i].onPointerMove += OnSlotPointerMove;
        }

        //�ӽý��� �ʱ�ȭ
        tempSlotUI.InitilizeSlot(inven.TempSlot);
        tempSlotUI.onTempSlotOpenClose += OnDetailPause;// tempSlot�� ���ȴٴ°� �巹�װ� ���۵ƴٴ� �� ���� Detail�� Pause��Ų��. Close�ɶ� False�� �Ű������� ȣ���ϰ� Open�� ������ true�� ȣ��

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
        canvasGroup.blocksRaycasts = true; //block �� �Ǹ� ������ ��
        onInventoryOpen_?.Invoke();// ���ݰ����ϴٰ� �˸��� �븮��
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
    //open�Լ������ ���� 1
    //close�Լ� ����� alpha 0
    //i Ű ������ �κ��丮�� ���������� ������ ���������� ������.
    //�κ��丮�� ������������ ���� �Ҽ����� playerController. onAttack -=
    private void OnItemMoveBegin(uint index)
    {
        inven.MoveItem(index, tempSlotUI.Index);//���۽��Կ��� �ӽý������� ������ �ű��
        tempSlotUI.Open();//�ӽý��� ����
    }
    private void OnItemMoveEnd(uint index, bool succes) //������ ��������?
    {
        uint finalIndex = index;
        if (!succes)
        {
            if (inven.FindEmptySlotIndex(out uint emptyIndex))//Ŭ�� ���н�
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
                inven[index].UseItem(Owner.gameObject);//�������� ������ ���
            }
            //�����ۻ��, ��� ���
        }
        else//�ӽý��Կ� �������������� Ŭ���� �Ǿ����� 
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
    private void OnSpliterOk(uint index, uint count) //������ �и�â���� ok��ư �������� �� ����� �Լ� 
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
    private void OnItemDrop(UnityEngine.InputSystem.InputAction.CallbackContext _)// ���콺Ŭ���� �������� �� // ������ �����
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
