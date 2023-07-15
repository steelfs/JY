using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : WayPointUser
{
    public Action<Vector3> onMove; //위치 이동했다고 알리는 델리게이트
    protected override void OnMove()
    {
        base.OnMove();
        onMove?.Invoke(moveDelta);
    }

}
