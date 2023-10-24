using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject gunCamera;

    GunBase gun;
    public GunBase Gun => gun;

    public Action<int> on_BulletCountChange
    {
        get => gun.on_BulletCountChange;
        set => gun.on_BulletCountChange = value;
    } 
    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;
        gun = GetComponentInChildren<GunBase>();

       //MeshRenderer render = gun.transform.GetChild(0).GetComponent<MeshRenderer>();
       //render.enabled = false;
    }

    private void Start()
    {
        gun.Equip();
    }

    /// <summary>
    /// 총 용 카메라 활성화 설정
    /// </summary>
    /// <param name="show">true면 총이 보인다., flase면 총이 안보인다.</param>
    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }

    public void GunFire()
    {
        gun.Fire();
    }
    public void RevolverReLoad()
    {
        Revolver revolver = gun as Revolver;
        if (revolver != null)
        {
            revolver.ReLoad();
        }
    }
}
