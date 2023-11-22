using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHitMotion : StateMachineBehaviour
{
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger("Selected", Random.Range(0, 3));
    }

}
