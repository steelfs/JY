using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ShipType : byte
{
    None = 0, // �谡 �ƴ� ��
    Carrier,  // �װ�����
    BattleShip,// ���� 4ĭ
    Destroyer, // ������ 3ĭ
    SubMarine, // ����� 3ĭ
    PatrolBoat // ����� 2ĭ
}
public enum ShipDirection : byte //��Ӹ��� ���ϴ� ����
{
    North = 0,
    East,
    South,
    West

}
public class Ship : MonoBehaviour
{
    //���� ����
    ShipType shipType = ShipType.None;
    public ShipType ShipType
    {
        get => shipType;
        private set
        {
            shipType = value;
            switch (shipType)
            {
                case ShipType.Carrier:
                    size = 5;
                    break;
                case ShipType.BattleShip:
                    size = 4;
                    break;
                case ShipType.Destroyer:
                    size = 3;
                    break;
                case ShipType.SubMarine:
                    size = 3;
                    break;
                case ShipType.PatrolBoat:
                    size = 2;
                    break;
                default:
                    break;
            }
            //shipName
        }
    }

    ShipDirection direction = ShipDirection.North;
    public ShipDirection Direction
    {
        get => direction;
        set
        {
            direction = value;
            model.rotation = Quaternion.Euler(0, (int)direction * 90, 0);
        }
    }

    string shipName;
    public string ShipName => shipName;

    int size = 0;
    public int Size => size;

    int hp = 0;
    public int HP
    {
        get => hp;
        set
        {

        }
    }

    bool isAlive = true;
    public bool IsAlive => isAlive;

    bool isDeployed = false;//��ġ�� �ƴ°�
    public bool IsDeployed => isDeployed;
    Vector2Int[] positions; // �谡 ��ġ�� ��ġ

    Renderer shipRenderer;

    Transform model;//�� �� �޽��� Ʈ������

    public Action<Ship> on_Hit;
    public Action<Ship> on_Sinking;
    public Action<bool> on_Deployed;// ��ġ �Ϸ�, ��ҽ�

    public void Initialize(ShipType type)
    {
        ShipType = type;

        model = transform.GetChild(0);
        shipRenderer = model.GetComponentInChildren<Renderer>();
        ResetData();
    }
    void ResetData()//�������� ������ �ʱ�ȭ�ϴ� �Լ� 
    {

    }

    public void SetMaterial_Type(bool isNormal = true)// true�� �������Ƽ����, false = ��ġ���� ������
    {

    }

    public void Deploy(Vector2Int position)//��ġ�� ��ġ
    {

    }
    public void UnDeploy()
    {

    }
    public void Rotate(bool isCW = true)//�Լ��� 90���� ȸ����Ű�� �Լ� true = �ݽð���� ȸ��, false�� �ð����
    {

    }

    public void RandomRotate()//������ �������� ȸ��
    {

    }
    public void OnHitted()
    {

    }
    void OnSinking()// �Լ��� ħ���� �� ����� �Լ� 
    {

    }

}
