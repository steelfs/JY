using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCurve : EnemyBase
{
    [Header("Curve Enemy data")]
    public float rotateSpeed = 10.0f;
    float curveDir = 0.0f;
    float? startY = null;
 
    public float StartY
    {
        set
        {
            if (startY == null) //startY를 이니셜라이즈에서 null로 초기화하고  널일때만 방향지정
            {
                startY = value;
                if (startY > 0)
                {
                    curveDir = 1.0f; // 위에서 등장하면 아래로 커브, 우회전
                }
                else
                {
                    curveDir = -1.0f;// 아래에서 등장하면 위로 커브
                }
            }
   
        }
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        startY = null;

    }
    protected override void OnMoveUpdate()
    {
        base.OnMoveUpdate();
        transform.Rotate(Time.deltaTime * rotateSpeed * curveDir * Vector3.forward);
    }
}
