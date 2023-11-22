using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnActionManager : MonoBehaviour
{
    enum ButtonList {
        Attack = 0,
        Defence,
        Skill,
        Options,
        attack,
        defence,
        skill,
        options
    }
    Action[] buttonClickListener; 
     void Awake()
    {
        Array enumList = Enum.GetValues(typeof(ButtonList));
        buttonClickListener  = new Action[enumList.Length];

        for (int i = 0; i < enumList.Length; i++)
        {
            switch (enumList.GetValue(i))
            {
                case ButtonList.Attack:
                    buttonClickListener[i] += Attack;
                    break;
                case ButtonList.Defence:
                    buttonClickListener[i] += Defence;
                    break;
                case ButtonList.Skill:
                    buttonClickListener[i] += Skill;
                    break;
                case ButtonList.Options:
                    buttonClickListener[i] += Options;
                    break;
                case ButtonList.attack:
                    buttonClickListener[i] += Attack;
                    break;
                case ButtonList.defence:
                    buttonClickListener[i] += Defence;
                    break;
                case ButtonList.skill:
                    buttonClickListener[i] += Skill;
                    break;
                case ButtonList.options:
                    buttonClickListener[i] += Options;
                    break;
                default:
                    break;
            }
        }
        //
        RectTransform rt = transform.GetChild(0).GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(ButtonCreateBase<ButtonList>.width, ButtonCreateBase<ButtonList>.height * enumList.Length);
        ButtonCreateBase<ButtonList>.SettingNoneScriptButton(transform.GetChild(0).GetChild(0), ButtonList.Attack, buttonClickListener);
    }


    private void Attack() {
        Debug.Log("Attack 함수실행");
    }
    private void Defence() { 
    
        Debug.Log("Defence 함수실행");
    }
    private void Skill()
    {
        Debug.Log("Skill 함수실행");
    }
    private void Options() 
    {
        Debug.Log("Options 함수실행");
    }
}
