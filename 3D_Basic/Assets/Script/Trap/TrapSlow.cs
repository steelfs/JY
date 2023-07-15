using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSlow : TrapBase
{
    public float slowDuration = 5.0f;
    public float slowRatio = 0.8f;
    protected override void OnTrapActivate(GameObject target)
    {
       // base.OnTrapActivate(target);
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.SetSpeedDebuff(1 - slowRatio);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();  
        if (player != null)
        {
            StartCoroutine(RestoreSpeed(player));
        }
    }
    IEnumerator RestoreSpeed(Player player)
    {
        yield return new WaitForSeconds(slowDuration);
        player.RestoreSpeed();
    }
}
