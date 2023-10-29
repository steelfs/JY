using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : TestBase
{
    public CellVisualizer cellVisualizer;
    
    protected override void Test1(InputAction.CallbackContext context)
    {
        Test();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        cellVisualizer.OnSet_Default_Material();
    }
    async void Test()
    {
        Debug.Log("Start_Test");
        TestDelay();
        
        Debug.Log("End_Test");
    }
    void TestDelay()
    {
        Debug.Log("Start_Delay");
        Debug.Log("End_Delay");
    }
}
