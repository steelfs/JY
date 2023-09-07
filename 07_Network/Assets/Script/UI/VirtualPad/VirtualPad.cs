using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPad : MonoBehaviour
{
    Virtual_Stick stick;
    Virtual_Button[] button;

    public Action<Vector2> onMoveInput
    {
        get => stick.onMoveInput;
        set { stick.onMoveInput = value;}
    }
 
    public Action onAttack01Input
    {
        get => button[(int)ButtonType.Bullet].on_Press;
        set => button[(int)ButtonType.Bullet].on_Press = value;
    }
    public Action onAttack02Input
    {
        get => button[(int)ButtonType.Orb].on_Press;
        set => button[(int)ButtonType.Orb].on_Press = value;
    }
    public enum ButtonType
    {
        Orb = 0,
        Bullet = 1
    }
    private void Awake()
    {
        stick = GetComponentInChildren<Virtual_Stick>();
        button = GetComponentsInChildren<Virtual_Button>();
    }
}
