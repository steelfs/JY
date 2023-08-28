using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GameManager : Net_SingleTon<GameManager>
{
    PlayerOnLine playerOnLine;
    Logger logger;
    NetPlayer player;
    public NetPlayer Player => player;//�� �÷��̾� 

    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);//���� ������ �� 

    protected override void OnInitialize()
    {
        logger = FindObjectOfType<Logger>(); // ���ÿ��� ���Ǵ� ��. ��Ÿ�ֿ̹� ã�Ƶ� ��
        playerOnLine = FindObjectOfType<PlayerOnLine>();
        NetworkManager.Singleton.OnClientConnectedCallback += OnclientConnect; //���� ������ ��� Ŭ���̾�Ʈ�� ������ �� ���� ����Ǵ� �Լ� 
        NetworkManager.Singleton.OnClientDisconnectCallback += OnclientDisConnect;
        playersInGame.OnValueChanged += UpdatePlayercount;
        UpdatePlayercount(0, 0);
    }

    private void UpdatePlayercount(int previousValue, int newValue)
    {
        playerOnLine.UpdateCount(newValue);
    }

  
    private void OnclientDisConnect(ulong id)
    {
        NetworkObject netobj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);

        if (IsServer)
        {
            playersInGame.Value--;
        }
        else
        {
            Sub_Playercount_RequestServerRpc();
        }
        if (netobj.IsOwner)
        {
            player = null;
        }

    }

    private void OnclientConnect(ulong id)// param = ������ Ŭ���̾�Ʈ�� ID
    {
        if (IsServer)
        {
            playersInGame.Value++;
        }
        else
        {
            Add_Playercount_RequestServerRpc();
        }
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
        if (netObj.IsOwner)//�� �ɸ��� �� �� 
        {
          
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player - {id}";

            foreach (var net in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList) //��� ������Ʈ���� ��ȸ
            {
                NetPlayer netplayer = net.GetComponent<NetPlayer>();
                if (netplayer != null && player != netplayer)// ������ �ƴ϶�� 
                {
                    net.gameObject.name = $"Other Player - {id}";
                }
            }
        }
        else
        {
    
            NetPlayer netplayer = netObj.GetComponent<NetPlayer>();
            if (netplayer != null || player != netplayer)
            {
                netObj.gameObject.name = $"Other Player - {id}";//�ٸ������ ������Ʈ �̸� ����
            }
        }
    }

    public void Log(string message)//�α׸� ����� �Լ� 
    {
        logger.Log(message);
    }
    [ServerRpc]
    void Add_Playercount_RequestServerRpc()
    {
        playersInGame.Value++;
    }
    [ServerRpc]
    void Sub_Playercount_RequestServerRpc()
    {
        playersInGame.Value--;
    }
}
