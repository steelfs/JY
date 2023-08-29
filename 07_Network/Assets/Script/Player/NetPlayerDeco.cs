using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerDeco : NetworkBehaviour
{
    NetworkVariable<Color> color = new NetworkVariable<Color>();
    Renderer playerRenderer;
    Material bodyMat;

    private void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>();
        bodyMat = playerRenderer.material;

       // color.OnValueChanged += OnColorChange;
    }

    //private void OnColorChange(Color previousValue, Color newValue)
    //{
    //    bodyMat.SetColor("_BaseColor", newValue);
    //}
    public override void OnNetworkSpawn()
    {
        if (IsServer)// 이 클라이언트가 서버이면 
        {
            color.Value = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 1.0f,1.0f, 1.0f, 1.0f);//서버쪽에서만 생삭을 랜덤으로 지정
        }
        bodyMat.SetColor("_BaseColor", color.Value);
    }
}
