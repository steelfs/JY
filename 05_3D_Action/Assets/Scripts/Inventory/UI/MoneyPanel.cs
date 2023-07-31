using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyPanel : MonoBehaviour
{
    Player owner;
    TextMeshProUGUI moneyText;

    int money;
    public int Money
    {
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                RefreshMoneyText();
            }
        }
    }
    private void Awake()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        RefreshMoneyText();
    }

    void RefreshMoneyText()
    {
        moneyText.text = $"{money:N0}";
    }
}
