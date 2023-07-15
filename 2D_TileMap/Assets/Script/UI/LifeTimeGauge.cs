using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += ChangeLifeTime;

        slider.value = 1.0f;
    }
    void ChangeLifeTime(float ratio)
    {
        slider.value = ratio;
    }
}
