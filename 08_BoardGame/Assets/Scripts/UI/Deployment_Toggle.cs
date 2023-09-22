using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Deployment_Toggle : MonoBehaviour
{
    Image image;
    Button button;
    GameObject deployText;

    readonly Color selectColor = new(1,1,1,0.2f);
    //bool isToggled = false;
    //bool IsToggled
    //{
    //    get => isToggled;
    //    set
    //    {
    //        if (isToggled != value)
    //        {
    //            isToggled = value;
    //            if (isToggled)
    //            {
    //                image.color = pressColor;
    //                onPress?.Invoke(this);
    //            }
    //            else
    //            {
    //                image.color = Color.white;
    //            }
    //        }
    //    }
    //}                                                                           
    public enum DeployState
    {
        NotSelect,//선택되지 않은 상태 
        Select,
        Deploy
    }
    DeployState state;
    public DeployState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case DeployState.NotSelect:
                        image.color = Color.white;
                        deployText.gameObject.SetActive(false);
                        break;
                    case DeployState.Select:
                        image.color = selectColor;
                        deployText.gameObject.SetActive(false);
                        break;
                    case DeployState.Deploy:
                        image.color = selectColor;
                        deployText.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    UserPlayer player;

    public ShipType shipType;
   // public Action<Deployment_Toggle> onSelect;
    public Action<Deployment_Toggle> onStateChange;
    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        deployText = transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
    }
    private void OnClick()
    {
        switch (state)
        {
            case DeployState.NotSelect:
                State = DeployState.Select;//처음 선택 , 자신 이외의것들 선택 해제
                player.SelectShipToDeploy(shipType);
                break;
            case DeployState.Select:
                State = DeployState.NotSelect;//자신에 대해 선택 해제
                //1. 함선배치 구현
                // 플레이어의 입력델리게이트 구현
                //onPress?.Invoke(this);
                break;
            case DeployState.Deploy:
                State = DeployState.NotSelect;//배치 취소 후 NotSelect
                player.UndoShipDeploy(shipType);
                break;
        }
        onStateChange?.Invoke(this);
    }

    public void SetSelect()//패널에서 제어하기위한 함수 
    {
       // IsToggled = true;
    }
    public void SetNotSelect()
    {
        if (State != DeployState.Deploy)
        {
            State = DeployState.NotSelect;
        }
    }
}
