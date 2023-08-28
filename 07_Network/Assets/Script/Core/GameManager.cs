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
    public NetPlayer Player => player;//내 플레이어 

    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);//현재 동접자 수 

    protected override void OnInitialize()
    {
        logger = FindObjectOfType<Logger>(); // 로컬에서 사용되는 것. 이타이밍에 찾아도 됨
        playerOnLine = FindObjectOfType<PlayerOnLine>();
        NetworkManager.Singleton.OnClientConnectedCallback += OnclientConnect; //나를 포함한 모든 클라이언트가 접속할 떄 마다 실행되는 함수 
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

    private void OnclientConnect(ulong id)// param = 접속한 클라이언트의 ID
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
        if (netObj.IsOwner)//내 케릭터 일 때 
        {
          
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player - {id}";

            foreach (var net in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList) //모든 오브젝트들을 순회
            {
                NetPlayer netplayer = net.GetComponent<NetPlayer>();
                if (netplayer != null && player != netplayer)// 내것이 아니라면 
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
                netObj.gameObject.name = $"Other Player - {id}";//다른사람의 오브젝트 이름 변경
            }
        }
    }

    public void Log(string message)//로그만 남기는 함수 
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
