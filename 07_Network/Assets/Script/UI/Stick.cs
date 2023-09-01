using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    RectTransform rect;
    RectTransform backGoundRect;
    float maxDistance;
    Vector2 newPos = Vector2.zero;
    Vector2 origin;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        backGoundRect = transform.parent.GetChild(0).GetComponent<RectTransform>();
        maxDistance = (backGoundRect.sizeDelta.x - rect.sizeDelta.x) * 0.5f;
        origin = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position;
        //Vector2 direction = eventData.position - origin;
        //if (direction.sqrMagnitude > maxDistance)
        //{
        //    direction = Vector2.ClampMagnitude(direction, maxDistance);
        //}
        //transform.position = origin + direction;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rect.anchoredPosition = Vector2.zero;
    }
}
