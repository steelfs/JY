using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    public int pellet = 6;

    protected override void FireProcess(bool isFireStart = true)
    {
        base.FireProcess();
        for (int i = 0; i < pellet; i++)
        {
            Ray ray = new(GameManager.Inst.Player.FireTransform.position, GetFireDirection());
            if (Physics.Raycast(ray, out RaycastHit hitInfo, range))
            {
                Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
                Factory.Inst.GetBulletHole(hitInfo.point, hitInfo.normal, reflect);
                //bulletHole.transform.position = hitInfo.point;
                //bulletHole.transform.forward = -hitInfo.normal;
            }
        }
        FireRecoil();
    }
    
}
