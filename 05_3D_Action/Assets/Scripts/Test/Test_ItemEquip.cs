using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemEquip : TestBase
{
    private void Start()
    {
        ItemFactory.MakeItem(ItemCode.IronSword);
        ItemFactory.MakeItem(ItemCode.SilverSword);
        ItemFactory.MakeItem(ItemCode.OldSword);
        ItemFactory.MakeItem(ItemCode.RoundShield);
        ItemFactory.MakeItem(ItemCode.KnightShield);

    }
    protected override void Test1(InputAction.CallbackContext context)
    {
  
    }

}
