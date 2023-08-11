using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Counter : TestBase
{
    public TimeCounter counter;
    public GameManager.GameState state = GameManager.GameState.Ready;

    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.Inst.TestFlag(GameManager.Inst.FlagCount + 1);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        GameManager.Inst.TestFlag(GameManager.Inst.FlagCount - 1);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        GameManager.Inst.TestState(state);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
    }
}
