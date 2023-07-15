using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestTurret : TestBase
{
    Transform fire;
    private void Start()
    {
        fire = transform.GetChild(0);
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        Factory.Inst.GetObject(Pool_Object_Type.Bullet,fire.position);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        base.Test2(context);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        base.Test3(context);
    }

}
