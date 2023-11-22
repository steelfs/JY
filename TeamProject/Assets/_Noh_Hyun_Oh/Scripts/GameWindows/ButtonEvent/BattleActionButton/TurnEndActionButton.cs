using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        ITurnBaseData node = TurnManager.Instance.CurrentTurn; //현재 턴인 유닛을 가져와서 
        if (node is EnemyTurnObject ) //적군턴은 내가 종료할수없다.
        {
            return;
        }
        if (node == null) // 없으면 실행안하고 
        {
            Debug.Log("왜못찾냐?");
            return;
        }
        ICharcterBase unit = node.CurrentUnit;
        //테스트코드 
        if (node.CurrentUnit != null)
        {
            SpaceSurvival_GameManager.Instance.AttackRange.ClearLineRenderer(); //공격범위 초기화한다.
            SpaceSurvival_GameManager.Instance.AttackRange.isAttacRange = false;
            SpaceSurvival_GameManager.Instance.AttackRange.isSkillAndAttack = false;

            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(node.CurrentUnit.CurrentTile); //이동범위 리셋시킨다.
            node.CurrentUnit = null;
        }
        


        
        if (!node.IsMove) 
        {
            node.IsTurn = false;
            Debug.Log($"아군턴 종료 남은행동력 :{node.TurnActionValue}");
            MoveActionButton.IsMoveButtonClick = false; //귀찮아서 스태틱

            node.TurnEndAction(); //턴완료 델리게이트를 실행한다 .
        
        }


    }

}
