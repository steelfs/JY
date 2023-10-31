using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoint : MonoBehaviour
{
    TextMeshProUGUI hpText;
    private void Awake()
    {
        hpText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        GameManager.Inst.Player.on_HP_Change = On_HpChange;
        
    }
    void On_HpChange(float hp)
    {
        hpText.text = $"{hp:f0} / 100";
    }
}
