using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Capsule : TestBase
{
    float rotateSpeed = 5.0f;
    protected override void Test1(InputAction.CallbackContext context)
    {
        //transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
        //transform.LookAt(new Vector3(1, 0, 0));
        transform.Rotate(0, 45, 0);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void Update()
    {
        
    }
}
