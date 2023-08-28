using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GameManager : Net_SingleTon<GameManager>
{
    Logger logger;
    NetPlayer player;
    public NetPlayer Player => player;//�� �÷��̾� 

    protected override void OnInitialize()
    {
        logger = FindObjectOfType<Logger>(); // ���ÿ��� ���Ǵ� ��. ��Ÿ�ֿ̹� ã�Ƶ� ��
        NetworkManager.Singleton.OnClientConnectedCallback += OnclientConnect; //���� ������ ��� Ŭ���̾�Ʈ�� ������ �� ���� ����Ǵ� �Լ� 
        NetworkManager.Singleton.OnClientConnectedCallback += OnclientDisConnect;

    }

    private void OnclientDisConnect(ulong id)
    {

    }

    private void OnclientConnect(ulong id)// param = ������ Ŭ���̾�Ʈ�� ID
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
        if (netObj.IsOwner)//�� �ɸ��� �� �� 
        {
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player - {id}";

            foreach (var net in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList) //��� ������Ʈ���� ��ȸ
            {
                if (netObj != net)// ������ �ƴ϶�� 
                {
                    net.gameObject.name = $"Other Player - {id}";
                }
            }
        }
        else
        {
            netObj.gameObject.name = $"Other Player - {id}";//�ٸ������ ������Ʈ �̸� ����
        }
    }

    public void Log(string message)//�α׸� ����� �Լ� 
    {
        logger.Log(message);
    }


}
