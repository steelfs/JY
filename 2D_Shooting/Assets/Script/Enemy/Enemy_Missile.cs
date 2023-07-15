using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Missile : EnemyBase
{
    Transform target;
    bool onGuided = true;
  
    protected override void OnInitialize()
    {
        base.OnInitialize();
        target = GameManager.Inst.Player.transform;
        onGuided = true;
    }
    protected override void OnMoveUpdate()
    {
        base.OnMoveUpdate();
        if (onGuided)
        {
            Vector3 dir = target.position - transform.position;

            transform.right = -Vector3.Lerp(-transform.right, dir, Time.deltaTime * 0.5f);
        }       
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onGuided = false;
        }        
    }
}
