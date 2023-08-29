using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Net_SingleTon<GameManager>
{

    PlayerOnLine playerOnLine;
    Logger logger;
    NetPlayer player;
    public NetPlayer Player => player;//�� �÷��̾� 

    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);//���� ������ �� 
    NetworkVariable<Color> playerColor = new NetworkVariable<Color>();


    Color userColor = Color.clear;//���� �������� �÷�
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
        logger = FindObjectOfType<Logger>(); // ���ÿ��� ���Ǵ� ��. ��Ÿ�ֿ̹� ã�Ƶ� ��
        playerOnLine = FindObjectOfType<PlayerOnLine>();
        NetworkManager.Singleton.OnClientConnectedCallback += OnclientConnect; //���� ������ ��� Ŭ���̾�Ʈ�� ������ �� ���� ����Ǵ� �Լ� 
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

    private void OnclientConnect(ulong id)// param = ������ Ŭ���̾�Ʈ�� ID
    {

        if (IsServer)
        {
            playersInGame.Value++;
            
        }
      
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
    
        if (netObj.IsOwner)//�� �ɸ��� �� �� 
        {
          
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player - {id}";

            playerDeco = netObj.GetComponent<NetPlayerDeco>();
            if (userColor != Color.clear)
            {
                playerDeco.SetColor(userColor);
            }

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
    //private void OnPlayerInGameChange(int previousValue, int newValue)
    //{
    //    playerOnLine.UpdateCount(newValue);
    //}
    public void Log(string message)//�α׸� ����� �Լ� 
    {
        logger.Log(message);
    }
    public void SetUserName(string name)
    {
        UserName = name;
    }
}
