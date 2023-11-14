using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : TestBase
{
    public Vector2Int grid;
    public int amount;
    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.ToolBox.IncreaseCash(amount);
    }
    bool IsNearByPlayer(Vector2Int spawnGridPosition)
    {
        bool result = false;
        Vector2Int playerGridPos = Util.WorldToGrid(GameManager.Player.transform.position);
        int diffX = playerGridPos.x - spawnGridPosition.x;
        int diffY = playerGridPos.y - spawnGridPosition.y;
        if ((diffX < 3 && diffX > -3) && (diffY < 3 && diffY > -3))
        {
            result = true;
        }

        return result;
    }
}
