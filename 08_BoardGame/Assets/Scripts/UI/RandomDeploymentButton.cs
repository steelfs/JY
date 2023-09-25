using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDeploymentButton : MonoBehaviour
{
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Onclick);
    }
    void Onclick()
    {
        UserPlayer player = GameManager.Inst.UserPlayer;
        if (player.IsAllDeployed)//모두 배치됐으면
        {
            player.UndoAllShipDeployment();//배치 리셋 
        }
        player.AutoShipDeployment(true);
    }
}
