using Cinemachine;
using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Net_SingleTon<GameManager>
{

    PlayerOnLine playerOnLine;
    Logger logger;
    NetPlayer player;
    public NetPlayer Player => player;//�� �÷��̾� 

    CinemachineVirtualCamera virtualCam;
    public CinemachineVirtualCamera Vcam => virtualCam;

    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);//���� ������ �� 
    NetworkVariable<Color> playerColor = new NetworkVariable<Color>();

    VirtualPad virtualPad;
    public VirtualPad VirtualPad => virtualPad;


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

    NetPlayerDeco deco;
    public NetPlayerDeco Deco => deco;

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
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        NetworkManager.Singleton.OnClientConnectedCallback += OnclientConnect; //���� ������ ��� Ŭ���̾�Ʈ�� ������ �� ���� ����Ǵ� �Լ� 
        NetworkManager.Singleton.OnClientDisconnectCallback += OnclientDisConnect;
        playersInGame.OnValueChanged += (_,newValue) => onPlayersInGameChange?.Invoke(newValue);
        virtualPad = FindObjectOfType<VirtualPad>();
        
    }



    private void OnclientDisConnect(ulong id)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
        if (netObj.IsOwner)
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

        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
    
        if (netObj.IsOwner)//�� �ɸ��� �� �� 
        {
          
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player - {id}";
          

            deco = netObj.GetComponent<NetPlayerDeco>();
            if (userColor != Color.clear)
            {
                deco.SetColor(userColor);
            }
            deco.SetName($"{UserName}_{id}");//Ÿ���� ������ ���̸��� ó��

            foreach (var net in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList) //��� ������Ʈ���� ��ȸ
            {
                NetPlayer netplayer = net.GetComponent<NetPlayer>();
                if (netplayer != null && player != netplayer)// ������ �ƴ϶�� 
                {
                    net.gameObject.name = $"Other Player - {id}";
                }

                NetPlayerDeco netDeco = net.GetComponent<NetPlayerDeco>();
                if (netDeco != null && netDeco != deco)
                {
                    netDeco.RefreshNamePlate();// �ٸ� �������� �̸� ����
                }
            }
        }
        else
        {
    
            NetPlayer netplayer = netObj.GetComponent<NetPlayer>();
            if (netplayer != null || player != netplayer)
            {
                netObj.gameObject.name = $"Other Player - {id}";//�ٸ������ ������Ʈ �̸� ����
                logger.Log($"{netplayer.nameString.Value}_{id} �� �����߽��ϴ�.");
            }
        }

        if (IsServer)
        {
            playersInGame.Value++;
        }

    }
    private void OnUserConnected(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
      
    }
    private void OnUserDisConnected(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        Log($"[{newValue}] �� ���� ���� �߽��ϴ�.");
    }

 
    public void Log(string message)//�α׸� ����� �Լ� 
    {
        logger.Log(message);
    }
    public void SetUserName(string name)
    {
        UserName = name;
    }
}
