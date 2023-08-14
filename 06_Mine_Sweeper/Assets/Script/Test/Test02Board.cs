using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test02Board : TestBase
{
    public Board board;
    protected override void Test1(InputAction.CallbackContext context)
    {
        board.Initialize(15, 15, 15);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        board.TestResetBoard();
    }
}
