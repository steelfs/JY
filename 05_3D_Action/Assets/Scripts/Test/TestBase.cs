using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour
{
   protected  PlayerInputAction inputActions;
    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }
    protected virtual void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += Test1;
        inputActions.Test.Test2.performed += Test2;
        inputActions.Test.Test3.performed += Test3;
        inputActions.Test.Test4.performed += Test4;
        inputActions.Test.Test5.performed += Test5;
        inputActions.Test.RightClick.performed += RightClick;
        inputActions.Test.LeftClick.performed += LeftClick;
    }


    protected virtual void OnDisable()
    {
        inputActions.Test.Test1.performed -= Test1;
        inputActions.Test.Test2.performed -= Test2;
        inputActions.Test.Test3.performed -= Test3;
        inputActions.Test.Test4.performed -= Test4;
        inputActions.Test.Test5.performed -= Test5;
        inputActions.Test.RightClick.performed -= RightClick;
        inputActions.Test.Disable();
    }

    protected virtual void Test1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }
    protected virtual void Test2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }
    protected virtual void Test3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }
    protected virtual void Test4(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }
    protected virtual void Test5(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }
    protected virtual void RightClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }

    private void LeftClick(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }
}
