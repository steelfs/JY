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
    TextMeshProUGUI logger;
    StringBuilder sb;

    PlayerBase userPlayer;
    PlayerBase enemyPlayer;
    private void Awake()
    {
        logger = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        sb = new StringBuilder(4);
    }
    private void Start()
    {
        TurnManager.Inst.onTurnStart += PrintTurnText;
        userPlayer = GameManager.Inst.UserPlayer;
        enemyPlayer = GameManager.Inst.EnemyPlayer;
        userPlayer.Board.on_AttackSuccess += AttackSuccess;
        userPlayer.Board.on_AttackFailed += AttackFailed;
        enemyPlayer.Board.on_AttackSuccess += AttackSuccess;
        enemyPlayer.Board.on_AttackFailed -= AttackFailed;
        userPlayer.onDefeat += GameOver;
        enemyPlayer.onDefeat += GameOver;
        int i = 0;
        while(i < 5)
        {
            userPlayer.Ships[i].onSinking += Sinking;
            enemyPlayer.Ships[i].onSinking += Sinking;
            i++;
        }
    }
    void PrintTurnText(int turnNumber)
    {
        sb.AppendLine($"<#00ffff>{turnNumber}</color> 번째 턴이 시작되었습니다.");
        logger.text = sb.ToString();
    }
    void AttackSuccess(ShipType type, string Opponent, string owner)
    {
        if (userPlayer.IsDepeat || enemyPlayer.IsDepeat)
            return;
        string shipType = type.ToString();
        if (Opponent == "나")
        {
            sb.AppendLine($"<#00ff00>{Opponent}의 공격</color> : <#ff0000>{owner}</color>의 {shipType}에 포탄이 명중했습니다.");
        }
        else
        {
            sb.AppendLine($"<#ff0000>{Opponent}의 공격</color> : <#00ff00>{owner}</color>의 {shipType}에 포탄이 명중했습니다.");
        }
        logger.text = sb.ToString();
    }
    //#FF0000이고, 초록색은 #00FF00
    void AttackFailed(string opponent)
    {
        if (userPlayer.IsDepeat || enemyPlayer.IsDepeat)
            return;

        if (opponent == "나")
        sb.AppendLine($"<#00ff00>{opponent}의 공격</color> : <#00ff00>{opponent}</color>의 포탄이 빗나갔습니다.");
        logger.text = sb.ToString();
    }
    void Sinking(Ship ship)
    {
        if (userPlayer.IsDepeat || enemyPlayer.IsDepeat)
            return;

        if (ship.Owner == "나")
        {
            sb.AppendLine($"<#ff0000>{ship.Opponent}의</color> 공격 : <#00ff00>{ship.Owner}</color>의 [{ship.ShipName}]이 침몰했습니다.");
        }
        else
        {
            sb.AppendLine($"<#00ff00>{ship.Opponent}의</color> 공격 : <#ff0000>{ship.Owner}</color>의 [{ship.ShipName}]이 침몰했습니다.");
        }
        logger.text = sb.ToString();
    }
    void GameOver(PlayerBase player)
    {
        UserPlayer user = player as UserPlayer;
        if (user != null)//내가 졌을 경우
        {
            sb.AppendLine($"<#00ff00>당신</color>의 <#ff0000>패배</color> 입니다...");
        }
        else
        {
            sb.AppendLine($"<#00ff00>당신</color>의 <#00ff00>승리</color>!");
        }
        logger.text = sb.ToString();
    }
}
