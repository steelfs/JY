using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 게임종료 버튼 
/// </summary>
public class ExitButton : MonoBehaviour
{

    public void OnClickExit()
    {
        //LoadingScene.SceneLoading(EnumList.SceneName.BattleShip);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit( 0 );

#endif
    }
}
