using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_04GameManager : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.Inst.GameReset();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        GameManager.Inst.GameOver();
    }
}
