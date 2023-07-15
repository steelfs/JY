using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCountText : MonoBehaviour
{
    public float speed = 3.0f;
    float targetValue = 0.0f;
    float currentValue = 0.0f;


    TextMeshProUGUI killCountText;
    private void Awake()
    {
        killCountText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onKillCountChange += UpdateKillCountText;
    }

    private void UpdateKillCountText(int killCount)
    {
        targetValue = killCount;
        killCountText.text = $"Kill : {killCount.ToString()}";
    }

    private void Update()
    {
        currentValue += Time.deltaTime * speed;
        if (currentValue > targetValue)
        {
            currentValue = targetValue;
        }
        killCountText.text = $"Kill : {Mathf.FloorToInt(currentValue).ToString()}";
    }
}
