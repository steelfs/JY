using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Board_Attack : Test_05_ShipDeploy_Auto
{
    protected override void LClick(InputAction.CallbackContext obj)
    {
        base.LClick(obj);

        //Ŭ������ �� targetShip�� ������  ���忡 ����
        // Board.OnAttack �Լ� ����
        // ���� ���̰ų� �̹� ���ݹ޾Ҵ� ��ġ�� �ƹ��� ó�� ����
        //ó�� ���ݹ��� �� ��ũ ǥ��
    }
}
