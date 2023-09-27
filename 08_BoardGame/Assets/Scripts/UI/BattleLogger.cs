using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TMPro;
using UnityEngine;

public class BattleLogger : MonoBehaviour
{
    //턴 시작시 몇번째 턴이 시작되었는지 출력
    //턴 번호 다른색으로 출력
    //적이 공격시 공격의 성공, 실패 출력
    // ex/ 성공시   "[적의 공격]이 [당신]의 [{배 이름}]에 명중했습니다."
    // ex/ 성공시   "[나의 공격]이 [적]의 [{배 이름}]에 명중했습니다."
    // 실패시       "[적의 공격]이 [적] 의 포탄이 빗나갔습니다.
    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;


    TextMeshProUGUI logger;
    const int maxLineCount = 15;
    StringBuilder sb;

    const string YOU = "당신";
    const string ENEMY = "적";

    

    List<string> logLines;
    private void Awake()
    {
        logger = GetComponentInChildren<TextMeshProUGUI>();
        sb = new StringBuilder(maxLineCount + 5);
        logLines = new List<string>(maxLineCount + 5);
    }
    private void Start()
    {
        TurnManager.Inst.onTurnStart += Log_TurnStart;

        UserPlayer user = GameManager.Inst.UserPlayer;
        EnemyPlayer enemy = GameManager.Inst.EnemyPlayer;

        foreach(Ship ship in user.Ships)
        {
            ship.onHit += (targetShip) => Log_AttackSuccess(false, targetShip);
            ship.onSinking = (targetShip) => { Log_ShipSinking(true, targetShip); } + ship.onSinking;
        }
        foreach (Ship ship in enemy.Ships)
        {
            ship.onHit += (targetShip) => Log_AttackSuccess(true, targetShip);
            ship.onSinking = (targetShip) => { Log_ShipSinking(true, targetShip); } + ship.onSinking;// a = a + b // 와 같은 원리
        }
        user.onAttackFail += Log_AttackFail;
        enemy.onAttackFail += Log_AttackFail;

        user.onDefeat += Log_Defeat;
        enemy.onDefeat += Log_Defeat;


        Clear();
        Log_TurnStart(1);


    }
    public void Log(string text)
    {
        logLines.Add(text);
        if (logLines.Count > maxLineCount)//줄 수 초과시
        {
            logLines.RemoveAt(0);//맨처음 라인 지우고
        }
        sb.Clear();
        foreach(string line in logLines)//다시 sb로 붙혀주기
        {
            sb.AppendLine(line);
        }
        logger.text = sb.ToString();
    }
    public void Clear()
    {
        logLines.Clear();
        logger.text = string.Empty;
    }



    /// <summary>
    /// 공격 성공시 상황을 출력하는 함수 
    /// </summary>
    /// <param name="isUserAttack">true = 유저, false = 적이 공격했다.</param>
    /// <param name="ship">공격한 배</param>
    void Log_AttackSuccess(bool isUserAttack, Ship ship)
    {
        string attackerColor;
        string attackerName;
        string hittedColor;
        string hittedName;
        if (isUserAttack)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;

            hittedColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            hittedName = ENEMY;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;

            hittedColor = ColorUtility.ToHtmlStringRGB(userColor);
            hittedName = YOU;
        }
        string targetShipColor = ColorUtility.ToHtmlStringRGB(shipColor);//함선용 색상
        // 피격자용 문자열 생성

        //최종 문자열 조합
        Log($"<#{attackerColor}>{attackerName}의 공격</color>\t: <#{hittedColor}>{hittedName}</color>의 <#{targetShipColor}>{ship.ShipName}</color>에 포탄이 명중했습니다. ");

    }
    /// <summary>
    /// 공격실패시 상황을 출력하는 함수
    /// </summary>
    /// <param name="isUserAttack">공격자</param>
    void Log_AttackFail(PlayerBase attacker)
    {

        string attackerColor;
        string attackerName;

        if (attacker is UserPlayer)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;
        }
        Log($"<#{attackerColor}>{attackerName}의 공격</color> : <#{attackerColor}>적</color>의 포탄이 빗나갔습니다.");
    }
    /// <summary>
    /// 함선 침몰시 상황 출력하는 함수  
    /// </summary>
    /// <param name="isUserAttack">true = 유저, false = 적이 공격했다.</param>
    /// <param name="ship">공격한 배</param>
    void Log_ShipSinking(bool isUserAttack, Ship ship)
    {
        string attackerColor;
        string attackerName;
        string hittedColor;
        string hittedName;
        if (isUserAttack)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;

            hittedColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            hittedName = ENEMY;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;

            hittedColor = ColorUtility.ToHtmlStringRGB(userColor);
            hittedName = YOU;
        }
        string targetShipColor = ColorUtility.ToHtmlStringRGB(shipColor);//함선용 색상
        Log($"<#{attackerColor}>{attackerName}</color>의 공격 : <#{hittedColor}>{hittedName}</color>의 <#{targetShipColor}>{ship.ShipName}</color> 이 침몰했습니다.");
    }
    /// <summary>
    /// 턴이 시작할 때 상황을 표시하는 함수 
    /// </summary>
    /// <param name="number">턴 카운트</param>
    void Log_TurnStart(int number)
    {
        string colorText = ColorUtility.ToHtmlStringRGB(turnColor);
        Log($"<#{colorText}>{number}</color> 번째 턴이 시작되었습니다.");
    }
    void Log_Defeat(PlayerBase player)
    {

        if (player is UserPlayer)//내가 졌을 때
        {
            Log($"<#{ColorUtility.ToHtmlStringRGB(userColor)}>{YOU}</color>의 <#{ColorUtility.ToHtmlStringRGB(enemyColor)}>패배</color>입니다..");
        }
        else
        {
            string colorString = ColorUtility.ToHtmlStringRGB(userColor);
            Log($"<#{colorString}>당신</color>의 <#{colorString}>승리</color>!");
        }
    }
}
