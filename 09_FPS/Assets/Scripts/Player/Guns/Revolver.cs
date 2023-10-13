using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Revolver : GunBase
{
    protected override void FireProcess()
    {
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, range))
        {
            // GameObject obj = Instantiate(GameManager.Inst.bulletHolePrefab, hitinfo.point, Camera.main.transform.rotation);
            BulletHole hole = Factory.Inst.GetbulletHole();
            //hole.transform.forward = -hitinfo.normal;
            hole.Initialize(hitinfo.point, hitinfo.normal);
        }
    }

}
