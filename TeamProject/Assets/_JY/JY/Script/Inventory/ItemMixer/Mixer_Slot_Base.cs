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
    public Image imageComp;//���� ������ �󽽷��̹���
    public Image itemIcon;//���� �������� ������
    TextMeshProUGUI itemNameText;


    public Action<ItemData> onItemDataChange;
    public Action<ItemData> onPointerEnter;
    public Action<Vector2> onPointerMove;
    public Action onPointerExit;
    public bool IsEmpty => ItemData == null;//SlotManager����  �� �������� Ȯ���Ҷ� ���� ������Ƽ// �ʱ� 
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