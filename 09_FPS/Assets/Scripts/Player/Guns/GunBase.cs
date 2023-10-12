using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunBase : MonoBehaviour
{
    public float range;
    public float damage;
    public float fireRate;
    public int clipSize;
    public int bulletRemain;
    public float spread; //Åº ÆÛÁö´Â °¢µµ
    public float recoil;// ÃÑ ¹Ýµ¿

    VisualEffect muzzleEffect;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
    }

    public void Fire()
    {
        if (bulletRemain > 0)
        {
            muzzleEffect.Play();
            bulletRemain--;

            FireProcess();
        }
    }

    protected virtual void FireProcess()
    {

    }


    protected Transform fireTransform;
    public void Equip()
    {
        fireTransform = GameManager.Inst.Player.transform.GetChild(0);

    }




}
