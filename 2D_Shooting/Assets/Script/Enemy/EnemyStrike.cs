using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrike : EnemyBase
{
    [Header("Rush Enemy data")]
    public float secondSpeed = 10.0f; //돌진 스피드
    public float appearTime = 1f;// 등장 시간
    public float waitTime = 5.0f;// 등장 후 대기시간


    protected override void OnInitialize()
    {
        base.OnInitialize();
        StopAllCoroutines();
        StartCoroutine(AppearProcess());
    }

    IEnumerator AppearProcess()
    {
        yield return new WaitForSeconds(appearTime);
        speed = 0;
        yield return new WaitForSeconds(waitTime);
        speed = secondSpeed;
    }
    protected override void Die()
    {
        Factory.Inst.GetObject(Pool_Object_Type.PowerUp,transform.position);
        base.Die();
    }
}
