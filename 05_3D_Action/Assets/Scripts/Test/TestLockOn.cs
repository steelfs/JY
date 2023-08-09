using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestLockOn : TestBase
{
    public Enemy[] dummys;
    Player player;
    private void Start()
    {
        
        
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        dummys[0].HP -= 1000;
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        dummys[1].HP -= 1000;
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        dummys[2].HP -= 1000;
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
  
        player.HP -= 1000;
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
        player = GameManager.Inst.Player;
        player.Inventory.AddItem(ItemCode.SilverSword);
    }
}
