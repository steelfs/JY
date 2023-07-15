using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSlimeSpawn : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        Slime[] slime = FindObjectsOfType<Slime>();
        foreach (Slime sl in slime)
        {
            sl.Die();
        }
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        Slime slime = FindObjectOfType<Slime>();
        slime.Die();
    }
}
