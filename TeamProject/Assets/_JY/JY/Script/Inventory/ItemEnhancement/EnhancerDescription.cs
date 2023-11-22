using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnhancerDescription : MonoBehaviour
{


    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemLevel;
    public TextMeshProUGUI attackPointText;
    public TextMeshProUGUI defencePointText;
    public CanvasGroup canvasGroup;

    bool isPause = false;

    public float alphaChangeSpeed = 10.0f;
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if (isPause)
                Close();
        }
    }
    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    //pause, open, close,
    private void Start()
    {

        Enhancer_Slot_Before beforeSlot;
        Enhancer_Slot_After afterSlot;
        beforeSlot = GameManager.Enhancer.EnhancerUI.BeforeSlot;
        afterSlot = GameManager.Enhancer.EnhancerUI.AfterSlot;

        beforeSlot.onPointerEnter += Open_BeforeSlotDescription;
        afterSlot.onPointerEnter += Open_AfterSlotDescription;
        beforeSlot.onPointerExit += Close;
        afterSlot.onPointerExit += Close;
        beforeSlot.onPointerMove += MovePosition;
        afterSlot.onPointerMove += MovePosition;
    }
    public void Open_BeforeSlotDescription(ItemData_Enhancable data)
    {
        if (!isPause && data != null)
        {
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
            itemLevel.text = $"{data.itemLevel}";
            attackPointText.text = $"{data.attackPoint}";
            defencePointText.text = $"{data.defencePoint}";
            StopAllCoroutines();
            StartCoroutine(FadeIn());

            MovePosition(Mouse.current.position.ReadValue());
        }
    }
    public void Open_AfterSlotDescription(ItemData_Enhancable data)
    {
        data.Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        if (!isPause && data != null)
        {
            itemIcon.sprite = data.itemIcon;
            itemName.text = itemname;
            itemLevel.text = $"{data.itemLevel + 1}";
            attackPointText.text = $"{resultAttackPoint}";
            defencePointText.text = $"{resultDefencePoint}";
            StopAllCoroutines();
            StartCoroutine(FadeIn());

            MovePosition(Mouse.current.position.ReadValue());
        }
    }
    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
    public void MovePosition(Vector2 screenPos)
    {
        if (canvasGroup.alpha > 0.0f)
        {
            RectTransform rectTransform = (RectTransform)transform;
            int overX = (int)(rectTransform.sizeDelta.x + screenPos.x) - Screen.width; //화면을 넘어간 부분 구하기
            overX = Mathf.Max(0, overX);
            screenPos.x -= overX;

            transform.position = screenPos;
        }

    }
    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
        yield break;
    }
    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
        yield break;

    }
}