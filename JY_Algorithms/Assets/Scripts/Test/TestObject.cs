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
    protected override void Test2(InputAction.CallbackContext context)
    {
        int[] count = new int[4];
        for (int i = 0; i < 100000; i++)
        {
            Vector2Int[] positions = Util.Get_StartingPoints();
            Vector2Int chosen = positions[0];
            if (chosen.x == 0 && chosen.y == 0)
            {
                count[0]++;
            }
            else if (chosen.x == 9 && chosen.y == 0)
            {
                count[1]++;
            }
            else if (chosen.x == 0 && chosen.y == 9)
            {
                count[2]++;
            }
            else if (chosen.x == 9 && chosen.y == 9)
            {
                count[3]++;
            }
        }
        Debug.Log($"1 = {count[0]}, 2 = {count[1]}, 3 = {count[2]}, 4 = {count[3]} ");
    }
}
