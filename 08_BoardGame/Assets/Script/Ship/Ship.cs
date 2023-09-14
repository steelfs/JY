using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    public Vector2Int[] Positions => positions;
    Vector2Int[] defaultSize;

    Board board;
    Renderer shipRenderer;

    Transform model;//�� �� �޽��� Ʈ������

    public Action<Ship> on_Hit;
    public Action<Ship> on_Sinking;
    public Action<bool> on_Deploy;// ��ġ �Ϸ�, ��ҽ�

    public void Initialize(ShipType type)
    {
        board = FindObjectOfType<Board>();
        ShipType = type;
        defaultSize = new Vector2Int[Size];
        positions = defaultSize;

        model = transform.GetChild(0);
        shipRenderer = model.GetComponentInChildren<Renderer>();
        shipRenderer.material = ShipManager.Inst.DeployModeShip_Material;

        ResetData();

        gameObject.name = $"{ShipType}_{size}";
        StartCoroutine(movingCoroutine());
        gameObject.SetActive(false);
    }
    IEnumerator movingCoroutine()
    {
        Vector3 newPos = Vector3.zero;
        Vector2Int gridPos = Vector2Int.zero; 

        while (true)
        {
            gridPos = board.Get_Mouse_Grid_Pos();

            newPos.x = gridPos.x + 0.5f;
            newPos.z = -gridPos.y -(0.5f);
            newPos.y = 0;
            transform.position = newPos;
            yield return null;
        }

    }

    void ResetData()//�������� ������ �ʱ�ȭ�ϴ� �Լ� 
    {
        hp = size; 
        Direction = ShipDirection.North;

        isAlive = true;
        isDeployed = false; //��ġ ����
        positions = null;
    }

    public void SetMaterial_Type(bool isNormal = true)// true�� �������Ƽ����, false = ��ġ���� ������
    {
        if (isNormal)
        {
            shipRenderer.material = ShipManager.Inst.NormalShip_Material;
        }
        else
        {
            shipRenderer.material = ShipManager.Inst.DeployModeShip_Material;
        }
    }

    public void Deploy(Vector2Int[] position)//��ġ�� ��ġ
    {
        positions = position;
        isDeployed = true;
        on_Deploy?.Invoke(true);

       // SetMaterial_Type(true);
       // StopAllCoroutines();
        //��ġ ����
    }
    public void UnDeploy()
    {
        ResetData();
        on_Deploy?.Invoke(false);
    }
    public void Rotate(bool isCW = true)//�Լ��� 90���� ȸ����Ű�� �Լ� true = �ݽð���� ȸ��, false�� �ð����
    {
        int dirCount = ShipManager.Inst.ShipDirection_Count;
        if (isCW)
        {
            //Direction++;
            Direction = (ShipDirection)(((int)(Direction) + 1) % dirCount);
        }
        else
        {
            Direction = (ShipDirection)(((int)Direction + dirCount - 1) % dirCount);
            //Direction--;
        }
        
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
