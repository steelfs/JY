using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : Singleton<ShipManager>
{
    public GameObject shipPrefab;// 함선 프리팹  모델 정보 없음
    public GameObject[] shipModels;// 함선의 모델 프리팹 (매시만 있음)

    public Material[] shipMaterials;//함선의 머티리얼  0 = 일반상황용 // 1 = 배치모드용 
    public Material NormalShip_Material => shipMaterials[0];
    public Material DeployModeShip_Material => shipMaterials[1];



    readonly Color successColor = new Color(0, 1, 0, 0.2f);// 반투명한 녹색  //배치 가능할 때 
    readonly Color failColor = new Color(1, 0, 0, 0.2f);// 반투명한 빨간색 //배치가 불가능할 때 사용 

    int shipType_Count;// 배의 종류 enum의 수
    public int ShipType_Count => shipType_Count;

    int shipDirection_Count;//enum 의방향 갯수 
    public int ShipDirection_Count => shipDirection_Count;

    readonly public string[] shipNames = { "항공모함", "전함", "구축함", "잠수함", "경비정" };
    string this[ShipType type] => shipNames[(int) type -1];

    protected override void OnInitialize()
    {
        shipType_Count = Enum.GetValues(typeof(ShipType)).Length - 1;
        shipDirection_Count = Enum.GetValues(typeof(ShipDirection)).Length;
    }

    public Ship MakeShip(ShipType type, Transform ownerTransform) // 배 게임오브젝트를 생성하는 함수  type = 배의 종류 // 생성된 배를 적과 나 중 어느 보드에 붙힐 것인가? 
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

    public void SetDeployMode_Color(bool isSuccess)//배치모드 색상의 머티리얼 색상 변경 
    {
        if (isSuccess)
        {
            DeployModeShip_Material.color = successColor;
        }
        else
        {
            DeployModeShip_Material.color = failColor;
        }
        //deploy 의 색상 변경 
        //successColor; failColor; 둘 중 하나 
    }
}
