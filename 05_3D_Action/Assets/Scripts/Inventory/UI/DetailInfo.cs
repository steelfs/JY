using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    bool isPause = false;
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if (isPause)
            {
                Close();
            }
        }
    }
    float alphaChangeSpeed = 5.0f;

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

    }
    private void Start()
    {
        inventoryUI = transform.parent.GetComponent<InventoryUI>();
    }
    public void Open(ItemData data)
    {
        if (!IsPause && data != null)// && !inventoryUI.IsMoving)
        {
            itemIcon.sprite = data.itemIcon;
            itemName.text = data.itemName;
            itemPrice.text = data.price.ToString("N0");
            itemDescription.text = data.itemDescription.ToString();

            MovePosition(Mouse.current.position.ReadValue());
            // canvasGroup.alpha = 1.0f;
            StopAllCoroutines();
            StartCoroutine(FadeIn()); 
        }
    }
    public void Close()
    {
        //canvasGroup.alpha = 0.0f;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
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
    public void MovePosition(Vector2 screenPos)
    {
        if (canvasGroup.alpha > 0.0f)
        {
            RectTransform rectTransform = (RectTransform)transform;

            int overX = (int)(screenPos.x + rectTransform.sizeDelta.x) - Screen.width;//화면밖으로 넘친부분 계산, 저장
            overX = Mathf.Max(0, overX); //음수일경우 0
            screenPos.x -= overX;//넘친만큼 왼쪽으로 이동

            transform.position = screenPos;
            //if (rectTransform.anchoredPosition.x > -50.0f) //내가 쓴 코드
            //{
            //    Vector2 newPos = rectTransform.anchoredPosition;
            //    newPos.x = -50.0f;
            //    rectTransform.anchoredPosition = newPos;
            //    Debug.Log("범위 벗어남");
            //}
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
