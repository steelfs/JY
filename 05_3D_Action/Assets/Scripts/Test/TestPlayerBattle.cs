using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerBattle : TestBase
{
    Player player;
    private void Start()
    {


   
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        player = GameManager.Inst.Player;
        ItemFactory.MakeItem(ItemCode.IronSword);
        ItemFactory.MakeItem(ItemCode.SilverSword);
        ItemFactory.MakeItem(ItemCode.OldSword);
        ItemFactory.MakeItem(ItemCode.RoundShield);
        ItemFactory.MakeItem(ItemCode.KnightShield);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
    
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        player.defence(75.0f);
    }
}
