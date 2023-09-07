using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Virtual_Button : MonoBehaviour, IPointerDownHandler
{
    public Action on_Press;

    public void OnPointerDown(PointerEventData eventData)
    {
        on_Press?.Invoke();
    }
}
