using Cinemachine;
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
    FinishButton finishButton;
    CinemachineImpulseSource impulseSource;
    public CinemachineImpulseSource ImpulseSource => impulseSource;
    public UserPlayer UserPlayer => user;
    public EnemyPlayer EnemyPlayer => enemy;
    public FinishButton FinishButton => finishButton;

    //게임상태 ------------------------------------------------------------------------
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
    //게임상태 ------------------------------------------------------------------------

    // 배치상태 저장
    ShipDeployData[] shipDeployDatas;
    //

    //테스트용
    public bool IsTestMode = true;
    //테스트용
    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        input = GetComponent<InputController>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    protected override void OnInitialize()
    {
        user = FindAnyObjectByType<UserPlayer>();
        enemy = FindAnyObjectByType<EnemyPlayer>();

        onStateChange = null;
        if (user != null)
        {
            onStateChange += user.OnStateChange;
        }
        if (enemy != null)
        {
            onStateChange += enemy.OnStateChange;
        }



        //onStateChange = user.OnStateChange;
    }

    //모든배가 배치되어있을 때만 저장하도록
    public bool SaveShipDeployData(UserPlayer targetPlayer)// 배치된 함선 위치, 방향 저장
    {
        bool result = true;
        shipDeployDatas = new ShipDeployData[targetPlayer.Ships.Length];
        if (targetPlayer.IsAllDeployed)
        {
            for (int i = 0; i < shipDeployDatas.Length; i++)
            {
                Ship ship = targetPlayer.Ships[i];
                shipDeployDatas[i] = new ShipDeployData(ship.Direction, ship.Positions[0]);
                Debug.Log($"{ship.Direction}, {ship.Positions[0]}");
            }
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public bool LoadShipDeployData(UserPlayer targetPlayer)//저장되 데이터 불러오기
    {
        bool result = false;
        if (shipDeployDatas != null)
        {
            targetPlayer.UndoAllShipDeployment();
            for(int i = 0; i < shipDeployDatas.Length; i++)
            {
                Ship ship = targetPlayer.Ships[i];
                ShipDeployData shipDeployData = shipDeployDatas[i];
                ship.Direction = shipDeployDatas[i].Direction;//방향설정
                targetPlayer.Board.ShipDeployment(ship, shipDeployData.Position);// 배 생성
                ship.gameObject.SetActive(true);// 활성화

            }
            result = true;
        }
        //for (int i = 0; i < shipDeployDatas.Length; i++)
        //{
        //    Ship ship = UserPlayer.Ships[i];
        //    ShipDeployData shipDeployData = shipDeployDatas[i];
        //    shipDeployDatas[i] = new ShipDeployData(ship.Direction, ship.Positions[0]);
        //    Debug.Log($"{ship.Direction}, {ship.Positions[0]}");
        //    UserPlayer.Board.ShipDeployment(ship, shipDeployData.Position);
        //    ship.Direction = shipDeployData.Direction;
        //    ship.Rotate();
        //}

        return result;
    }
}
