using Cinemachine;
using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerDeco : NetworkBehaviour
{
    NetworkVariable<Color> bodyColor = new NetworkVariable<Color>();
    NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();
    NamePlate namePlate;
    Logger logger;

    Renderer playerRenderer;
    Material bodyMat;

    private void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>();
        bodyMat = playerRenderer.material;
        namePlate = GetComponentInChildren<NamePlate>();
        logger = FindObjectOfType<Logger>();

        bodyColor.OnValueChanged += OnColorChange;
    }

    private void OnColorChange(Color previousValue, Color newValue)
    {
        bodyMat.SetColor("_BaseColor", newValue);
    }

    void ChangeName(string name)
    {
        if (IsServer)
        {
            playerName.Value = name;
        }
        namePlate.SetName(name);
    }
 
    public override void OnNetworkSpawn()
    {
        if (IsServer)// 이 클라이언트가 서버이면 
        {
            if (GameManager.Inst.UserColor == Color.clear)
            {
                bodyColor.Value = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);//user컬러가 아직 선택이 안됐으면 새로 
            }
            else
            {
                //bodyColor.Value = GameManager.Inst.UserColor;
            }
        }
        bodyMat.SetColor("_BaseColor", bodyColor.Value);

    }

    public void SetColor(Color color)
    {
        if (IsOwner)
        {
            if (IsServer)
            {
                bodyColor.Value = color;
            }
            else
            {
                RequestBodyColorChangeServerRpc(color);
            }
        }
    }
    [ServerRpc]
    void RequestBodyColorChangeServerRpc(Color color)
    {
        bodyColor.Value = color;
    }
}
