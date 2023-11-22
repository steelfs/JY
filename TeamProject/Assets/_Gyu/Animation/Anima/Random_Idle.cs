using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Idle : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))   // layerIndex ���̾ Ʈ������ ���� �ƴ� ��
        {
            animator.SetInteger("Selected", RandomSelect());
        }
    }

    int RandomSelect()
    {
        int select = 0;

        float num = Random.value;
        if (num < 0.75f)
        {
            select = 0;
        }
        else
        {
            select = 1;
        }

        return select;
    }
}
