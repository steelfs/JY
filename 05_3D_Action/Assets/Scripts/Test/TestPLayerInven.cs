using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPLayerInven : TestBase
{
    public ItemCode ItemCode = ItemCode.Ruby;
    protected override void Test1(InputAction.CallbackContext context)
    {
        int index = UnityEngine.Random.Range(0, GameManager.Inst.ItemData.length);
        ItemCode code = GameManager.Inst.ItemData[index].code;

      
        Vector3 pos = UnityEngine.Random.insideUnitSphere * 5;
        pos.y = 0;
        ItemFactory.MakeItem(code, pos, true);

        
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        Player player = GameManager.Inst.Player;
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Ruby);
        player.Inventory.AddItem(ItemCode.Emerald);
        player.Inventory.AddItem(ItemCode.Emerald);
        player.Inventory.AddItem(ItemCode.Emerald);
        player.Inventory.AddItem(ItemCode.Emerald);
        player.Inventory.AddItem(ItemCode.Emerald);
        player.Inventory.AddItem(ItemCode.Sapphire);
        player.Inventory.AddItem(ItemCode.Sapphire);
        player.Inventory.AddItem(ItemCode.Sapphire);

    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        Vector3 pos = UnityEngine.Random.insideUnitSphere * 2;
        pos.y = 0;
        ItemFactory.MakeItem(ItemCode,pos,true);
    }
}
