using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestLog : TestBase
{
    public Logger logger;
    public int i;
    private void Start()
    {
        logger.TestClear();
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        logger.Log(i.ToString());
        i++;

    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        logger.Log("aaa[bbb]ccc{ddd}eee");
        //logger.Log("aaa[bbbccc{ddd}eee");
        //logger.Log("aaa[bbb]ccc{dddeee");
        //logger.Log("aaa[bbbcccddd}eee");
    }
}
