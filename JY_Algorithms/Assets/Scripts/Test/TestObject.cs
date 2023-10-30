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
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        cellVisualizer.OnSet_Default_Material();
    }
  
}
