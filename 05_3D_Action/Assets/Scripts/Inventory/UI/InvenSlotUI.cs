using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

//�κ��丮 ������ UI
public class InvenSlotUI : SlotUIBase,IDragHandler, IBeginDragHandler,IEndDragHandler,IPointerClickHandler, IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    TextMeshProUGUI equippedText;

    public Action<uint> onDragBegin;
    public Action<uint, bool> onDragEnd;
    public Action<uint> onClick;
    public Action<uint> onPointerEnter;
    public Action<uint> onPointerExit;
    public Action<Vector2> onPointerMove;

    protected override void Awake()
    {
        base.Awake();
        equippedText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    protected override void OnRefresh()//������̸� ������
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

    public void OnDrag(PointerEventData eventData)
    {
        //OnBeginDrag �� OnEndDrag �� �ߵ���Ű������ �߰��� �� , ���� �����ڵ� ����
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"�巹�� ���� : {Index} �� ����");
        onDragBegin?.Invoke(Index);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;// ���콺�� ������ ���콺�� ��ġ�� �ִ� ������Ʈ �ҷ�����
        if (obj != null)
        {
            InvenSlotUI endSlot = obj.GetComponent<InvenSlotUI>();
            if (endSlot != null)
            {
                Debug.Log($"�巡�� ���� : {endSlot.Index}�� ����");
                onDragEnd?.Invoke(endSlot.Index, true);
            }
            else
            {
                Debug.Log("������ �ƴմϴ�.");
                onDragEnd?.Invoke(Index, false);
            }
        }
        else
        {
            Debug.Log("�ƹ��� ������Ʈ�� �����ϴ�.");
        }
        Debug.Log($"�巹�� ���� : {Index} �� ����");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(Index);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(Index);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }
}
