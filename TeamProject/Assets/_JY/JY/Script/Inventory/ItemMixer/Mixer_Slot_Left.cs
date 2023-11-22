using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mixer_Slot_Left : Mixer_Slot_Base, IPointerClickHandler
{
    public Action onClearLeftSlot;

    protected override void Start()
    {
        base.Start();
        GameManager.Mixer.onLeftSlotDataSet += (itemData) => this.ItemData = itemData;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ItemData != null)
        {
            onClearLeftSlot?.Invoke();// mixer에서 ItemData 를 null 로 만들기때문에 굳이 여기서 null로 변경하지 않는다.
        }
    }
}