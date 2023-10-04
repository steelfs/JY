using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test14_ResultPanel : TestBase
{
    public ResultBoard board;
    protected override void Test1(InputAction.CallbackContext context)
    {
        board.SetVictory();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        board.SetDefeat();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        board.Open();
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        board.Close();
    }
}
