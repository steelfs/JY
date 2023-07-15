using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : TrapBase
{
    ParticleSystem ps;
    private void Awake()
    {
        Transform child = transform.GetChild(1);
        ps = child.GetComponent<ParticleSystem>();
        ps.Stop(); 
    }
    protected override void OnTrapActivate(GameObject target)
    {
        ps.Play();
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.Die();
        }
    }
    IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(5.0f);
        ps.Stop();
    }
}
