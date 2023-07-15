using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPush : TrapBase
{
    Animator anim;
    public float pushPower = 10.0f;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void OnTrapActivate(GameObject target)
    {
        //base.OnTrapActivate(target);
        anim.SetTrigger("Activate");

        Rigidbody targetRigid = target.GetComponent<Rigidbody>();
        Player player = target.GetComponent<Player>();
        if (player != null && targetRigid != null)
        {
            Vector3 dir = (transform.up - transform.forward).normalized;
            targetRigid.AddForce(pushPower * dir, ForceMode.Impulse);
            player.SetForceJumpMode();
        }
    }
}
