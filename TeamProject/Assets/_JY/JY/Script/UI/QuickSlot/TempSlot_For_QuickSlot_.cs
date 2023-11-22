using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TempSlot_For_QuickSlot_ : MonoBehaviour
{
    Image slotIcon;
    TextMeshProUGUI countText;
    CanvasGroup canvasGroup;


    public Action<ItemData_Potion, uint> onEndDrag;
    ItemData_Potion itemData;
    public ItemData_Potion ItemData
    {
        get => itemData;
        set
        {
            itemData = value;
        }
    }
    SkillData skillData;
    public SkillData SkillData
    {
        get => skillData;
        set
        {
            skillData = value;

        }
    }
    uint itemCount = 0;
    public uint ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
        }
    }
    public bool IsOpen => canvasGroup.alpha == 1.0f;
    private void Awake()
    {
        slotIcon = transform.GetChild(1).GetComponent<Image>();
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void OpenWith_Potion_Data(ItemData_Potion itemData, uint count)
    {
        this.SkillData = null;
        this.ItemData = itemData;
        ItemCount = count;
        countText.text = count.ToString();
        slotIcon.sprite = itemData.itemIcon;
        slotIcon.color = Color.white;

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void OpenWith_Skill_Data(SkillData skill_Data)
    {
        this.ItemData = null;
        this.SkillData = skill_Data;
        slotIcon.sprite = skill_Data.skill_sprite;
        slotIcon.color = Color.white;

        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void MovePosition()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
