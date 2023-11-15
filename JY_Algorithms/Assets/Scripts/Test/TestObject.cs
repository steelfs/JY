using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : TestBase
{
    public Vector3 world;
    public int amount;
    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.ToolBox.IncreaseCash(amount);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        Vector2Int result = Util.WorldToGrid(world);
        Debug.Log(result);
    }
}
