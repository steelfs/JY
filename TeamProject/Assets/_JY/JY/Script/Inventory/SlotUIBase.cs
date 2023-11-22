using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SlotUI_Base : MonoBehaviour
{
    Slot slotComp;
    Image itemIcon;

    TextMeshProUGUI itemCountText;
    QuickSlot bindingSlot;
    public QuickSlot BindingSlot 
    {
        get => bindingSlot;
        set
        {
            if (value == null)//슬롯의 바인딩슬롯(QuickSlot)을 null로 셋팅 할 때  
            {
                bindingSlot.ItemCount = 0;
                bindingSlot.ItemData = null;//바인딩된 슬롯의 ItemData역시 null 로 셋팅 한다.
            }
            bindingSlot = value;
        }
    }
    public Action<QuickSlot, ItemData> onItemCountChange;
    public Action<ItemData> onItemDataChange;
    public bool IsEmpty => ItemData == null;//SlotManager에서  빈 슬롯인지 확인할때 쓰일 프로퍼티// 초기 
    private ItemData itemData = null;
    public ItemData ItemData//SlotManager의  GetItem 함수가 실행될때 Item의 정보를 받아오기위한 프로퍼티
    {
        get => itemData;
        set
        {
            if (itemData != value)
            {
                itemData = value;
                onItemDataChange?.Invoke(itemData);
                IQuest_Item questItem = itemData as IQuest_Item;
                if (questItem != null)
                {
                    questItem.on_QuestItem_CountChange?.Invoke((int)itemCount, itemData.code);
                }
            }
        }
    }
    uint itemCount;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            if (itemCount != value)
            {
                itemCount = value;
                onItemDataChange?.Invoke(itemData);
                if (BindingSlot != null)
                {
                    onItemCountChange?.Invoke(BindingSlot, itemData);
                }
            }
            if (itemData != null) 
            {
                ItemData.ItemCountBinding((int)itemCount);
            }
        }
    }
    protected virtual void Awake()
    {
        // 상속받은 클래스에서 추가적인 초기화가 필요하기 때문에 가상함수로 만듬
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCountText = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯 초기화용 함수
    /// </summary>
    /// <param name="slot">이 UI와 연결할 슬롯</param>
    public virtual void InitializeSlot(Slot slot)
    {
        this.slotComp = slot;                       // 슬롯 저장
        onItemDataChange = Refresh;   // 슬롯에 변화가 있을 때 실행될 함수 등록
        ItemData = null;
        Refresh(itemData);                              // 초기 모습 갱신
    }

    /// <summary>
    /// 슬롯이 보이는 모습을 갱신하는 함수
    /// </summary>
    protected virtual void Refresh(ItemData itemData)
    {
        if (IsEmpty)
        {
            // 비어있으면
            itemIcon.color = Color.clear;   // 아이콘 안보이게 투명화
            itemIcon.sprite = null;         // 아이콘에서 이미지 제거
            itemCountText.text = string.Empty;  // 개수도 안보이게 글자 제거
        }
        else
        {
            // 아이템이 들어있으면
            itemIcon.sprite = ItemData.itemIcon;      // 아이콘에 이미지 설정
            itemIcon.color = Color.white;                       // 아이콘이 보이도록 투명도 제거
            itemCountText.text = ItemCount.ToString(); 
            if (itemCount < 2)
            {
                itemCountText.text = string.Empty;
            }
        }

        OnRefresh();        // 상속받은 클래스에서 개별로 실행하고 싶은 코드 실행
    }

    /// <summary>
    /// 상속받은 클래스에서 개별적으로 실행하고 싶은 코드를 모아놓은 함수
    /// </summary>
    protected virtual void OnRefresh()
    {
    }
}