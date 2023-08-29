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
            if (NetworkManager.Singleton.StartHost()) //호스트로 시작 시도
            {
                Debug.Log("호스트 시작");
            }
            else
            {
                Debug.Log("호스트 시작 실패");
            }
        });

        startClient.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())// 클라이언트로 호스트(서버) 에 접속 시도
            {
                Debug.Log("클라이언트로 연결 시작");
            }
            else
            {
                Debug.Log("클라이언트로 연결 실패");
            }
        });

        disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
        });

        GameManager.Inst.onPlayersInGameChange += (newPlayerCount) => playersInGame.text = $"Players : {newPlayerCount}";
        //델리게이트에 연결되면 힙메모리에 올라가게 된다.
    }
}
