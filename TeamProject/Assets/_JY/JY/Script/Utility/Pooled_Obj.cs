using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooled_Obj : MonoBehaviour
{
    public Action<Pooled_Obj> on_ReturnPool;
    ParticleSystem ps;

    public int poolIndex { get; set; }
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        
    }
    private void OnDisable()
    {
        //Debug.Log($"{name}");
        //on_ReturnPool?.Invoke(this);
        //GameManager.EffectPool.ReturnPool(this);
    }
}
