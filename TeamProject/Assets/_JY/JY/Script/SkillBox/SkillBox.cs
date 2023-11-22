using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Save_SkillData
{
    public int skillLevel;
    public QuickSlot_Type bindingSlot;
    public SkillType skillType;
    public Save_SkillData(SkillData data)
    {
        this.skillLevel = data.SkillLevel;
        this.bindingSlot = data.BindingSlot;
        this.skillType = data.SkillType;
    }
}

public class SkillBox : MonoBehaviour, IPopupSortWindow
{
    CanvasGroup canvasGroup;
    Button closeButton;
    TextMeshProUGUI darkForce_Text;

    SkillData[] skillDatas;
    public SkillData[] SkillDatas => skillDatas;

    public Action<IPopupSortWindow> PopupSorting { get; set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        closeButton = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        closeButton.onClick.AddListener(Close);
        darkForce_Text = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        skillDatas = new SkillData[5];
        int i = 0; 
        while (i < skillDatas.Length)
        {
            skillDatas[i] = transform.GetChild(i + 2).GetComponent<SkillData>();
            i++;
        }
    }


    private void Toggle_Open_Close(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if (canvasGroup.alpha > 0)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Start()
    {
        InputSystemController.InputSystem.UI_Inven.SkillBox_Open.performed += Toggle_Open_Close;
        //GameManager.PlayerStatus.Base_Status.on_DarkForceChange += Refresh;
        Refresh(GameManager.PlayerStatus.Base_Status.Base_DarkForce);
    }
    public void Open()
    {
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        WindowList.Instance.PopupSortManager.PopupSortDataAppend(this);
    }
    public void Close()
    {
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
        canvasGroup.alpha = 0.0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void Refresh(uint darkForce)
    {
        darkForce_Text.text = $"{darkForce}";
    }

    public void OpenWindow()
    {
        Open();
    }

    public void CloseWindow()
    {
        Close();
    }
    public Save_SkillData[] SaveSkillData()
    {
        SkillData skillData;
        Save_SkillData[] datas = new Save_SkillData[5];
        int i = 0;
        while (i < 5)
        {
            skillData = transform.GetChild(i + 2).GetComponent<SkillData>();
            datas[i] = new Save_SkillData(skillData);
            i++;
        }
        return datas;
    }
    public void LoadSkillData_In_QuickSlot(Save_SkillData[] datas)
    {
        for (int i = 0; i < datas.Length; i++)
        {
            QuickSlot bindingSlot = null;
            SkillData skillData = skillDatas[(int)datas[i].skillType];
            if (datas[i].bindingSlot != QuickSlot_Type.None)
            {
                bindingSlot = GameManager.QuickSlot_Manager.QuickSlots[(int)datas[i].bindingSlot - 1];//저장된 연결 슬롯 가져오기
                bindingSlot.SkillData = skillData;
            }
            if (skillData.SkillLevel != datas[i].skillLevel)//저장된 레벨이 현재 레벨과 다르면 레벨업
            {
                skillData.on_ResetData?.Invoke();
                while (skillData.SkillLevel < datas[i].skillLevel)
                {
                    skillData.on_Skill_LevelUp?.Invoke();
                }
            }

        }
    }
    Save_SkillData[] SkillDatas_;// 세이브용 스킬 데이터

    public void TestSave()
    {
        SkillDatas_ = SaveSkillData();
    }
    public void TestLoadData()
    {
        LoadSkillData_In_QuickSlot(SkillDatas_);
    }
}
