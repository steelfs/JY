using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reLoadTime = 1;
    WaitForSeconds reLoadWait;

    public bool isReLoading = false;
    protected override void FireProcess(bool isFireStart = true)
    {
        base.FireProcess();
        Ray ray = new(GameManager.Inst.Player.FireTransform.position, GetFireDirection());
        if( Physics.Raycast(ray, out RaycastHit hitInfo, range) )
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                HitEnemy(enemy);
            }
            else
            {
                Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
                Factory.Inst.GetBulletHole(hitInfo.point, hitInfo.normal,reflect);
            }
        }

        FireRecoil();
    }

    public void ReLoad()
    {
        if (!isReLoading)
        {
            isReLoading = true;
            isFireReady = false;
            StartCoroutine(ReLoadCoroutine());
        }
    }

    IEnumerator ReLoadCoroutine()
    {
        yield return new WaitForSeconds(reLoadTime);
        isFireReady = true;
        BulletCount = clipSize;
        isReLoading = false;
    }
}
