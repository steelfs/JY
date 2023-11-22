using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mixer_Slot_Middle : Mixer_Slot_Base, IPointerClickHandler
{
    public Action onClearMiddleSlot;
    protected override void Start()
    {
        base.Start();
        GameManager.Mixer.onMiddleSlotDataSet += (itemData) => this.ItemData = itemData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ItemData != null)
        {
            onClearMiddleSlot?.Invoke();
        }
    }
}
