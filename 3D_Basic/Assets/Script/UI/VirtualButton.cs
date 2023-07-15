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
    public void OnPointerClick(PointerEventData eventData)// ������ ������ 
    {
        onClick?.Invoke();
    }
    public void RefreshCoolDown(float ratio)//��Ÿ�� �����Ȳ�� �˸��� ��������Ʈ�� ����Ǹ� ����� �Լ�
    {
        coolDown.fillAmount = ratio;
    }
}
