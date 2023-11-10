using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Test04_Pool : TestBase
{
    public Transform fireTransform;
    protected override void Test1(InputAction.CallbackContext context)
    {
        Explosion obj = Factory.Inst.GetExplosion(Vector3.zero, Vector3.up);
        obj.Initialize(Vector3.zero, Vector3.up);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        Factory.Inst.GetShell(fireTransform);
    }
}
