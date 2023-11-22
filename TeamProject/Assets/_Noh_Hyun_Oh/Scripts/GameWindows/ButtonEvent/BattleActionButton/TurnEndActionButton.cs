using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndActionButton : BattleActionButtonBase
{
    protected override void OnClick()
    {
        ITurnBaseData node = TurnManager.Instance.CurrentTurn; //���� ���� ������ �����ͼ� 
        if (node is EnemyTurnObject ) //�������� ���� �����Ҽ�����.
        {
            return;
        }
        if (node == null) // ������ ������ϰ� 
        {
            Debug.Log("�ָ�ã��?");
            return;
        }
        ICharcterBase unit = node.CurrentUnit;
        //�׽�Ʈ�ڵ� 
        if (node.CurrentUnit != null)
        {
            SpaceSurvival_GameManager.Instance.AttackRange.ClearLineRenderer(); //���ݹ��� �ʱ�ȭ�Ѵ�.
            SpaceSurvival_GameManager.Instance.AttackRange.isAttacRange = false;
            SpaceSurvival_GameManager.Instance.AttackRange.isSkillAndAttack = false;

            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(node.CurrentUnit.CurrentTile); //�̵����� ���½�Ų��.
            node.CurrentUnit = null;
        }
        


        
        if (!node.IsMove) 
        {
            node.IsTurn = false;
            Debug.Log($"�Ʊ��� ���� �����ൿ�� :{node.TurnActionValue}");
            MoveActionButton.IsMoveButtonClick = false; //�����Ƽ� ����ƽ

            node.TurnEndAction(); //�ϿϷ� ��������Ʈ�� �����Ѵ� .
        
        }


    }

}
