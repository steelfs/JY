using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishButton : MonoBehaviour
{
    Button button;
    UserPlayer player;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Onclick);
        player = GameManager.Inst.UserPlayer;
        foreach(var ship in player.Ships)
        {
            ship.onDeploy += OnShipDeploed;
        }
    }

    private void OnShipDeploed(bool isDeployed)
    {
        if (isDeployed && player.IsAllDeployed)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    void Onclick()
    {
        UserPlayer player = GameManager.Inst.UserPlayer;

        Debug.Log("Finish 버튼 클릭");
        SceneManager.LoadScene("Battle");
    }
}
