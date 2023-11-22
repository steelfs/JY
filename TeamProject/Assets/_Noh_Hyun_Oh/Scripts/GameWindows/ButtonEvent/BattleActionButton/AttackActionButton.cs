using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        //if (TurnManager.Instance.CurrentTurn is PlayerTurnObject pto) //현재 턴인지 체크하고 형변환가능하면true 니깐 아군턴
        //{
        //    BattleMapPlayerBase player = (BattleMapPlayerBase)pto.CurrentUnit; //아군턴이면 아군유닛이 무조건있음으로 그냥형변환시킨다.
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
