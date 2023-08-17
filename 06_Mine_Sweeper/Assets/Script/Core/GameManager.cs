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

    public bool IsPlaying => State == GameState.Play;//게임이 진행중인지 아닌지 확인하기 위한 프로퍼티

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
    public int startFlagCount = 10;
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
    public void FinishPlayerAction()
    {
        if (board.IsBoardClear)
        {
            GameClear();
        }
    }
    public void GameStart()
    {
        if (state == GameState.Ready)
        {
            State = GameState.Play;
        }
    }
    public void GameReset()
    {
        //게임 초기화 하기
        // 보드, 타이머, 플레그카운터
        FlagCount = mineCount;
        if (state == GameState.GameClear || state == GameState.GameOver)
        {
            State = GameState.Ready;
        }
    }
    public void GameOver()
    {
        State = GameState.GameOver;
        //타이머 정지,
        //셀이 더이상 열리지 않기(플레이 상태일때만 열리게 하기)
    }

    public void GameClear()
    {
        State = GameState.GameClear;
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
