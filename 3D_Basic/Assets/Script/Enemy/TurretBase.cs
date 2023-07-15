using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    public float fireInterval = 0.2f;
    public Pool_Object_Type projectileType = Pool_Object_Type.Bullet;

    protected Transform barrelBodyTransform;
    protected Transform fireTransform;

    protected IEnumerator fireCoroutine;

    protected virtual void Awake()
    {
        barrelBodyTransform = transform.GetChild(4);
        fireTransform = barrelBodyTransform.GetChild(1);
        fireCoroutine = PeriodFire();
    }
    IEnumerator PeriodFire()
    {
        while (true)
        {
            OnFire();
            yield return new WaitForSeconds(fireInterval);
        }       
    }
    protected virtual void OnFire()
    {
        Factory.Inst.GetObject(projectileType, fireTransform);
    }
}
