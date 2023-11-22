using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Merchant_UI_Item : Base_PoolObj,IPointerClickHandler
{
    [SerializeField]
    uint slotItemCount;
    [SerializeField]
    ItemData slotItemData;
    [SerializeField]
    Slot itemSlot;

    Image icon;

    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDescript;
    TextMeshProUGUI itemDarkForce;
    TextMeshProUGUI itemCoin;
    TextMeshProUGUI itemCount;

    public Action<ItemData, uint, Slot > onItemClick;

    protected override void Awake()
    {
        base.Awake();
        itemName = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        itemDescript = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        itemDarkForce = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        itemCoin = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        itemCount = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        icon = transform.GetChild(2).GetChild(0).GetComponent<Image>();
    }


    public void InitData(ItemData itemData, uint itemCount , Slot slot = null) 
    {
        slotItemCount = itemCount;
        slotItemData = itemData;
        icon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemDescript.text = itemData.itemDescription;
        itemDarkForce.text = $"{itemData.price * 0.1f:0} Force";
        itemCoin.text = $"{itemData.price:0} G";
        this.itemCount.text = $"{itemCount} 개";
        itemSlot = slot;
    }

    public void ResetData()
    {
        slotItemCount = 0;
        slotItemData = null;

        icon.sprite = null;
        itemName.text = "";
        itemDescript.text = "";
        itemDarkForce.text = "";
        itemCoin.text = "";

        itemSlot = null;

        transform.SetParent(poolTransform);
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onItemClick?.Invoke(slotItemData,slotItemCount,itemSlot);
    }
}
