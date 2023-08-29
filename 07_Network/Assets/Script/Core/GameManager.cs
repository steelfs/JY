using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Net_SingleTon<GameManager>
{

    PlayerOnLine playerOnLine;
    Logger logger;
    NetPlayer player;
    public NetPlayer Player => player;//내 플레이어 

    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);//현재 동접자 수 
    NetworkVariable<Color> playerColor = new NetworkVariable<Color>();


    Color userColor = Color.clear;//현재 접속자의 컬러
    public Color UserColor
    {
        get => userColor;
        set
        {
            userColor = value;
            onUserColorChange?.Invoke(userColor);
        }
    }
    public Action<Color> onUserColorChange;

    NetPlayerDeco playerDeco;
    public NetPlayerDeco PlayerDeco => playerDeco;

    public Action<int> onPlayersInGameChange;
    public Action<string> onUserNameChange;
    string userName = "Default";
    public string UserName
    {
        get => userName;
        set
        {
            userName = value;
            onUserNameChange?.Invoke (userName);
        }
    }



    protected override void OnInitialize()
    {
        logger = FindObjectOfType<Logger>(); // 로컬에서 사용되는 것. 이타이밍에 찾아도 됨
        playerOnLine = FindObjectOfType<PlayerOnLine>();
        NetworkManager.Singleton.OnClientConnectedCallback += OnclientConnect; //나를 포함한 모든 클라이언트가 접속할 떄 마다 실행되는 함수 
        NetworkManager.Singleton.OnClientDisconnectCallback += OnclientDisConnect;
        playersInGame.OnValueChanged += (_,newValue) => onPlayersInGameChange?.Invoke(newValue);
    }


    private void OnclientDisConnect(ulong id)
    {
        NetworkObject netobj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
        if (netobj.IsOwner)
        {
            player = null;
        }
        if (IsServer)
        {
            playersInGame.Value--;
        }
   
    }

    private void OnclientConnect(ulong id)// param = 접속한 클라이언트의 ID
    {

        if (IsServer)
        {
            playersInGame.Value++;
            
        }
      
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
    
        if (netObj.IsOwner)//내 케릭터 일 때 
        {
          
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player - {id}";

            playerDeco = netObj.GetComponent<NetPlayerDeco>();
            if (userColor != Color.clear)
            {
                playerDeco.SetColor(userColor);
            }

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
    //private void OnPlayerInGameChange(int previousValue, int newValue)
    //{
    //    playerOnLine.UpdateCount(newValue);
    //}
    public void Log(string message)//로그만 남기는 함수 
    {
        logger.Log(message);
    }
    public void SetUserName(string name)
    {
        UserName = name;
    }
}
