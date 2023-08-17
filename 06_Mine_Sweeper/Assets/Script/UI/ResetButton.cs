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
                image.sprite = buttonSprites[(int)state];//내부에서만 바꿀 것이기 때문에 굳이 인덱서를 사용할 이유가 없다
            }
        }
    }

    public Sprite[] buttonSprites;//버튼에 표시될 스프라이트 
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
        //누르면 놀란표정
        //때면 되돌아오기
        //게임오버시 사망표정
        //클리어시 클리어
    }

    // flagCount == cells CloseCell Count
    // 
}
