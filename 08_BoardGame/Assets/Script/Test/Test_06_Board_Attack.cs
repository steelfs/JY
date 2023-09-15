using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Board_Attack : Test_05_ShipDeploy_Auto
{
    protected override void Start()
    {
        base.Start();

    }

    protected override void LClick(InputAction.CallbackContext obj)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if (targetShip != null )
        {
            if (Board.shipDeployment(targetShip, world))
            {
                Debug.Log("��ġ ����");
                Target = null;
            }
            else
            {
                Debug.Log("��ġ ����");
            }        
        }
        else
        {
            Board.OnAttacked(Board.World_To_Grid(world));
        }

    }
}
