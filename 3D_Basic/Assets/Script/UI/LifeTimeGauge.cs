using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI textMeshProUGUI;

    float maxValue = 1.0f;
    private void Awake()
    {
        slider = GetComponent<Slider>();//??
        textMeshProUGUI = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onLifeTimeChange += Refresh;
        maxValue = player.lifeTimeMax;
    }

    void Refresh(float ratio)
    {
        slider.value = ratio;
        textMeshProUGUI.text = ratio.ToString("N2");
        //$"{(ratio * maxValue) :f1} Sec"; 
    }

    //몇초 남았는지 소숫점 한자리수까지 출력
}
