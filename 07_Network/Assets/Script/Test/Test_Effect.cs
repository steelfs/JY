using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Effect : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        NetPlayer player = GameManager.Inst.Player;
        player.Die();
    }
}
