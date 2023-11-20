using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : TestBase
{
    public ItemSpawner Spawner;
    protected override void Test1(InputAction.CallbackContext context)
    {
        Spawner.TestCounter();
    }
}
