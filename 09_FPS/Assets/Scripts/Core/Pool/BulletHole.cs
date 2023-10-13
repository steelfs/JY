using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletHole : PooledObject
{
    VisualEffect effect;
    readonly int durationId = Shader.PropertyToID("Duration");
    readonly int positionId = Shader.PropertyToID("SpawnPosition");
    readonly int spawnNormalId = Shader.PropertyToID("SpawnNormal");
    readonly int OnStartEvent = Shader.PropertyToID("OnStart");
    float duration;

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
        duration = effect.GetFloat(durationId);
    }
    private void OnEnable()
    {

        StartCoroutine(LifeOver(duration));
    }
  
    public void Initialize(Vector3 position, Vector3 normal)
    {
        effect.SetVector3(positionId, position);
        transform.position = position;
        transform.forward = normal;

        effect.SetVector3(spawnNormalId, normal);

        effect.SendEvent(OnStartEvent);
    }


}
