using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        //if (TurnManager.Instance.CurrentTurn is PlayerTurnObject pto) //���� ������ üũ�ϰ� ����ȯ�����ϸ�true �ϱ� �Ʊ���
        //{
        //    BattleMapPlayerBase player = (BattleMapPlayerBase)pto.CurrentUnit; //�Ʊ����̸� �Ʊ������� �������������� �׳�����ȯ��Ų��.
        //    SpaceSurvival_GameManager.Instance.AttackRange.AttackRangeTileView(player.CurrentTile, player.AttackRange);
        //}
    }
    protected override void OnMouseEnter()
    {
        uiController.ViewButtons();
    }
    protected override void OnMouseExit()
    {
        uiController.ResetButtons();
    }
}
