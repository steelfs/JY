using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    GameObject gunCamera;

    GunBase activeGun;
    GunBase defaultGun;
    GunBase[] guns;
    public GunBase[] Guns => guns;
    public GunBase Gun => activeGun;
    StarterAssets.FirstPersonController controller;

    public IEnumerator rifleFire;

    Transform fireTransform;
    public Transform FireTransform => fireTransform;
   
    public void SetBulletCountChangeDelegate(Action<int> callBack)
    {
        defaultGun.on_BulletCountChange = callBack;
        foreach (var gun in guns)
        {
            gun.on_BulletCountChange = callBack;
            gun.on_BulletCountChange += OnAmmoDepleted;
        }
    }
    void OnAmmoDepleted(int ammo)
    {
        if (ammo <= 0 && activeGun != defaultGun)
        {
            GunChange(GunType.Revolver);
        }
    }
    private void Awake()
    {
        gunCamera = transform.GetChild(2).gameObject;
        activeGun = GetComponentInChildren<GunBase>();

        fireTransform = transform.GetChild(0);
        controller = GetComponent<StarterAssets.FirstPersonController>();
        rifleFire = RifleFireCoroutine();
        defaultGun = transform.GetChild(3).GetComponent<GunBase>();

        activeGun = defaultGun;

        guns = transform.GetChild(4).GetComponentsInChildren<GunBase>(true);
        foreach (var gun in guns)
        {

        }
    }

    private void Start()
    {
        Crosshair crosshair = FindAnyObjectByType<Crosshair>();

        activeGun.Equip();
        activeGun.on_FireRecoil += GunFireRecoil;
        activeGun.on_FireRecoil += (expand) => crosshair.Expend(expand * 10);

        foreach(var gun in guns)
        {
            gun.on_FireRecoil += GunFireRecoil;
            gun.on_FireRecoil += (expand) => crosshair.Expend(expand * 10);
        }

    }

    private void GunFireRecoil(float recoil)
    {
        controller.fireRecoil(recoil);
    }

    /// <summary>
    /// 총 용 카메라 활성화 설정
    /// </summary>
    /// <param name="show">true면 총이 보인다., flase면 총이 안보인다.</param>
    public void ShowGunCamera(bool show = true)
    {
        gunCamera.SetActive(show);
    }

    public void GunFire(bool isFireStart = true)
    {
        activeGun.Fire(isFireStart);
    }

    IEnumerator RifleFireCoroutine()
    {
        while (true)
        {
            activeGun.Fire();
            yield return null;
        }
    }
    public void RevolverReLoad()
    {
        Revolver revolver = activeGun as Revolver;
        if (revolver != null)
        {
            revolver.ReLoad();
        }
    }

    public void GunChange(GunType type)
    {
        switch (type)
        {
            case GunType.Revolver:
                activeGun = defaultGun;
                activeGun.BulletCount = activeGun.clipSize;
                defaultGun.gameObject.SetActive(true);
                foreach(var gun in guns)
                {
                    gun.gameObject.SetActive(false);
                }
                break;
            case GunType.Shotgun:
                activeGun = guns[(int)GunType.Shotgun - 1];
                activeGun.BulletCount = activeGun.clipSize;
                activeGun.gameObject.SetActive(true);
                foreach (var gun in guns)
                {
                    if (gun != activeGun)
                    {
                        gun.gameObject.SetActive(false);
                    }
                }
                break;
            case GunType.AssaultRifle:
                activeGun = guns[(int)GunType.AssaultRifle - 1];
                activeGun.BulletCount = activeGun.clipSize;
                activeGun.gameObject.SetActive(true);
                foreach (var gun in guns)
                {
                    if (gun != activeGun)
                    {
                        gun.gameObject.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }
}
