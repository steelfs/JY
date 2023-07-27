using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemDrop : TestBase
{
    public ItemCode ItemCode;
    Transform testTransform;
    public bool randomNoise = true;
    public uint count = 3;

    private void Start()
    {
        testTransform = transform.GetChild(0);
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItem(ItemCode);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItem(ItemCode, testTransform.position, randomNoise);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItems(ItemCode, count);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        ItemFactory.MakeItems(ItemCode, count, testTransform.position, randomNoise);
    }
}
