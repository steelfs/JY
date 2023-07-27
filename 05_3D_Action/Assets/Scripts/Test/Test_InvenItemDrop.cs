using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_InvenItemDrop : TestBase
{
    public InventoryUI inventoryUI;
    Inventory inven;

    public Player player;

    uint size = 10;

    private void Start()
    {
        inven = new Inventory(player, size);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 1);
        inven.AddItem(ItemCode.Emerald, 2);
        inven.AddItem(ItemCode.Sapphire, 3);

        inven.PrintInventory();
        inventoryUI.InitializeInventory(inven);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {

    }

    protected override void Test2(InputAction.CallbackContext context)
    {
  
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
      
    }
    protected override void Test4(InputAction.CallbackContext context)
    {

    }
}
