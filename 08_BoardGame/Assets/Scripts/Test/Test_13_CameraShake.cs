using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_13_CameraShake : TestBase
{
    public CinemachineImpulseSource source;

    protected override void Test1(InputAction.CallbackContext context)
    {
        source.GenerateImpulse();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        source.GenerateImpulseAtPositionWithVelocity(transform.position, Vector3.right);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        source.GenerateImpulseWithForce(10);
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        source.GenerateImpulseWithVelocity(Vector3.right + Vector3.up);
    }

    // ���ݴ��� �� ������ �������� shake
    // �谡 ħ���ϸ� �� ũ�� ��鸰��.
}
