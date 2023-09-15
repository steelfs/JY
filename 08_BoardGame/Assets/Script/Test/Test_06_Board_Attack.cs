using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Board_Attack : Test_05_ShipDeploy_Auto
{
    protected override void LClick(InputAction.CallbackContext obj)
    {
        base.LClick(obj);

        //클릭했을 때 targetShip이 없으면  보드에 공격
        // Board.OnAttack 함수 구현
        // 보드 밖이거나 이미 공격받았던 위치는 아무런 처리 없음
        //처음 공격받을 시 마크 표시
    }
}
