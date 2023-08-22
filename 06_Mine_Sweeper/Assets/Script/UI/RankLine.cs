using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RankLine : MonoBehaviour
{
    TextMeshProUGUI rank;
    TextMeshProUGUI record;
    TextMeshProUGUI recordText;
    TextMeshProUGUI nameText;

    private void Awake()
    {
        rank = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        record = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        recordText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        nameText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }
    void SetData<T>(int rank_, T record_, string name)
    {
        rank.text = rank_.ToString();
        nameText.text = name;
        if (record_ is int recordInt)
        {
            record.text = recordInt.ToString();
        }
        else if (record_ is float recordFloat)
        {
            record.text = recordFloat.ToString("N1");
        }
    }
}   

