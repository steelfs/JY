using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Result_Success : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    public Action<ItemData> onPointerEnter;
    public Action<Vector2> onPointerMove;
    public Action onPointerExit;

    RectTransform rectTransform;

    MixerDescription mixerDescription;
    Item_Mixer mixer;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    
    }
    private void Start()
    {
        mixerDescription = FindObjectOfType<MixerDescription>();
        mixer = GameManager.Mixer;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 distance = (Vector2)transform.position - eventData.position;
        if (rectTransform.rect.Contains(distance))
        {
            mixerDescription.transform.SetParent(transform);
            onPointerEnter?.Invoke(GameManager.Mixer.ResultSlot.ItemData);
        }
   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mixerDescription.transform.SetParent(mixer.transform);
        onPointerExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 distance = (Vector2)transform.position - eventData.position;
        if (rectTransform.rect.Contains(distance))
        {
            onPointerMove?.Invoke(eventData.position);
        }
    }

}
