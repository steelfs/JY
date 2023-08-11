using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Image_Number : TestBase
{
    public int testNumber = 0;
    public ImageNumber imageNumber;
    Timer timer;
    private void Start()
    {
        timer = FindObjectOfType<Timer>();
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        GameManager.Inst.FlagCount++;
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        timer.Play();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        timer.Stop();
    }
    //    private void OnValidate()
    //    {
    //        imageNumber.Number = testNumber;
    //    }
}
