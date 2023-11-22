using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enhancer_Slot_Base : MonoBehaviour,IPointerEnterHandler,IPointerMoveHandler,IPointerExitHandler
{
    public enum EnhancerSlotState
    {
        None = 0,
        PointerEnter,
        PointerExit,
    }
    protected Item_Enhancer item_Enhancer;
    public Image imageComp;
    public Image itemIcon;
    public Action onValueChange;
    public Action<ItemData_Enhancable> onPointerEnter;
    public Action<Vector2> onPointerMove;
    public Action onPointerExit;
    public bool IsEmpty => ItemData == null;//SlotManager����  �� �������� Ȯ���Ҷ� ���� ������Ƽ// �ʱ� 
    private ItemData_Enhancable itemData = null;
    public ItemData_Enhancable ItemData//SlotManager��  GetItem �Լ��� ����ɶ� Item�� ������ �޾ƿ������� ������Ƽ
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                onValueChange?.Invoke();
            }
        }
    }

    EnhancerSlotState enhancerSlotState = EnhancerSlotState.None;
    public EnhancerSlotState EnhancerSlotsState
    {
        get => enhancerSlotState;
        set
        {
            if (enhancerSlotState != value)
            {
                enhancerSlotState = value;
            }
            switch (enhancerSlotState)
            {
                case EnhancerSlotState.PointerEnter:
                    if (ItemData != null)
                    {
                        onPointerEnter?.Invoke(ItemData);
                    }
                    break;
                case EnhancerSlotState.PointerExit:
                    onPointerExit?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        // ��ӹ��� Ŭ�������� �߰����� �ʱ�ȭ�� �ʿ��ϱ� ������ �����Լ��� ����
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        imageComp = GetComponent<Image>();

    }
    protected virtual void Start()
    {
        onValueChange = Refresh;
        item_Enhancer = GameManager.Enhancer;
        item_Enhancer.onSetItem += (itemData) => ItemData = itemData;
        Refresh();
    }

    protected virtual void Refresh()
    {
        if (IsEmpty)
        {
            // ���������
            itemIcon.color = Color.clear;
            itemIcon.sprite = null;
        }
        else
        {
            // �������� ���������
            itemIcon.sprite = ItemData.itemIcon;      // �����ܿ� �̹��� ����
            itemIcon.color = Color.white;                       // �������� ���̵��� ���� ����
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnhancerSlotsState = EnhancerSlotState.PointerEnter;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EnhancerSlotsState = EnhancerSlotState.PointerExit;
    }
}