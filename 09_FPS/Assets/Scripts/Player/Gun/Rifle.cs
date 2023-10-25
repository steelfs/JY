using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : GunBase
{
    protected override void FireProcess(bool isFireStart = true)
    {
        if (isFireStart)
        {
            StartCoroutine(FireRepeat());
        }
        else
        {
            StopAllCoroutines();
        }
        //StartCoroutine(FireReady());
    }
    IEnumerator FireRepeat()
    {
        while(BulletCount > 0)
        {
            MuzzleEffect();
            BulletCount--;

            Ray ray = new(GameManager.Inst.Player.FireTransform.position, GetFireDirection());
            if (Physics.Raycast(ray, out RaycastHit hitInfo, range))
            {
                Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
                Factory.Inst.GetBulletHole(hitInfo.point, hitInfo.normal, reflect);
            }

            FireRecoil();
            yield return new WaitForSeconds(1 / fireRate);
        }

        isFireReady = true;
    }
}
