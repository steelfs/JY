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
        Play, // ������(ù��° ���� ���� ���� )
        GameClear, //��� ���ڸ� ã���� ��
        GameOver// ���ڰ� �ִ� ���� ������ ��
    }

    public GameState state = GameState.Ready;
    public Action onGameReady; // ���� �ʱ�ȭ��
    public Action onGamePlay;
    public Action onGameClear;
    public Action onGameOver;
    //���� ���°��� -----------------------------------------------------------------------


    //���� ���� ----------------------------------------------------------------------------------------------
    Board board;
    public Board Board => board;

    public int mineCount = 10;
    public int boardWidth = 8;
    public int boardHeight = 8;
    //���� ���� ----------------------------------------------------------------------------------------------

    //��߰��� ���� -----------------------------------------------------------------
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
