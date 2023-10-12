using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{


    protected override void FireProcess()
    {
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, range))
        {
            Instantiate(GameManager.Inst.bulletHolePrefab, hitinfo.point, Quaternion.identity);
        }
    }

}
