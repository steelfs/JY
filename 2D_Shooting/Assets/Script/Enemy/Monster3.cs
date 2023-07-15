using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster3 : EnemyBase
{
    public GameObject powerUpItem;
    Vector3 targetPosition;

    WaitForSeconds movingInterval = new WaitForSeconds(2.0f);
    bool isMoving = true;
    protected override void Awake()
    {
        base.Awake();       
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        targetPosition = new Vector3(Random.Range(-2.0f, 4.0f), Random.Range(4.5f, -4.5f), 0);
    }
    protected override void Die()
    {
        GameObject obj = Instantiate(powerUpItem);
        obj.transform.position = transform.position;
        base.Die();
    }
    protected override void OnMoveUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.002f)
            {
                isMoving = false;
                StartCoroutine(SetTargetPosition());              
            }
        }
 
    }
    IEnumerator SetTargetPosition()
    {

        targetPosition = new Vector3(Random.Range(-2.0f, 4.0f), Random.Range(4.5f, -4.5f), 0);
        yield return movingInterval;
        isMoving = true;
    }
}
