using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualButton : MonoBehaviour, IPointerClickHandler
{
    Image coolDown;
    public Action onClick;
    private void Awake()
    {
        coolDown = transform.GetChild(1).GetComponent<Image>();
        coolDown.fillAmount = 0.0f;
    }
    public void OnPointerClick(PointerEventData eventData)// 눌렀다 땠을때 
    {
        onClick?.Invoke();
    }
    public void RefreshCoolDown(float ratio)//쿨타임 진행상황을 알리는 델리게이트가 실행되면 실행될 함수
    {
        coolDown.fillAmount = ratio;
    }
}
