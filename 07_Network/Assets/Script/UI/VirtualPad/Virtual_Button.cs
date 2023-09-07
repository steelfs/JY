using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Virtual_Button : MonoBehaviour, IPointerDownHandler
{
    public Action on_Press;

    Image coolTimeImage;
    float coolDown = 3.0f;
    private void Awake()
    {
        coolTimeImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        on_Press?.Invoke();
    }
    public void StartCoolDown()
    {
        StartCoroutine(coolDownProcess());
    }
    IEnumerator coolDownProcess()
    {
        coolTimeImage.fillAmount = 1;
        float time = 0;
        while(time < coolDown)
        {
            time += Time.deltaTime / coolDown;
            coolTimeImage.fillAmount -= time;
            yield return null;

        }
    }
}
