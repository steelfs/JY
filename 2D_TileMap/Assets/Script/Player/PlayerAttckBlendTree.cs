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
    /// <param name="animator">이 상태를 처리하는 애니메이터</param>
    /// <param name="stateInfo">애니메이터 관련 정보</param>
    /// <param name="layerIndex"></param>
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.RestoreInputDir();
    }


}
