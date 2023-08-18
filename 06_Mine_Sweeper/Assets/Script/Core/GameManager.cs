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

    public Action<int> onActionCountChange;
    int actionCount = 0;
    public int ActionCount
    {
        get => actionCount;
        set
        {
            if (actionCount != value)
            {
                actionCount = value;
                onActionCountChange?.Invoke(actionCount);
            }
        }
    }
    public Action<int, int> onUpdateUI;
    int findCount = 0;


    protected override void OnInitialize()
    {
        FlagCount = mineCount;
        board = FindObjectOfType<Board>();
        board.Initialize(boardWidth, boardHeight, mineCount);// ���� �ʱ�ȭ 
    }
    public void IncreaseFlagCount()
    {
        FlagCount++;
        findCount--;
    }
    public void DecreaseFlagCount()
    {
        FlagCount--;
        findCount++;
    }
    public void FinishPlayerAction()
    {
        ActionCount++;
        if (board.IsBoardClear)
        {
            GameClear();
            findCount = mineCount;
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
      
        if (state == GameState.GameClear || state == GameState.GameOver)
        {
            State = GameState.Ready;
            FlagCount = mineCount;
            ActionCount = 0;
            findCount = 0;
            onUpdateUI?.Invoke(mineCount, findCount);
        }
    }
    public void GameOver()
    {
        onUpdateUI?.Invoke(mineCount, findCount);
        State = GameState.GameOver;
        //Ÿ�̸� ����,
        //���� ���̻� ������ �ʱ�(�÷��� �����϶��� ������ �ϱ�)
    }

    public void GameClear()
    {
        onUpdateUI?.Invoke(mineCount, mineCount);
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
