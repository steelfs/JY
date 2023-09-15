using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Board_Attack : Test_05_ShipDeploy_Auto
{
    protected override void LClick(InputAction.CallbackContext obj)
    {
        base.LClick(obj);
        Vector2Int gridPos = Board.Get_Mouse_Grid_Pos();
        if (Board.Is_In_Board(gridPos) && targetShip == null)
        {
            Board.OnAttacked(gridPos);
        }


       // int index = Board.Grid_To_Index(gridPos);
       //if (targetShip == null && Board.Is_In_Board(gridPos))
       //{
       //    if (Board.OnAttacked(gridPos))
       //    {
       //        if (!Board.IsAttacked[index])
       //        {
       //            bombMark.SetBombMark(Board.Grid_To_World(gridPos), true);
       //            Board.IsAttacked[index] = true;
       //        }
       //        else
       //        {
       //            Debug.Log("이미 공격받은 자리 입니다.");
       //        }
       //    }
       //    else
       //    {
       //        if (!Board.IsAttacked[index])
       //        {
       //            bombMark.SetBombMark(Board.Grid_To_World(gridPos), false);
       //            Board.IsAttacked[index] = true;
       //        }
       //        else
       //        {
       //            Debug.Log("이미 공격받은 자리 입니다.");
       //        }
       //    }
       //}
       // 클릭했을 때 targetShip이 없으면  보드에 공격
       // BombMark 클래스 구현
       // Board.OnAttack 함수 구현
       // 보드 밖이거나 이미 공격받았던 위치는 아무런 처리 없음
       // 처음 공격받을 시 마크 표시
    }
}
