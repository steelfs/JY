using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject gunCamera;
    
    GunBase gun;
    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;
        gun = gunCamera.GetComponentInChildren<GunBase>();
    }
    private void Start()
    {
        gun.Equip();
    }

    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }
    public void GunFire()
    {
        gun.Fire();
    }
}
