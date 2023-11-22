using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 타이틀에서 사용할 버튼 게임시작 화면 정해지면 수정
/// </summary>
public class NewGameButton: MonoBehaviour
{
    public void OnClickNewStart()
    {
        LoadingScene.SceneLoading(EnumList.SceneName.SpaceShip);
    }
}
