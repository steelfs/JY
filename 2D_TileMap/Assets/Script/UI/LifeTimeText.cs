using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeTimeText : MonoBehaviour
{
    TextMeshProUGUI lifeTimeText;
    float maxLifeTime;
    private void Awake()
    {
        lifeTimeText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifetimeChange;
        maxLifeTime = player.maxLifeTime;
    }

    private void OnLifetimeChange(float ratio)
    {
      
        lifeTimeText.text = $"{maxLifeTime * ratio :N2} Sec"; 
       // lifeTimeText.text = $"{(maxLifeTime * ratio):f2} sec";
    }
}
