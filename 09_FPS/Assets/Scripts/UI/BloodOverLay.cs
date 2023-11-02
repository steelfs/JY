using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodOverLay : MonoBehaviour
{
    public AnimationCurve animationCurve;
    float min = 0.0f;
    float max = 0.7f;

    float inverseMaxHP;
    Image image;
    public Color color = Color.clear;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = color;
    }
    private void Start()
    {
        inverseMaxHP = 1 / GameManager.Inst.Player.maxHP;
        GameManager.Inst.Player.on_HP_Change += OnHPChange;
    }
    void OnHPChange(float hp)
    {
        float ratio = 1 - hp * inverseMaxHP;
        color.a = animationCurve.Evaluate(ratio);
        image.color = color;

    }
}
