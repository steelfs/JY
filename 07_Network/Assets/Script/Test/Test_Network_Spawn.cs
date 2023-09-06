using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Network_Spawn : Net_TestBase
{
    public GameObject bullet;
    public Transform firePos;
    protected override void Test1(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            RequestSpawnServerRpc();
        }
    }


    [ServerRpc]
    void RequestSpawnServerRpc()
    {
        GameObject bullet_ = Instantiate(bullet, firePos);
        NetworkObject netObj = bullet_.GetComponent<NetworkObject>();
        netObj.Spawn();

    }


}
