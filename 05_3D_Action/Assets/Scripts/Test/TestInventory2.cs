using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInventory2 : TestBase
{
    public uint size = 6;
    public ItemCode code = ItemCode.Ruby;
    public uint index = 2;

    Inventory inven;

    private void Start()
    {
        inven = new Inventory(null, size);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 0);
        inven.AddItem(ItemCode.Ruby, 1);
        inven.AddItem(ItemCode.Ruby, 1);
        inven.AddItem(ItemCode.Ruby, 2);
        inven.AddItem(ItemCode.Ruby, 3);
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        inven.PrintInventory();

    }
}
