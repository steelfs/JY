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
       //            Debug.Log("�̹� ���ݹ��� �ڸ� �Դϴ�.");
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
       //            Debug.Log("�̹� ���ݹ��� �ڸ� �Դϴ�.");
       //        }
       //    }
       //}
       // Ŭ������ �� targetShip�� ������  ���忡 ����
       // BombMark Ŭ���� ����
       // Board.OnAttack �Լ� ����
       // ���� ���̰ų� �̹� ���ݹ޾Ҵ� ��ġ�� �ƹ��� ó�� ����
       // ó�� ���ݹ��� �� ��ũ ǥ��
    }
}
