using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        GameManager.SkillBox.Open();
        if (TurnManager.Instance.CurrentTurn is PlayerTurnObject pto) //���� ������ üũ�ϰ� ����ȯ�����ϸ�true �ϱ� �Ʊ���
        {
            BattleMapPlayerBase player = (BattleMapPlayerBase)pto.CurrentUnit; //�Ʊ����̸� �Ʊ������� �������������� �׳�����ȯ��Ų��.

            //���ݹ��� ����
            if (!SpaceSurvival_GameManager.Instance.AttackRange.isAttacRange) //���ݾ��ϰ�������   
            {
                ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // �� ������Ʈã�Ƽ� 
                if (player == null)
                {
                    Debug.LogWarning("������ �����̾����ϴ�");
                    return;
                }
                if (!player.IsMoveCheck) //�̵����� �ƴѰ�츸  
                {
                    float moveSize = player.CharcterData.Player_Status.Stamina > player.MoveSize ? player.MoveSize : player.CharcterData.Player_Status.Stamina;
                    SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(player.CurrentTile);
                    SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(player.CurrentTile, moveSize);//�̵�����ǥ�����ֱ� 
                }
            }
            else //���� ���¸� 
            {
                SpaceSurvival_GameManager.Instance.To_AttackRange_From_MoveRange();
            }

        }
        GameManager.Inst.ChangeCursor(false);

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
