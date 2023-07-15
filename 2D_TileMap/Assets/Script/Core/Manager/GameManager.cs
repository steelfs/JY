using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player; 

    WorldManager worldManager;// ���� ��ü ���� �����ϴ� �޴���
    public WorldManager WorldManager => worldManager;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize(); //�̱����� ����� ���� �� �ѹ��� ȣ��

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
