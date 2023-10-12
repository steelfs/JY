using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject gunCamera;
    
    GunBase gunBase;
    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;
        gunBase = gunCamera.GetComponentInChildren<GunBase>();
    }

    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }
    public void GunFire()
    {
        gunBase.Fire();
    }
}
