using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_05_EndProcess : TestBase
{
    public Transform shellTransform;
    protected override void Test1(InputAction.CallbackContext context)
    {
        Factory.Inst.GetShell(shellTransform);
    }
}
