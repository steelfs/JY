using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class VirtualPrac : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    RectTransform conTainerRect;
    RectTransform handleRect;

    float movingArea;
    private void Start()
    {
        movingArea = (conTainerRect.rect.width - handleRect.rect.width) * 0.5f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(conTainerRect, eventData.position, eventData.pressEventCamera, out Vector2 pos);
        pos = Vector2.ClampMagnitude(pos, movingArea); 
        handleRect.anchoredPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handleRect.localPosition = Vector2.zero;
    }


    private void Awake()
    {
        conTainerRect = GetComponent<RectTransform>();
        handleRect = transform.GetChild(0).GetComponent<RectTransform>();
    }
}
