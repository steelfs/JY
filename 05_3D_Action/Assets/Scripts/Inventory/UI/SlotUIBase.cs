using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIBase : MonoBehaviour
{

    InvenSlot invenSlot;// 이 UI가 표현할 슬롯
    public InvenSlot InvenSlot => invenSlot; // 확인용 프로퍼티

    public uint Index => invenSlot.Index; // 슬롯이 몇번째 슬롯인지

    Image itemIcon;//아이템 아이콘 표시용 이미지
    TextMeshProUGUI itemCount;

    protected virtual void Awake()//상속받을 클래스에서 기능을 추가할 수 있기때문에 virtual
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCount = child.GetComponent<TextMeshProUGUI>();
    }

    public virtual void InitilizeSlot(InvenSlot slot)
    {
        invenSlot = slot; //Invenslot과 바인딩
        invenSlot.onSlotItemChange = Refresh; // 델리게이트 연결
        Refresh();//슬롯이 보이는 모습 갱신
    }

    private void Refresh()
    {
        if (InvenSlot.isEmpty)// InvenSlot의 아이템이 변경되어 델리게이트를 호출하면 (IsEmpty일 경우 )
        {
            itemIcon.color = Color.clear;
            itemCount.text = string.Empty;
            itemIcon.sprite = null;
        }
        else
        {
            itemIcon.sprite = invenSlot.ItemData.itemIcon;
            itemIcon.color = Color.white;
            itemCount.text = invenSlot.ItemCount.ToString();
        }
        OnRefresh();// 추가적으로 원가를 해야할 것이 있을 경우 가상함수를 추가로 실행
    }

    protected virtual void OnRefresh()
    {

    }
}
