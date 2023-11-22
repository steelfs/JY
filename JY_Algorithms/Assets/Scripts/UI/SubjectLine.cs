using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubjectLine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public AnimationCurve animationCurve;
    Outline outline;
    Image image;
    public Color normalColor;
    public Color hoverColor;
    public Color pressedColor;

    Color baseOutLineColor;
    float outLineAlpha = 0.0f;
    float timeElapsed = 0;
    float time = 0;
    public void OnPointerDown(PointerEventData eventData)
    {
        image.color = pressedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.color = normalColor;
        GameManager.Inst.CurrentScene = CurrentScene.Field;
    }

    private void Awake()
    {
        outline = GetComponent<Outline>();
        image = GetComponent<Image>();
        baseOutLineColor = outline.effectColor;
    }
    private void Update()
    {
        time += Time.deltaTime;
        timeElapsed = (Mathf.Sin(time) + 1) * 0.5f;
        outLineAlpha = animationCurve.Evaluate(timeElapsed);
        baseOutLineColor.a = outLineAlpha;
        outline.effectColor = baseOutLineColor;
    }
}
