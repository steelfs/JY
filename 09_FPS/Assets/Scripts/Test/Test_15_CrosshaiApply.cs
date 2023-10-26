using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_15_CrosshaiApply : TestBase
{
    public GunType gunType;

    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.Inst.Player.GunChange(GunType.Revoler);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        GameManager.Inst.Player.GunChange(GunType.Shotgun);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        GameManager.Inst.Player.GunChange(GunType.AssaultRifle);
    }


}
