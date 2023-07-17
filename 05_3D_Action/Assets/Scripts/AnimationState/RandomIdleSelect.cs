using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelect : StateMachineBehaviour
{


    //OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex)) //layerIndex ���̾ Ʈ������ ���� �ƴ� ��(�ٲ�� ��)
        {
            animator.SetInteger("IdleSelect", randomSelect());
        }

    }
    int randomSelect()
    {
        return Random.value < 0.15f? 1 : Random.value < 0.3f? 2 : Random.value < 0.45f? 3 : Random.value < 0.6f? 4 : 0;
    }

}
