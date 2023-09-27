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
    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;


    TextMeshProUGUI logger;
    const int maxLineCount = 15;
    StringBuilder sb;

    const string YOU = "���";
    const string ENEMY = "��";

    

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
            ship.onSinking = (targetShip) => { Log_ShipSinking(true, targetShip); } + ship.onSinking;// a = a + b // �� ���� ����
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
        if (logLines.Count > maxLineCount)//�� �� �ʰ���
        {
            logLines.RemoveAt(0);//��ó�� ���� �����
        }
        sb.Clear();
        foreach(string line in logLines)//�ٽ� sb�� �����ֱ�
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
    /// ���� ������ ��Ȳ�� ����ϴ� �Լ� 
    /// </summary>
    /// <param name="isUserAttack">true = ����, false = ���� �����ߴ�.</param>
    /// <param name="ship">������ ��</param>
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
        string targetShipColor = ColorUtility.ToHtmlStringRGB(shipColor);//�Լ��� ����
        // �ǰ��ڿ� ���ڿ� ����

        //���� ���ڿ� ����
        Log($"<#{attackerColor}>{attackerName}�� ����</color>\t: <#{hittedColor}>{hittedName}</color>�� <#{targetShipColor}>{ship.ShipName}</color>�� ��ź�� �����߽��ϴ�. ");

    }
    /// <summary>
    /// ���ݽ��н� ��Ȳ�� ����ϴ� �Լ�
    /// </summary>
    /// <param name="isUserAttack">������</param>
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
        Log($"<#{attackerColor}>{attackerName}�� ����</color> : <#{attackerColor}>��</color>�� ��ź�� ���������ϴ�.");
    }
    /// <summary>
    /// �Լ� ħ���� ��Ȳ ����ϴ� �Լ�  
    /// </summary>
    /// <param name="isUserAttack">true = ����, false = ���� �����ߴ�.</param>
    /// <param name="ship">������ ��</param>
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
        string targetShipColor = ColorUtility.ToHtmlStringRGB(shipColor);//�Լ��� ����
        Log($"<#{attackerColor}>{attackerName}</color>�� ���� : <#{hittedColor}>{hittedName}</color>�� <#{targetShipColor}>{ship.ShipName}</color> �� ħ���߽��ϴ�.");
    }
    /// <summary>
    /// ���� ������ �� ��Ȳ�� ǥ���ϴ� �Լ� 
    /// </summary>
    /// <param name="number">�� ī��Ʈ</param>
    void Log_TurnStart(int number)
    {
        string colorText = ColorUtility.ToHtmlStringRGB(turnColor);
        Log($"<#{colorText}>{number}</color> ��° ���� ���۵Ǿ����ϴ�.");
    }
    void Log_Defeat(PlayerBase player)
    {

        if (player is UserPlayer)//���� ���� ��
        {
            Log($"<#{ColorUtility.ToHtmlStringRGB(userColor)}>{YOU}</color>�� <#{ColorUtility.ToHtmlStringRGB(enemyColor)}>�й�</color>�Դϴ�..");
        }
        else
        {
            string colorString = ColorUtility.ToHtmlStringRGB(userColor);
            Log($"<#{colorString}>���</color>�� <#{colorString}>�¸�</color>!");
        }
    }
}
