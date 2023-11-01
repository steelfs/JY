using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    protected PlayerInputActions inputActions;

    protected virtual void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    protected virtual void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += Test1;
        inputActions.Test.Test2.performed += Test2;
        inputActions.Test.Test3.performed += Test3;
        inputActions.Test.Test4.performed += Test4;
        inputActions.Test.Test5.performed += Test5;
        inputActions.Test.Test6.performed += Test6;
        inputActions.Test.Test7.performed += Test7;
        inputActions.Test.Test8.performed += Test8;
        inputActions.Test.Test9.performed += Test9;
        inputActions.Test.MouseLeft.performed += MouseLeft;
        inputActions.Test.MouseRight.performed += MouseRight;
    }

  

    protected virtual void OnDisable()
    {
        inputActions.Test.MouseLeft.performed -= MouseLeft;
        inputActions.Test.MouseRight.performed -= MouseRight;
        inputActions.Test.Test9.performed -= Test9;
        inputActions.Test.Test8.performed -= Test8;
        inputActions.Test.Test7.performed -= Test7;
        inputActions.Test.Test6.performed -= Test6;
        inputActions.Test.Test5.performed -= Test5;
        inputActions.Test.Test4.performed -= Test4;
        inputActions.Test.Test3.performed -= Test3;
        inputActions.Test.Test2.performed -= Test2;
        inputActions.Test.Test1.performed -= Test1;
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
    protected virtual void Test6(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {

    }
    protected virtual void Test7(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {

    }
    protected virtual void Test8(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {

    }
    protected virtual void Test9(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {

    }
    protected virtual void MouseRight(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
    }
    protected virtual void MouseLeft(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
    }

}
