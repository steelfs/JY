
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MixerDescription : MonoBehaviour
{


    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI itemDescription;
    StringBuilder sb;
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
        sb = new StringBuilder(10);
    }
    //pause, open, close,
    private void Start()
    {
        Mixer_Slot_Left leftSlot;
        Mixer_Slot_Middle middleSlot;
        Mixer_Slot_Result resultSlot;
        Result_Success result_Success;

        result_Success = FindObjectOfType<Result_Success>();
        leftSlot = GameManager.Mixer.MixerUI.Left_Slot;
        middleSlot = GameManager.Mixer.MixerUI.Middle_Slot;
        resultSlot = GameManager.Mixer.MixerUI.Result_Slot;

        leftSlot.onPointerEnter += Open_Description;
        middleSlot.onPointerEnter += Open_Description;
        resultSlot.onPointerEnter += Open_Description;
        result_Success.onPointerEnter += Open_Description;

        leftSlot.onPointerExit += Close;
        middleSlot.onPointerExit += Close;
        resultSlot.onPointerExit += Close;
        result_Success.onPointerExit += Close;

        leftSlot.onPointerMove += MovePosition;
        middleSlot.onPointerMove += MovePosition;
        resultSlot.onPointerMove += MovePosition;
        result_Success.onPointerMove += MovePosition; 
    }
    public void Open_Description(ItemData data)
    {
        if (!isPause && data != null)
        {
            IEquippable equippable_Item = data as IEquippable;
            if(equippable_Item != null)
            {
                sb.Clear();
                if (equippable_Item.ATT > 0)
                {
                    sb.AppendLine($"공격력 {equippable_Item.ATT} +");
                }
                if (equippable_Item.DP > 0)
                {
                    sb.AppendLine($"방어력 {equippable_Item.DP} +");
                }
                if (equippable_Item.Critical_Rate > 0)
                {
                    sb.AppendLine($"크리티컬 {equippable_Item.Critical_Rate}% +");
                }
                if (equippable_Item.Dodge_Rate > 0)
                {
                    sb.AppendLine($"회피 {equippable_Item.Dodge_Rate}% +");
                }
                if (equippable_Item.STR > 0)
                {
                    sb.AppendLine($"STR {equippable_Item.STR} +");
                }
                if (equippable_Item.INT > 0)
                {
                    sb.AppendLine($"INT {equippable_Item.INT} +");
                }
                if (equippable_Item.LUK > 0)
                {
                    sb.AppendLine($"LUK {equippable_Item.LUK} +");
                }
                if (equippable_Item.DEX > 0)
                {
                    sb.AppendLine($"DEX {equippable_Item.DEX} +");
                }
                itemDescription.text = sb.ToString();
            }
            else
            {
                itemDescription.text = $"{data.itemDescription}";
            }
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
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
