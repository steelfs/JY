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

    public void TestFlag(int flag)
    {
        FlagCount = flag;
    }
    public void TestState(GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Ready:
                onGameReady?.Invoke();
                break;
            case GameManager.GameState.Play:
                onGamePlay?.Invoke();
                break;
            case GameManager.GameState.GameClear:
                onGameClear?.Invoke();
                break;
        }
    }
}
