using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Board board;//�� �÷��̾��� ���� 
    public Board Board => board;

    protected Ship[] ships;//�� �÷��̾ �����ִ� �Լ��� //
    public Ship[] Ships => ships;

    protected int remainShipCount;//���� �� ī��Ʈ
    public bool IsDefeat => remainShipCount < 1;

    bool isActionDone = false;//���� ���� �Ǿ��ִ��� ���� 
    public bool IsActionDone => isActionDone;

    protected PlayerBase opponent;//���� 

    public Action<PlayerBase> onAttackFail;//�� �÷��̾��� ������ ���������� �˸��� ��ȣ param = �ڱ� �ڽ�
    public Action<PlayerBase> onDefead;// �� �÷��̾ �й������� �˸��� ��ȣ 
    public Action onActionEnd;

    //�� ������ �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    public virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }
    public virtual void OnPlayerTurnEnd()
    {
    }
    //�� ������ �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    //���� ���� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    public void Attack(Vector2Int attackGridPos)//
    {

    }
    public void Attack(int index)
    {

    }
    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.World_To_Grid(worldPos));
    }
    
    public void AutoAttack()//CPU, �ΰ� �÷��̾ Ÿ�Ӿƿ� ���� �� ��� 
    {

    }
    //���� ���� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    //�Լ� ��ġ�� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    public void AutoShipDeployment(bool isShowShips)//�� �÷��̾��� ���忡 �Լ��� ��ġ�ϴ� �Լ� 
    {

    }
    private List<int> GetShipAroundPosition(Ship ship)//�Լ� �ֺ��� �ε������� ���ϴ� �Լ� 
    {
        return null;
    }
    public void UndoAllShipDeployment()//����Լ��� ��ġ�� ����ϴ� �Լ� 
    {

    }
    //�Լ� ��ġ�� �Լ��ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�


    //�Լ� ħ�� �� �й�ó�� �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    void OnShipDestroy(Ship ship)//���� ���� Ư���谡 ħ������ �� ����� �Լ� 
    {

    }

    void OnDefeat()//��� �谡 ħ���Ǿ��� �� ����� �Լ� 
    {

    }

    //��Ÿ�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    public void Clear()//�ʱ�ȭ . ���ӽ��� ���� ���·� ���� 
    {

    }

    //��Ÿ�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

}
