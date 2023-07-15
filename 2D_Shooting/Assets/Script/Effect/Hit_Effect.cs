using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Effect : PooledObject
{

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
    }


}
