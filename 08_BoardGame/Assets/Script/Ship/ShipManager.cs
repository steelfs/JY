using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : Singleton<ShipManager>
{
    public GameObject shipPrefab;// �Լ� ������  �� ���� ����
    public GameObject[] shipModels;// �Լ��� �� ������ (�Žø� ����)

    public Material[] shipMaterials;//�Լ��� ��Ƽ����  0 = �Ϲݻ�Ȳ�� // 1 = ��ġ���� 
    public Material NormalShip_Material => shipMaterials[0];
    public Material DeployModeShip_Material => shipMaterials[1];



    readonly Color successColor = new Color(0, 1, 0, 0.2f);// �������� ���  //��ġ ������ �� 
    readonly Color failColor = new Color(1, 0, 0, 0.2f);// �������� ������ //��ġ�� �Ұ����� �� ��� 

    int shipType_Count;// ���� ���� enum�� ��
    public int ShipType_Count => shipType_Count;

    int shipDirection_Count;//enum �ǹ��� ���� 
    public int ShipDirection_Count => shipDirection_Count;

    readonly public string[] shipNames = { "�װ�����", "����", "������", "�����", "�����" };
    string this[ShipType type] => shipNames[(int) type -1];

    protected override void OnInitialize()
    {
        shipType_Count = Enum.GetValues(typeof(ShipType)).Length - 1;
        shipDirection_Count = Enum.GetValues(typeof(ShipDirection)).Length;
    }

    public Ship MakeShip(ShipType type, Transform ownerTransform) // �� ���ӿ�����Ʈ�� �����ϴ� �Լ�  type = ���� ���� // ������ �踦 ���� �� �� ��� ���忡 ���� ���ΰ�? 
    {
        GameObject shipParent = Instantiate(shipPrefab, ownerTransform);
        GameObject child = GetShipModel(type);
        Instantiate(child, shipParent.transform);

        Ship ship = shipParent.GetComponent<Ship>();
        ship.Initialize(type);
        return ship;
    }

    GameObject GetShipModel(ShipType type)
    {
        return shipModels[(int)type - 1];
    }

    public void SetDeployMode_Color(bool isSuccess)//��ġ��� ������ ��Ƽ���� ���� ���� 
    {
        if (isSuccess)
        {
            DeployModeShip_Material.color = successColor;
        }
        else
        {
            DeployModeShip_Material.color = failColor;
        }
        //deploy �� ���� ���� 
        //successColor; failColor; �� �� �ϳ� 
    }
}
