using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Test_Net_Controller : MonoBehaviour
{

    private void Awake()
    {
        Button startHost = transform.GetChild(0).GetComponent<Button>();
        Button startClient = transform.GetChild(1).GetComponent<Button>();
        Button disconnect = transform.GetChild(2).GetComponent<Button>();

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
    }
}
