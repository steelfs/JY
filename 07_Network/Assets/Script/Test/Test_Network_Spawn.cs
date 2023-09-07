using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Network_Spawn : Net_TestBase
{
    public GameObject bullet;
    public GameObject orb_;
    public Transform firePos;
    protected override void Test1(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            RequestSpawnServerRpc();
        }
    }


    protected override void Test2(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            RequestSpawnEnergyOrbServerRpc();
    
        }
    }
    [ServerRpc]
    void RequestSpawnServerRpc()
    {
        GameObject bullet_ = Instantiate(bullet, firePos);
        bullet_.transform.position = firePos.position;
        bullet_.transform.rotation = firePos.rotation;
        NetworkObject netObj = bullet_.GetComponent<NetworkObject>();
        netObj.Spawn();

    }


    [ServerRpc]
    void RequestSpawnEnergyOrbServerRpc()
    {
        GameObject orb = Instantiate(orb_, firePos);
        orb.transform.position = firePos.position;
        orb.transform.rotation = firePos.rotation;
        NetworkObject netObj = orb.GetComponent<NetworkObject>();
        netObj.Spawn();

    }
}
