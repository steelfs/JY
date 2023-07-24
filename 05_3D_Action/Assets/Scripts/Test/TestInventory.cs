using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInventory : TestBase
{
    public uint size = 6;
    public ItemCode code = ItemCode.Ruby;
    public uint index = 2;

    Inventory inven;

    private void Start()
    {
        inven = new Inventory(null, size);
        inven.AddItem(code, 0);
        inven.AddItem(code, 0);
        inven.AddItem(code, 0);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Emerald, 1);
        inven.AddItem(ItemCode.Saphaire, 2);
        inven.AddItem(ItemCode.Saphaire, 2);
        inven.AddItem(ItemCode.Saphaire, 2);
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        inven.PrintInventory();
    
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        inven.AddItem(ItemCode.Emerald, index);
        
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        inven.RemoveItem(index);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        inven.ClearSlot(index);
    }
    protected override void Test5(InputAction.CallbackContext context)
    {
        inven.ClearInventory();
    }
}
