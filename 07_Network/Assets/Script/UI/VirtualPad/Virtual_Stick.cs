using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Virtual_Stick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    RectTransform containerRect;
    RectTransform handleRect;
    float stickRange;

    public Action<Vector2> onMoveInput;
    private void Awake()
    {
        containerRect = transform as RectTransform;
        handleRect = transform.GetChild(0).GetComponent<RectTransform>();
        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 direction);

        direction = Vector2.ClampMagnitude(direction, stickRange);
        InputUpdate(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputUpdate(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    void InputUpdate(Vector2 direction)
    {
        handleRect.anchoredPosition = direction;
        onMoveInput?.Invoke(direction / stickRange);
    }
}
