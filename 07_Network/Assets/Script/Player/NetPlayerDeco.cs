using Cinemachine;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerDeco : NetworkBehaviour
{
    NetworkVariable<Color> bodyColor = new NetworkVariable<Color>();
    NetworkVariable<FixedString32Bytes> userName = new NetworkVariable<FixedString32Bytes>();
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
        userName.OnValueChanged += OnNameSet;

    }

    private void OnNameSet(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {

        namePlate.SetName(newValue.ToString());

        GameManager.Inst.Log($"Deco : [{newValue}] �� ����  �߽��ϴ�.");

    }

    private void Start()
    {
        //GameManager.Inst.onUserNameChange += ChangeName;
    }
    private void OnColorChange(Color previousValue, Color newValue)
    {
        bodyMat.SetColor("_BaseColor", newValue);//���̴� ������Ƽ �� ����
    }

 
    public override void OnNetworkSpawn()
    {
        if (IsServer)// �� Ŭ���̾�Ʈ�� �����̸� 
        {
            if (GameManager.Inst.UserColor == Color.clear)
            {
                bodyColor.Value = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);//user�÷��� ���� ������ �ȵ����� ���� 
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



    [ServerRpc]
    void RequestBillBoardNAmeChangeServerRpc(string name)
    {
        userName.Value = name;
    }
    public void SetName(string name)
    {
        if (IsOwner)
        {
            if (IsServer)
            {
                userName.Value = name;
            }
            else
            {
                RequestBillBoardNAmeChangeServerRpc(name);
            }
        }
    }
    public void RefreshNamePlate()
    {
        namePlate.SetName(userName.Value.ToString());
    }
}
