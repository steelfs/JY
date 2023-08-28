using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerOnLine : MonoBehaviour
{
    TextMeshProUGUI playercount;
    private void Awake()
    {
        playercount = GetComponent<TextMeshProUGUI>();
    }
    public void UpdateCount(int count)
    {
        playercount.text = $"Player : {count}";
    }
}
