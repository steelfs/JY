using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnPrinter : MonoBehaviour
{
    TextMeshProUGUI turnText;
    private void Awake()
    {
        turnText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        TurnManager.Inst.onTurnStart += UpdateTurnText;
        turnText.text = "1 ео";
    }
    void UpdateTurnText(int turnCount)
    {
        turnText.text = $"{turnCount + 1} ео";
    }
}
