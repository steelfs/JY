using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mixer_Slot_Base : MonoBehaviour,IPointerEnterHandler,IPointerMoveHandler,IPointerExitHandler
{
    public enum Mixer_SlotState
    {
        None = 0,
        PointerEnter,
        PointerExit,
    }
    protected Item_Mixer item_Mixer;
    public Image imageComp;//상위 슬롯의 빈슬롯이미지
    public Image itemIcon;//하위 아이템의 아이콘
    TextMeshProUGUI itemNameText;


    public Action<ItemData> onItemDataChange;
    public Action<ItemData> onPointerEnter;
    public Action<Vector2> onPointerMove;
    public Action onPointerExit;
    public bool IsEmpty => ItemData == null;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티// 초기 
    private ItemData itemData = null;
    public ItemData ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                Refresh();
                onItemDataChange?.Invoke(itemData);
            }
        }
    }

    Mixer_SlotState mixerSlotState = Mixer_SlotState.None;
    public Mixer_SlotState SlotState
    {
        get => mixerSlotState;
        set
        {
            if (mixerSlotState != value)
            {
                mixerSlotState = value;
            }
            switch (mixerSlotState)
            {
                case Mixer_SlotState.PointerEnter:
                    if (ItemData != null)
                    {
                        onPointerEnter?.Invoke(ItemData);
                    }
                    break;
                case Mixer_SlotState.PointerExit:
                    onPointerExit?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
    protected virtual void Awake()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        imageComp = GetComponent<Image>();
    }
    protected virtual void Start()
    {
        item_Mixer = GameManager.Mixer;
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
        SlotState = Mixer_SlotState.PointerEnter;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SlotState = Mixer_SlotState.PointerExit;
    }
}