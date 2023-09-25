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
        if (player.IsAllDeployed)//��� ��ġ������
        {
            player.UndoAllShipDeployment();//��ġ ���� 
        }
        player.AutoShipDeployment(true);
    }
}
