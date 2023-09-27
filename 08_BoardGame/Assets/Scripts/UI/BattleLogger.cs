using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TMPro;
using UnityEngine;

public class BattleLogger : MonoBehaviour
{
    //�� ���۽� ���° ���� ���۵Ǿ����� ���
    //�� ��ȣ �ٸ������� ���
    //���� ���ݽ� ������ ����, ���� ���
    // ex/ ������   "[���� ����]�� [���]�� [{�� �̸�}]�� �����߽��ϴ�."
    // ex/ ������   "[���� ����]�� [��]�� [{�� �̸�}]�� �����߽��ϴ�."
    // ���н�       "[���� ����]�� [��] �� ��ź�� ���������ϴ�.
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
        sb.AppendLine($"<#00ffff>{turnNumber}</color> ��° ���� ���۵Ǿ����ϴ�.");
        logger.text = sb.ToString();
    }
    void AttackSuccess(ShipType type, string Opponent, string owner)
    {
        if (userPlayer.IsDepeat || enemyPlayer.IsDepeat)
            return;
        string shipType = type.ToString();
        if (Opponent == "��")
        {
            sb.AppendLine($"<#00ff00>{Opponent}�� ����</color> : <#ff0000>{owner}</color>�� {shipType}�� ��ź�� �����߽��ϴ�.");
        }
        else
        {
            sb.AppendLine($"<#ff0000>{Opponent}�� ����</color> : <#00ff00>{owner}</color>�� {shipType}�� ��ź�� �����߽��ϴ�.");
        }
        logger.text = sb.ToString();
    }
    //#FF0000�̰�, �ʷϻ��� #00FF00
    void AttackFailed(string opponent)
    {
        if (userPlayer.IsDepeat || enemyPlayer.IsDepeat)
            return;

        if (opponent == "��")
        sb.AppendLine($"<#00ff00>{opponent}�� ����</color> : <#00ff00>{opponent}</color>�� ��ź�� ���������ϴ�.");
        logger.text = sb.ToString();
    }
    void Sinking(Ship ship)
    {
        if (userPlayer.IsDepeat || enemyPlayer.IsDepeat)
            return;

        if (ship.Owner == "��")
        {
            sb.AppendLine($"<#ff0000>{ship.Opponent}��</color> ���� : <#00ff00>{ship.Owner}</color>�� [{ship.ShipName}]�� ħ���߽��ϴ�.");
        }
        else
        {
            sb.AppendLine($"<#00ff00>{ship.Opponent}��</color> ���� : <#ff0000>{ship.Owner}</color>�� [{ship.ShipName}]�� ħ���߽��ϴ�.");
        }
        logger.text = sb.ToString();
    }
    void GameOver(PlayerBase player)
    {
        UserPlayer user = player as UserPlayer;
        if (user != null)//���� ���� ���
        {
            sb.AppendLine($"<#00ff00>���</color>�� <#ff0000>�й�</color> �Դϴ�...");
        }
        else
        {
            sb.AppendLine($"<#00ff00>���</color>�� <#00ff00>�¸�</color>!");
        }
        logger.text = sb.ToString();
    }
}
