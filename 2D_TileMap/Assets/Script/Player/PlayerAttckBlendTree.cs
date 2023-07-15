using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttckBlendTree : StateMachineBehaviour
{

    Player player;
    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="animator">�� ���¸� ó���ϴ� �ִϸ�����</param>
    /// <param name="stateInfo">�ִϸ����� ���� ����</param>
    /// <param name="layerIndex"></param>
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.RestoreInputDir();
    }


}
