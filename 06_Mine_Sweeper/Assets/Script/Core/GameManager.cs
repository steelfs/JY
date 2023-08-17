using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //게임 상태관련 -----------------------------------------------------------------------
    public enum GameState
    {
        Ready = 0, // 게임 시작 전 (첫번째 셀이 아직 열리지 않은 상태)
        Play, // 진행중(첫번째 셀이 열린 이후, 또는 깃발이 설치된 이후 )
        GameClear, //모든 지뢰를 찾았을 때
        GameOver// 지뢰가 있는 셀을 열었을 때
    }

    GameState state = GameState.Ready;
    public GameState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case GameState.Ready:
                        onGameReady?.Invoke();
                        break;
                    case GameState.Play:
                        onGamePlay?.Invoke();
                        break;
                    case GameState.GameClear:
                        onGameClear?.Invoke();
                        break;
                    case GameState.GameOver:
                        onGameOver?.Invoke();
                        break;
                }
            }
            Debug.Log($"현재상태 : {state}");
        }
    }

    public Action onGameReady; // 게임 초기화시
    public Action onGamePlay;
    public Action onGameClear;
    public Action onGameOver;
    //게임 상태관련 -----------------------------------------------------------------------


    //보드 관련 ----------------------------------------------------------------------------------------------
    Board board;
    public Board Board => board;

    public int mineCount = 20;
    public int boardWidth = 15;
    public int boardHeight = 15;
    //보드 관련 ----------------------------------------------------------------------------------------------

    //깃발개수 관련 -----------------------------------------------------------------
    int flagCount = 0;
    public int FlagCount
    {
        get => flagCount; 
        set//private
        {
            flagCount = value;
            onFlagCountChange?.Invoke(flagCount);
        }
    }

    public Action<int> onFlagCountChange;
    //깃발개수 관련 End -----------------------------------------------------------------

    protected override void OnInitialize()
    {
        FlagCount = mineCount;
        board = FindObjectOfType<Board>();
        board.Initialize(boardWidth, boardHeight, mineCount);// 보드 초기화 
    }
    public void IncreaseFlagCount()
    {
        FlagCount++;
    }
    public void DecreaseFlagCount()
    {
        FlagCount--;
    }
    public void GameStart()
    {
        if (state == GameState.Ready)
        {
            State = GameState.Play;
        }
    }

    // 테스트 코드-------------------------------------------------------------------------
    public void TestFlag(int flag)
    {
        FlagCount = flag;
    }
    public void TestState(GameState state)
    {
        switch (state)
        {
            case GameState.Ready:
                onGameReady?.Invoke();
                break;
            case GameState.Play:
                onGamePlay?.Invoke();
                break;
            case GameState.GameClear:
                onGameClear?.Invoke();
                break;
        }
    }
}
