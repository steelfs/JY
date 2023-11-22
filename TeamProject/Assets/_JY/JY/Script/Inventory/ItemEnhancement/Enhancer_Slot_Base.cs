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
    public bool IsEmpty => ItemData == null;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티// 초기 
    private ItemData_Enhancable itemData = null;
    public ItemData_Enhancable ItemData//SlotManager의  GetItem 함수가 실행될때 Item의 정보를 받아오기위한 프로퍼티
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
        // 상속받은 클래스에서 추가적인 초기화가 필요하기 때문에 가상함수로 만듬
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
            // 비어있으면
            itemIcon.color = Color.clear;
            itemIcon.sprite = null;
        }
        else
        {
            // 아이템이 들어있으면
            itemIcon.sprite = ItemData.itemIcon;      // 아이콘에 이미지 설정
            itemIcon.color = Color.white;                       // 아이콘이 보이도록 투명도 제거
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