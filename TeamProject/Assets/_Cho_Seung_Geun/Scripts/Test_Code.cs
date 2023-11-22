using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Code : MonoBehaviour
{
    private void Awake()
    {
        int data = 11;
        int a = data & 8;
        bool result = false;
        if (a == 1 << 3)
        {
            result = true;
        }

        Debug.Log(Convert.ToString(data, 2));
        Debug.Log(a);
        Debug.Log(1 << 3);
        Debug.Log(result);
    }
}
