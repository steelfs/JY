using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : EnemyBase
{
    Vector3 dir;
    Vector3 targetPos;
    float rangeX = 8.5f;
    float rangeY = 4.5f;
    protected override void Awake()
    {
        base.Awake();
 
        dir = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
        targetPos = dir - transform.position;
    }
    protected override void OnMoveUpdate()
    {
        transform.Translate(Time.deltaTime * speed * targetPos);
        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            ResetTargetPos();
        }
   
    }
    void ResetTargetPos()
    {
        dir = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
    }
}
