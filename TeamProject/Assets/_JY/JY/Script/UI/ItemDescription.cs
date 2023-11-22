using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    TextMeshProUGUI itemDetail;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    StringBuilder sb;

    Vector2 fixedPos = Vector2.zero;//퀵슬롯 전용 y값 보정용

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
    ItemData itemData = null;
    public ItemData ItemData => itemData;
    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemPrice = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDetail = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        sb = new StringBuilder(10);
    }

    public void Open(ItemData data)
    {
        if (!isPause && data != null)
        {
            itemData = data;//장비창 상호작용 전용

            IEquippable Equippable_Item = data as IEquippable;
       

            if (Equippable_Item != null)
            {
                sb.Clear();
                if (Equippable_Item.ATT > 0)
                {
                    sb.AppendLine($"공격력 {Equippable_Item.ATT} +");
                }
                if (Equippable_Item.DP > 0)
                {
                    sb.AppendLine($"방어력 {Equippable_Item.DP} +");
                }
                if (Equippable_Item.Critical_Rate > 0)
                {
                    sb.AppendLine($"크리티컬 {Equippable_Item.Critical_Rate}% +");
                }
                if (Equippable_Item.Dodge_Rate > 0)
                {
                    sb.AppendLine($"회피 {Equippable_Item.Dodge_Rate}% +");
                }
                if (Equippable_Item.STR > 0)
                {
                    sb.AppendLine($"STR {Equippable_Item.STR} +");
                }
                if (Equippable_Item.INT > 0)
                {
                    sb.AppendLine($"INT {Equippable_Item.INT} +");
                }
                if (Equippable_Item.LUK > 0)
                {
                    sb.AppendLine($"LUK {Equippable_Item.LUK} +");
                }
                if (Equippable_Item.DEX > 0)
                {
                    sb.AppendLine($"DEX {Equippable_Item.DEX} +");
                }
                itemDetail.text = sb.ToString();
            }
            else
            {
                itemDetail.text = $"{data.itemDescription}";
            }
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
            itemPrice.text = data.price.ToString("N0");

            StopAllCoroutines();
            StartCoroutine(FadeIn());

            MovePosition(Mouse.current.position.ReadValue());
        }
        
    }
    public void Toggle_IsPause()
    {
        IsPause = !IsPause;
    }
    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
        itemData = null;
    }
    public void MovePosition(Vector2 mousePos)
    {
        //(rectTransform.sizeDelta.x + screenPos.x) 내 사이즈와 마우스좌표를 더한 후 스크린의 크기를 뺀 결과가 양수면 화면을 넘어갔다 판단.
        float minY = -180.0f;
        if (canvasGroup.alpha > 0.0f)
        {
            int overX = (int)(rectTransform.sizeDelta.x + mousePos.x) - Screen.width;
            overX = Mathf.Max(0, overX);
            mousePos.x -= overX;

            transform.position = mousePos;
            if (rectTransform.anchoredPosition.y < minY)
            {
                fixedPos.x = rectTransform.anchoredPosition.x;
                fixedPos.y = -180.0f;

                rectTransform.anchoredPosition = fixedPos;
            }

        }

    }
    IEnumerator FadeIn()
    {
        while(canvasGroup.alpha < 1.0f)
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
