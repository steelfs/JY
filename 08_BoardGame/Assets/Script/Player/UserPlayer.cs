using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    Ship selectedShip;//���� ��ġ�� ��
    Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            selectedShip = value;
        }
    }

    //�Է°��� ��������Ʈ .
    //���º��� ���� ó�� ���� null�̸� �� ���¿��� ������ ���� ���ٴ� �ǹ� 
    Action<Vector2>[] onClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    //�Լ� ��ġ���� �Է��Լ�
    void Onclick_shipDeployment(Vector2 screen)
    {

    }
    void OnMouseMove_shipDeployment(Vector2 screen)
    {

    }
    void OnMouseWheel_shipDeployment(float wheelDelta)
    {

    }
    //�Լ� ��ġ���� �Է��Լ�

    //�������� �Է��Լ�
    void Onclick_Battle(Vector2 screen)
    {

    }
    void OnMouseMove_Battle(Vector2 screen)
    {

    }
    void OnMouseWheel_Battle(float wheelDelta)
    {

    }
    //�������� �Է��Լ�


    public void SelectedShipToDeploy(ShipType shipType)//Ư������ �Լ��� �����ϴ� �Լ� 
    {
        SelectedShip = ships[(int)shipType - 1];
    }

    public void UndoShipDeploy(ShipType shipType)//Ư������ �Լ��� ��ġ ����ϴ� �Լ� 
    {

    }
}
