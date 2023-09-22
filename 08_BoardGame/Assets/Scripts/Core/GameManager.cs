using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState : byte
{
    Title,
    ShipDeployment,// 함선배치
    Battle,        // 배틀 
    GameEnd
}

[RequireComponent(typeof(InputController))]
public class GameManager : Singleton<GameManager>
{
    UserPlayer user;
    EnemyPlayer enemy;
    public UserPlayer UserPlayer => user;
    public EnemyPlayer EnemyPlayer => enemy;

    GameState gameState = GameState.Title;
    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState != value)
            {
                gameState = value;
                onStateChange?.Invoke(gameState);
            }
        }
    }
    public Action<GameState> onStateChange;
    InputController input;
    public InputController Input => input;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        input = GetComponent<InputController>();
    }
    protected override void OnInitialize()
    {
        user = FindAnyObjectByType<UserPlayer>();
        enemy = FindAnyObjectByType<EnemyPlayer>();

        onStateChange = null;
        //onStateChange = user.OnStateChange;
    }
}
