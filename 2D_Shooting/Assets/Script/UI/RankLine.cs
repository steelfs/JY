using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI nameTxt;
    TextMeshProUGUI recordTxt;
    private void Awake()
    {
        nameTxt = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        recordTxt = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    public void SetData(string rankName, int record)
    {
        nameTxt.text = rankName;
        recordTxt.text = record.ToString("N0");//N0 를 해야 세자리마다 , 가 찍힌다
    }
}
