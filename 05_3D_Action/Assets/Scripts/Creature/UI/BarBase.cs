using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarBase : MonoBehaviour
{
    public Color color = Color.white;
    protected Slider slider;
    protected TextMeshProUGUI current;
    protected TextMeshProUGUI max;

    protected float maxValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        current = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        max = transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        Image backGroundImage = transform.GetChild(0).GetComponent<Image>();
        Color backgroundColor = new Color(color.r, color.g, color.b, color.a * 0.5f);
        backGroundImage.color = backgroundColor;

        Image fillImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        fillImage.color = color;
    }
    protected void onvalueChange(float ratio)
    {

    }
}
