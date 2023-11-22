using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_deathPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Button title_Button;
    Button lobby_Button;

    public Action on_InitDeadCam;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        title_Button = transform.GetChild(0).GetComponent<Button>();
        lobby_Button = transform.GetChild(1).GetComponent<Button>();
        title_Button.onClick.AddListener(MoveToTitle);
        lobby_Button.onClick.AddListener(MoveToLobby);
    }

    public void Activate_DeathPanel()
    {
        StartCoroutine(PopupPanel());
    }
    IEnumerator PopupPanel()
    {
        float waitTime = 0;
        while (waitTime < 3.0f)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        float maxAlpha = 0.8f;
        float showTime = 2;
        float increase = maxAlpha / showTime;
        while(canvasGroup.alpha < maxAlpha)
        {
            canvasGroup.alpha += increase * Time.deltaTime;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    void MoveToTitle()
    {
        DeActivate_DeathPanel();
        LoadingScene.SceneLoading(EnumList.SceneName.TITLE);
    }
    void MoveToLobby()
    {
        DeActivate_DeathPanel();
        LoadingScene.SceneLoading(EnumList.SceneName.SpaceShip);
    }
    void DeActivate_DeathPanel()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        on_InitDeadCam?.Invoke();
        SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();
    }

}
