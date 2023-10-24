using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reLoadTime = 1;
    WaitForSeconds reLoadWait;

    public bool isReLoading = false;
    protected override void FireProcess()
    {
        Ray ray = new(fireTransform.position, GetFireDirection());
        if( Physics.Raycast(ray, out RaycastHit hitInfo, range) )
        {
            Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
            Factory.Inst.GetBulletHole(hitInfo.point, hitInfo.normal,reflect);
            //bulletHole.transform.position = hitInfo.point;
            //bulletHole.transform.forward = -hitInfo.normal;
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
