using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlotUI : SlotUIBase,IDragHandler, IBeginDragHandler,IEndDragHandler,IPointerClickHandler, IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    TextMeshProUGUI equippedText;

    public Action<uint> onDragBegin;
    public Action<uint> onDragEnd;
    public Action<uint> onClick;
    public Action<uint> onPointerEnter;
    public Action<uint> onPointerExit;
    public Action<Vector2> onPointerMove;

    protected override void Awake()
    {
        base.Awake();
        equippedText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    protected override void OnRefresh()
    {
        if (InvenSlot.IsEquipped)
        {
            equippedText.color = Color.red;
        }
        else
        {
            equippedText.color = Color.clear;
        }
    }

    public override void InitilizeSlot(InvenSlot slot)
    {
        onDragBegin = null;
        onDragEnd = null;
        onClick = null;
        onPointerEnter = null;
        onPointerExit = null;
        onPointerMove = null;

        base.InitilizeSlot(slot);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
