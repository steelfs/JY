using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : ObjectPool<Slime>
{
    protected override void OnGenerateObjects(Slime comp)
    {
        comp.Pool = comp.transform.parent; // 积己等 钱 历厘
    }
}
