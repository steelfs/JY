using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Result_Analysis : MonoBehaviour
{
    TextMeshProUGUI totalAttackCount;
    TextMeshProUGUI attackSuccessCount;
    TextMeshProUGUI attackFailCount;
    TextMeshProUGUI attackSuccessRate;


    public int AllAttackCount
    {
        set
        {
            texts[0].text = $"{value} 회";
        }
    }
    public int SuccessAttackCount
    {
        set
        {
            texts[1].text = $"{value} 회";
        }
    }

    public int FailAttackCount
    {
        set
        {
            texts[2].text = $"{value} 회";
        }
    }
    public float SuccessAttackRate
    {
        set
        {
            texts[3].text = $"{value * 100.0f:f1} %";
        }
    }
    TextMeshProUGUI[] texts;

    public bool player;
    private void Awake()
    {
        texts = transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

        totalAttackCount = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        attackSuccessCount = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        attackFailCount = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        attackSuccessRate = transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
    }
    public void Refresh()
    {
        StartCoroutine(Refresh_());
    }

    IEnumerator Refresh_()
    {
        yield return null;
        if (player)
        {
            totalAttackCount.text = $"{GameManager.Inst.UserPlayer.TotalAttackCount} 회";
            attackSuccessCount.text = $"{GameManager.Inst.UserPlayer.AttackSuccessCount} 회";
            attackFailCount.text = $"{GameManager.Inst.UserPlayer.failAttackCount} 회";
            attackSuccessRate.text = $"{GameManager.Inst.UserPlayer.AttackSuccessRate:f1} %"; 
        }
        else
        {
            totalAttackCount.text = $"{GameManager.Inst.EnemyPlayer.TotalAttackCount} 회";
            attackSuccessCount.text = $"{GameManager.Inst.EnemyPlayer.AttackSuccessCount} 회";
            attackFailCount.text = $"{GameManager.Inst.EnemyPlayer.failAttackCount} 회";
            attackSuccessRate.text = $"{GameManager.Inst.EnemyPlayer.AttackSuccessRate:f1} %";
        }
    }
}
