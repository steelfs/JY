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
        NotSelect,//���õ��� ���� ���� 
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
                State = DeployState.Select;//ó�� ���� , �ڽ� �̿��ǰ͵� ���� ����
                player.SelectShipToDeploy(shipType);
                break;
            case DeployState.Select:
                State = DeployState.NotSelect;//�ڽſ� ���� ���� ����
                //1. �Լ���ġ ����
                // �÷��̾��� �Էµ�������Ʈ ����
                //onPress?.Invoke(this);
                break;
            case DeployState.Deploy:
                State = DeployState.NotSelect;//��ġ ��� �� NotSelect
                player.UndoShipDeploy(shipType);
                break;
        }
        onStateChange?.Invoke(this);
    }

    public void SetSelect()//�гο��� �����ϱ����� �Լ� 
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
