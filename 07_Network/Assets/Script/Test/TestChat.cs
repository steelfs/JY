using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestChat : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        NetworkObject obj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        GameManager.Inst.Log(obj.gameObject.name);
    }
}
