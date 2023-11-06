using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    /// <summary>
    /// 플레이어
    /// </summary>
    public Player Player => player;

    CinemachineVirtualCamera vcamera;


    /// <summary>
    /// 플레이어를 찍는 가상카메라
    /// </summary>
    public static CinemachineVirtualCamera VCamera => Inst.vcamera;

    // 나중에 풀에서 가져오는 것으로 변경
    public GameObject bulletHolePrefab;
    int killCount = 0;
    public int KillCount => killCount;

    ResultPanel resultPanel;
    public ResultPanel ResultPanel => resultPanel;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        resultPanel = FindAnyObjectByType<ResultPanel>();
        resultPanel.gameObject.SetActive(false);

        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
        

        player = FindAnyObjectByType<Player>();
        player.onDie += () =>
        {
            crosshair.gameObject.SetActive(false);
            resultPanel.Open(killCount, false, Time.timeSinceLevelLoad);
        };

        GameObject obj = GameObject.Find("PlayerFollowCamera");
        if(obj != null) 
        {
            vcamera = obj.GetComponent<CinemachineVirtualCamera>();
        }

        Goal goal = FindAnyObjectByType<Goal>();
        goal.on_GameClear += () =>
        {
            crosshair.gameObject.SetActive(false);
            resultPanel.Open(killCount, true, Time.timeSinceLevelLoad);
        };

        killCount = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }
 
    public void AddKillCount()
    {
        killCount++;
    }
}
