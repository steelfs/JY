using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Ÿ��Ʋ���� ����� ��ư ���ӽ��� ȭ�� �������� ����
/// </summary>
public class NewGameButton: MonoBehaviour
{
    public void OnClickNewStart()
    {
        LoadingScene.SceneLoading(EnumList.SceneName.SpaceShip);
    }
}
