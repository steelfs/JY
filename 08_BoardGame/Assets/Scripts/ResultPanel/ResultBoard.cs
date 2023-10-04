using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultBoard : MonoBehaviour
{
    public Material VictoryMat;
    public Material DefeatMat;
    TextMeshProUGUI victory;
    private void Awake()
    {
        victory = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ToggleOnOff()
    {
        this.gameObject.SetActive(!gameObject.activeSelf);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void SetVictory()
    {
        victory.fontMaterial = VictoryMat;
        victory.text = "승리!";
    }
    public void SetDefeat()
    {
        
        victory.fontMaterial = DefeatMat;
        victory.text = "패배..";
    }
}
