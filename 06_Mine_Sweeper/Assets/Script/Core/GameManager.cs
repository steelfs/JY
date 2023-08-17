using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //���� ���°��� -----------------------------------------------------------------------
    public enum GameState
    {
        Ready = 0, // ���� ���� �� (ù��° ���� ���� ������ ���� ����)
        Play, // ������(ù��° ���� ���� ����, �Ǵ� ����� ��ġ�� ���� )
        GameClear, //��� ���ڸ� ã���� ��
        GameOver// ���ڰ� �ִ� ���� ������ ��
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
            Debug.Log($"������� : {state}");
        }
    }

    public bool IsPlaying => State == GameState.Play;//������ ���������� �ƴ��� Ȯ���ϱ� ���� ������Ƽ

    public Action onGameReady; // ���� �ʱ�ȭ��
    public Action onGamePlay;
    public Action onGameClear;
    public Action onGameOver;
    //���� ���°��� -----------------------------------------------------------------------


    //���� ���� ----------------------------------------------------------------------------------------------
    Board board;
    public Board Board => board;

    public int mineCount = 20;
    public int boardWidth = 15;
    public int boardHeight = 15;

    
    //���� ���� ----------------------------------------------------------------------------------------------

    //��߰��� ���� -----------------------------------------------------------------
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
    //��߰��� ���� End -----------------------------------------------------------------

    protected override void OnInitialize()
    {
        FlagCount = mineCount;
        board = FindObjectOfType<Board>();
        board.Initialize(boardWidth, boardHeight, mineCount);// ���� �ʱ�ȭ 
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
        //���� �ʱ�ȭ �ϱ�
        // ����, Ÿ�̸�, �÷���ī����
        FlagCount = mineCount;
        if (state == GameState.GameClear || state == GameState.GameOver)
        {
            State = GameState.Ready;
        }
    }
    public void GameOver()
    {
        State = GameState.GameOver;
        //Ÿ�̸� ����,
        //���� ���̻� ������ �ʱ�(�÷��� �����϶��� ������ �ϱ�)
    }

    public void GameClear()
    {
        State = GameState.GameClear;
    }
    // �׽�Ʈ �ڵ�-------------------------------------------------------------------------
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
