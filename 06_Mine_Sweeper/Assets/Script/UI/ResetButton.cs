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
                image.sprite = buttonSprites[(int)state];
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
    }
    private void Start()
    {
        
    }

}
