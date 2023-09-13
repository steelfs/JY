using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03ShipMake : TestBase
{
    public ShipType shipType = ShipType.Carrier;
    public Board board;

    protected override void LClick(InputAction.CallbackContext obj)
    {
        Ship ship = ShipManager.Inst.MakeShip(shipType, null);

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(mousePos);// 카메라의 nearPlan 의 위치
        Vector2Int position = board.World_To_Grid(world);

        Vector3 newPos = new Vector3(position.x + 0.5f, 0, -position.y -(0.5f));
        Transform shipTransform = ship.transform;
        shipTransform.position = newPos;
    }
}
