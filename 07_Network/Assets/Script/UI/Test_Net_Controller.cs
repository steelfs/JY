using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Test_Net_Controller : MonoBehaviour
{
    TextMeshProUGUI playersInGame;
    private void Awake()
    {
        Button startHost = transform.GetChild(0).GetComponent<Button>();
        Button startClient = transform.GetChild(1).GetComponent<Button>();
        Button disconnect = transform.GetChild(2).GetComponent<Button>();
        playersInGame = transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        startHost.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost()) //ȣ��Ʈ�� ���� �õ�
            {
                Debug.Log("ȣ��Ʈ ����");
            }
            else
            {
                Debug.Log("ȣ��Ʈ ���� ����");
            }
        });

        startClient.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())// Ŭ���̾�Ʈ�� ȣ��Ʈ(����) �� ���� �õ�
            {
                Debug.Log("Ŭ���̾�Ʈ�� ���� ����");
            }
            else
            {
                Debug.Log("Ŭ���̾�Ʈ�� ���� ����");
            }
        });

        disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
        });

        GameManager.Inst.onPlayersInGameChange += (newPlayerCount) => playersInGame.text = $"Players : {newPlayerCount}";
        //��������Ʈ�� ����Ǹ� ���޸𸮿� �ö󰡰� �ȴ�.
    }
}
