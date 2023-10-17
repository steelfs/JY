using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestFire : TestBase
{
    public GunBase gun;
    public float fireInterval;
    protected override void Test1(InputAction.CallbackContext context)
    {
        StartCoroutine(KeepFire());
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
    }
    IEnumerator KeepFire()
    {
        while (true)
        {
            gun.Fire();
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
