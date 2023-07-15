using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player; 

    WorldManager worldManager;// 게임 전체 맵을 관리하는 메니저
    public WorldManager WorldManager => worldManager;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize(); //싱글톤이 만들어 질때 단 한번만 호출

        worldManager = GetComponent<WorldManager>();
        worldManager.PreInitialize();
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindObjectOfType<Player>();
        worldManager.Initialize();
    }
}
