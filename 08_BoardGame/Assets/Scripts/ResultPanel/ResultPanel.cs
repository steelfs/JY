using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    Button dropDown;
    Button restartButton;

    GameObject resultBoard;
    CanvasGroup canvasGroup;

    UserPlayer user;
    EnemyPlayer enemy;

    ResultBoard board;
    Result_Analysis userAnalysis;
    Result_Analysis enemyAnalysis;

    private void Awake()
    {
        dropDown = transform.GetChild(0).GetComponent<Button>();
        board = GetComponentInChildren<ResultBoard>();
        userAnalysis = transform.GetChild(1).GetChild(1).GetComponent<Result_Analysis>();
        enemyAnalysis = transform.GetChild(1).GetChild(2).GetComponent<Result_Analysis>();
        restartButton = transform.GetChild(1).GetChild(3).GetComponent<Button>();

        canvasGroup = GetComponent<CanvasGroup>();
        resultBoard = transform.GetChild(1).gameObject;

        dropDown.onClick.AddListener(board.ToggleOnOff);
        restartButton.onClick.AddListener(ReStart);
       // analysis = GetComponentsInChildren<Result_Analysis>();
    }

    void ReStart()
    {

    }
    void Open()
    {
        userAnalysis.AllAttackCount = user.AttackSuccessCount + user.failAttackCount;
        userAnalysis.SuccessAttackCount = user.AttackSuccessCount;
        userAnalysis.FailAttackCount = user.failAttackCount;
        userAnalysis.SuccessAttackRate = (float)user.AttackSuccessCount / (float)(user.AttackSuccessCount + user.failAttackCount);

        enemyAnalysis.AllAttackCount = enemy.AttackSuccessCount + enemy.failAttackCount;
        enemyAnalysis.SuccessAttackCount = enemy.AttackSuccessCount;
        enemyAnalysis.FailAttackCount = enemy.failAttackCount;
        enemyAnalysis.SuccessAttackRate = (float)enemy.AttackSuccessCount / (float)(enemy.AttackSuccessCount + enemy.failAttackCount);
        gameObject.SetActive(true);
    }
    void Close()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        user = GameManager.Inst.UserPlayer;
        enemy = GameManager.Inst.EnemyPlayer;
        user.onDefeat += (_) =>
        {
            board.SetDefeat();
            Open();
        };
        enemy.onDefeat += (_) =>
        {
            board.SetVictory();
            Open();
        };
        Close();
    }
}
