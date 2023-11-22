using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipBox_Slot : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    Image itemIcon;
    ItemData itemData;
    public EquipType equip_Type;

    public Action<ItemData> onPointerEnter;
    public Action<Vector2> onPointerMove;
    public Action onPointerExit;

    public ItemData ItemData
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                Refresh();
            }
        }
    }
    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }
    void Refresh()
    {
        if (itemData == null)
        {
            itemIcon.color = Color.clear;
           // itemIcon.sprite = null;
        }
        else
        {
            itemIcon.sprite = itemData.itemIcon;
            itemIcon.color = Color.white;
        }
    }
    public void SetItemData(ItemData data)
    {
        if (itemData == null)//���â ������ ����ִ� ���
        {
            ItemData = data;
        }
        else//�̹� �������� �����Ǿ��ִ� ���
        {
            GameManager.SlotManager.Taking_Item_From_EquipBox(itemData);//�ش�����ۿ� �´� �κ��丮 ���� �� ������ ������ 
            ItemData = data;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(itemData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(Mouse.current.position.ReadValue());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }
}
