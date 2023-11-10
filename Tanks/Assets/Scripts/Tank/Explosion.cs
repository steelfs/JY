using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : PooledObject
{
    VisualEffect visualEffect;
    readonly int onExplosion_ID = Shader.PropertyToID("OnExplosion");
    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }
    public void Initialize(Vector3 position, Vector3 normal)
    {
        transform.position = position;
        transform.up = normal;
        visualEffect.SendEvent(onExplosion_ID);
        StartCoroutine(LifeOver(1.5f));
    }
}
