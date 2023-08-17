using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResetButton : MonoBehaviour
{
    enum ButtonState
    {
        Normal = 0,
        Surprise,
        GameClear,
        GameOver
    }

    ButtonState state;
    ButtonState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                image.sprite = buttonSprites[(int)state];//���ο����� �ٲ� ���̱� ������ ���� �ε����� ����� ������ ����
            }
        }
    }

    public Sprite[] buttonSprites;//��ư�� ǥ�õ� ��������Ʈ 
    Image image;
    Button button;
    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() => GameManager.Inst.GameReset());
    }
    private void Start()
    {
        GameManager manager = GameManager.Inst;

        Board board = manager.Board;
        board.onBoardLeftPress += () => State = ButtonState.Surprise;
        board.onBoardLeftRelease += () => State = ButtonState.Normal;
        manager.onGameClear += () => State = ButtonState.GameClear;
        manager.onGameOver += () => State = ButtonState.GameOver;
        manager.onGameReady += () => State = ButtonState.Normal;
        //������ ���ǥ��
        //���� �ǵ��ƿ���
        //���ӿ����� ���ǥ��
        //Ŭ����� Ŭ����
    }

    // flagCount == cells CloseCell Count
    // 
}
