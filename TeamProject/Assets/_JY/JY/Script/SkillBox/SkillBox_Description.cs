using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillBox_Description : MonoBehaviour
{
    SkillData skillData;
    public SkillData SkillData
    {
        get => skillData;
        set
        {
            skillData = value;
        }
    }

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    //아이콘, 레벨, 네임, 설명
    TextMeshProUGUI skillName_Text;
    Image currentLevel_Skill_Icon;
    Image nextLevel_Skill_Icon;
    TextMeshProUGUI currentLevel_LevelText;
    TextMeshProUGUI nextLevel_LevelText;
    TextMeshProUGUI currentLevel_Description;
    TextMeshProUGUI nextLevel_Description;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        skillName_Text = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        currentLevel_Skill_Icon = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        currentLevel_LevelText = transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        currentLevel_Description = transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();
        nextLevel_LevelText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        nextLevel_Description = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        rectTransform = (RectTransform)transform;
    }

    public void MovePosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        float maxY = mousePos.y + (rectTransform.sizeDelta.y * 0.5f);
        float minY = mousePos.y - (rectTransform.sizeDelta.y * 0.5f);
        float maxX = mousePos.x + rectTransform.sizeDelta.x;

        float overX = maxX - Screen.width;
        float overY = maxY - Screen.height;

        if (maxX > Screen.width && maxY > Screen.height)
        {
            mousePos.y -= overY;
            mousePos.x -= overX;
        }
        else if (maxX > Screen.width && minY < 0)
        {
            mousePos.x -= overX;
            mousePos.y -= minY;
        }
        else if (maxY > Screen.height)
        {
            mousePos.y -= overY;
        }
        else if (minY < 0)
        {
            mousePos.y -= minY;
        }
        else if (maxX > Screen.width)
        {
            mousePos.x -= overX;
        }


        
        transform.position = mousePos;
    }
    public void Open(SkillData skillData, string currentLevel_Info, string nextLevel_Info)
    {
        this.SkillData = skillData;
        MovePosition();
        Refresh(skillData, currentLevel_Info, nextLevel_Info);
        canvasGroup.alpha = 1.0f;

    }
    public void Close()
    {
        this.SkillData = null;
        canvasGroup.alpha = 0;
    }
    void Refresh(SkillData skillData, string current_Info, string next_Info)
    {
        skillName_Text.text = skillData.SkillName;
        currentLevel_Skill_Icon.sprite = skillData.skill_sprite;
        currentLevel_LevelText.text = skillData.SkillLevel.ToString();
        nextLevel_LevelText.text = $"{skillData.SkillLevel + 1}";
        currentLevel_Description.text = current_Info;
        nextLevel_Description.text = next_Info;
    }
}
