using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PooledObject
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StopAllCoroutines();
        StartCoroutine(LifeOver(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }
 


}
