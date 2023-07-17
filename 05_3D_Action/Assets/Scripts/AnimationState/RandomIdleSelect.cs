using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelect : StateMachineBehaviour
{


    //OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex)) //layerIndex 레이어가 트렌지션 중이 아닐 때(바뀌는 중)
        {
            animator.SetInteger("IdleSelect", randomSelect());
        }

    }
    int randomSelect()
    {
        float randValue = Random.value;
        return randValue < 0.15f? 1 : randValue < 0.3f? 2 : randValue < 0.45f? 3 : randValue < 0.6f? 4 : 0;
    }

}
