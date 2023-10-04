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

    public bool player;
    private void Awake()
    {
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
            attackFailCount.text = $"{GameManager.Inst.UserPlayer.AttackFailCount} 회";
            attackSuccessRate.text = $"{GameManager.Inst.UserPlayer.AttackSuccessRate:f1} %"; 
        }
        else
        {
            totalAttackCount.text = $"{GameManager.Inst.EnemyPlayer.TotalAttackCount} 회";
            attackSuccessCount.text = $"{GameManager.Inst.EnemyPlayer.AttackSuccessCount} 회";
            attackFailCount.text = $"{GameManager.Inst.EnemyPlayer.AttackFailCount} 회";
            attackSuccessRate.text = $"{GameManager.Inst.EnemyPlayer.AttackSuccessRate:f1} %";
        }
    }
}
