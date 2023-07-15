using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2 : EnemyBase
{
    Vector3 dir;
    float rangeX = 8.5f;
    float rangeY = 4.5f;
    public float intervalParameter;
    WaitForSeconds movingInterval;
    bool isMoving = true;
    protected override void Awake()
    {
        intervalParameter = 2.0f;
        movingInterval= new WaitForSeconds(intervalParameter);
        base.Awake();
        dir = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
    }
    protected override void OnMoveUpdate()
    {
        if (isMoving)
        {
            transform.Translate(Time.deltaTime * speed * (dir - transform.position));
            if (Vector3.Distance(transform.position, dir) < 0.001f)
            {
                isMoving = false;
                StartCoroutine(ResetDir());
            }
        }
   
    }
    IEnumerator ResetDir()
    {
        dir = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
        yield return movingInterval;
        isMoving = true;
    }
}
