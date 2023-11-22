using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum QuickSlot_Type
{
    None,
    Shift,
    _8,
    _9,
    _0,
    Ctrl,
    Alt,
    Space,
    Insert
}

public class QuickSlot : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler, IBeginDragHandler,IEndDragHandler, IDragHandler
{
    Image slotIcon;
    TextMeshProUGUI quickSlotText;
    ItemData_Potion itemData = null;
    TempSlot_For_QuickSlot_ tempSlot;

    //---------------------------------------------- description 팝업 관련
    public Action<ItemData> onPointerEnter;
    public Action onPointerExit;
    public Action<Vector2> onPointerMove;
    //----------------------------------------------
    public Action<ItemData_Potion, uint> on_BeginDrag_With_Potion;
    public Action<SkillData> on_BeginDrag_With_Skill;
    public Action onEndDrag;
    public Action on_Drag;

    public Action<ItemData_Potion, QuickSlot> onSet_ItemData;
    public Action<QuickSlot> on_Clear_Quickslot_Data;
    public Action<QuickSlot> on_SkillSet;

    public QuickSlot_Type type;
    uint itemCount = 0;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
            Refresh_Count(itemCount);
        }
    }
    public ItemData_Potion ItemData
    {
        get => itemData;
        set
        {
            //if (itemData != value)// 조건 삭제시 인벤토리 -> 퀵슬롯 설정할때 델리게이트 중복 등록
            //{
                itemData = value;
                Refresh_Icon();
                if (itemData != null)
                {
                    onSet_ItemData?.Invoke(itemData, this);//SlotManager, QuickSlotManager에서 받음
                    SkillData = null;
                }
                else
                {
                    on_Clear_Quickslot_Data?.Invoke(this);
                }
            //}
        }
    }

    SkillData skillData;
    public SkillData SkillData
    {
        get => skillData;
        set
        {
            if (skillData != value)
            {
                if (value == null)
                {
                    skillData.BindingSlot = QuickSlot_Type.None;
                }
                skillData = value;
                Refresh_Icon();
                if (skillData != null)
                {
                    on_SkillSet?.Invoke(this);//QuickSlotManager에 인풋컨트롤러로 델리게이트 연결 요청
                    ItemData = null;
                    skillData.BindingSlot = this.type;
                }
                else
                {
                    on_Clear_Quickslot_Data?.Invoke(this);//연결 해제 요청
                }
            }
        }
    }

    public bool IsEmpty => ItemData == null && SkillData == null;


    public string QuickSlot_Key_Value { get; set; }
    int index;
    public int Index
    {
        get => index;
        set
        {
            if (index == 99999)
            {
                index = value;
            }
        }
    }
    private void Awake()
    {
        slotIcon = transform.GetChild(1).GetComponent<Image>();
        quickSlotText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        index = 99999;
        tempSlot = transform.parent.GetChild(9).GetComponent<TempSlot_For_QuickSlot_>();

    }
    void Refresh_Icon()
    {
        if (IsEmpty)
        {
            slotIcon.sprite = null;
            slotIcon.color = Color.clear;
        }
        else
        {
            if (SkillData != null)
            {
                slotIcon.sprite = skillData.skill_sprite;
                slotIcon.color = Color.white;
            }
            else if (ItemData != null)
            {
                slotIcon.sprite = itemData.itemIcon;
                slotIcon.color = Color.white;
            }
   
        }
    }
    void Refresh_Count(uint count)
    {
        if (itemData != null)
        {
            quickSlotText.text = Mathf.Max(count, 0).ToString();
        }
        else
        {
            quickSlotText.text = QuickSlot_Key_Value;
        }
    }
    void Clear()
    {
        ItemData = null;
        ItemCount = 0; //ItemData가 null이면 CountText는 자동으로 Default값으로 세팅
        SkillData = null;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(this.itemData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      // onPointerClick?.Invoke(itemData);
      // Clear();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (itemData != null)
        {
            onPointerMove?.Invoke(eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        on_Drag?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (SkillData != null)
        {
            on_BeginDrag_With_Skill?.Invoke(SkillData);
        }
        else if (ItemData != null)
        {
            on_BeginDrag_With_Potion?.Invoke(itemData, itemCount);//tempSlot으로 아이템 이전
        }
        else
        {
            return;
        }
        Clear();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            QuickSlot otherSlot = obj.GetComponent<QuickSlot>();
            if (otherSlot != null)
            {
                if (tempSlot.SkillData != null)
                {
                    otherSlot.SkillData = tempSlot.SkillData;
                }
                else if (tempSlot.ItemData != null)
                {
                    otherSlot.ItemData = tempSlot.ItemData;
                    otherSlot.ItemCount = tempSlot.ItemCount;
                }
            }
        }
        tempSlot.Close();
    }

}
