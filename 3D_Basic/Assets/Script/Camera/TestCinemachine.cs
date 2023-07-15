using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCinemachine : TestBase
{
    public CinemachineVirtualCamera[] vcams;
    private void Start()
    {
        if (vcams == null)
        {
            vcams = FindObjectsOfType<CinemachineVirtualCamera>();
        }
    }
    void ResetCam()
    {
        foreach( var vcam in vcams)
        {
            vcam.Priority = 10;// Priority =우선순위 
        }
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        ResetCam();
        vcams[0].Priority = 100;
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        ResetCam();
        vcams[1].Priority = 100;
    }
}
