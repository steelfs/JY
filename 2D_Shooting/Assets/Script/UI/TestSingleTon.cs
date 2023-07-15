using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSingleTon : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        SingleTonExample.Instance.testI = 11;
        Debug.Log(SingleTonExample.Instance.testI);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        SingleTonExample.Instance.testI = 12;
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        base.Test3(context);
    }
}
