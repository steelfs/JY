using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BitMasking : TestBase
{
    public int key;
    protected override void Test1(InputAction.CallbackContext context)
    {
        int data = Random.Range(0, 100);
        bool result = IsOwner(data);
    }

    bool IsOwner(int data)
    {
        transform.SetParent(transform);
        bool result = false;
        int key = 8;
        if ((data & key) == key)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    // 0000 1000
    // 0000 1111
    // 0001 0000
    //23 false, 24 true
}
