using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    Ship selectedShip;//지금 배치할 배
    Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            selectedShip = value;
        }
    }

    //입력관련 델리게이트 .
    //상태별로 따로 처리 만약 null이면 그 상태에서 수행할 일이 없다는 의미 
    Action<Vector2>[] onClick;
    Action<Vector2>[] onMouseMove;
    Action<float>[] onMouseWheel;

    //함선 배치씬용 입력함수
    void Onclick_shipDeployment(Vector2 screen)
    {

    }
    void OnMouseMove_shipDeployment(Vector2 screen)
    {

    }
    void OnMouseWheel_shipDeployment(float wheelDelta)
    {

    }
    //함선 배치씬용 입력함수

    //전투씬용 입력함수
    void Onclick_Battle(Vector2 screen)
    {

    }
    void OnMouseMove_Battle(Vector2 screen)
    {

    }
    void OnMouseWheel_Battle(float wheelDelta)
    {

    }
    //전투씬용 입력함수


    public void SelectedShipToDeploy(ShipType shipType)//특정종류 함선을 선택하는 함수 
    {
        SelectedShip = ships[(int)shipType - 1];
    }

    public void UndoShipDeploy(ShipType shipType)//특정종류 함선을 배치 취소하는 함수 
    {

    }
}
