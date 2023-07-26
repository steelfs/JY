using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailInfo : MonoBehaviour
{
    InventoryUI inventoryUI;

    CanvasGroup canvasGroup;
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    TextMeshProUGUI itemDescription;

    InvenSlot invenSlot;

    RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        itemPrice = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild (4);
        itemDescription = child.GetComponent<TextMeshProUGUI>();

        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        inventoryUI = transform.parent.GetComponent<InventoryUI>();
    }
    public void Open(ItemData data)
    {
        if (data != null && !inventoryUI.IsMoving)
        {
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
            itemPrice.text = data.price.ToString("N0");
            itemDescription.text = data.itemDescription.ToString();

            // canvasGroup.alpha = 1.0f;
            StartCoroutine(IncreaseAlpha()); 
        }
    }
    public void Close()
    {
        //canvasGroup.alpha = 0.0f;
        StartCoroutine(DecreaseAlpha());
    }
    IEnumerator IncreaseAlpha()
    {
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += 0.1f;
            yield return null;
        }
        yield break;
    }
    IEnumerator DecreaseAlpha()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= 0.1f;
            yield return null;
        }
        yield break;
    }
    public void MovePosition(Vector2 eventDataPos)
    {
        if (canvasGroup.alpha > 0.0f)
        {
            transform.position = eventDataPos;
            if (rectTransform.anchoredPosition.x > -50.0f)
            {
                Vector2 newPos = rectTransform.anchoredPosition;
                newPos.x = -50.0f;
                rectTransform.anchoredPosition = newPos;
                Debug.Log("범위 벗어남");
            }
        }
    }
    public void OnDetailPause()
    {
        inventoryUI.IsMoving = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDetailAvailable()
    {
        inventoryUI.IsMoving = false;
    }
    //디테일창 화면 벗어나지 않게 하기
    // 이동시 알파 0
    //알파 값 천천히 
}
