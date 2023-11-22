using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        GameManager.SkillBox.Open();
        if (TurnManager.Instance.CurrentTurn is PlayerTurnObject pto) //현재 턴인지 체크하고 형변환가능하면true 니깐 아군턴
        {
            BattleMapPlayerBase player = (BattleMapPlayerBase)pto.CurrentUnit; //아군턴이면 아군유닛이 무조건있음으로 그냥형변환시킨다.

            //공격범위 리셋
            if (!SpaceSurvival_GameManager.Instance.AttackRange.isAttacRange) //공격안하고있으면   
            {
                ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // 턴 오브젝트찾아서 
                if (player == null)
                {
                    Debug.LogWarning("선택한 유닛이없습니다");
                    return;
                }
                if (!player.IsMoveCheck) //이동중이 아닌경우만  
                {
                    float moveSize = player.CharcterData.Player_Status.Stamina > player.MoveSize ? player.MoveSize : player.CharcterData.Player_Status.Stamina;
                    SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(player.CurrentTile);
                    SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(player.CurrentTile, moveSize);//이동범위표시해주기 
                }
            }
            else //공격 상태면 
            {
                SpaceSurvival_GameManager.Instance.To_AttackRange_From_MoveRange();
            }

        }
        GameManager.Inst.ChangeCursor(false);

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
