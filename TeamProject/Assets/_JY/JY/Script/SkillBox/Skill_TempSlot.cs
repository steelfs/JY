using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Skill_TempSlot : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Image image;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
    }

    public void Open(SkillData skillData)
    {
        image.sprite = skillData.skill_sprite;
        canvasGroup.alpha = 0.5f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = false;
    }
    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void MovePosition()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
