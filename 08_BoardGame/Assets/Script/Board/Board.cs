using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
 

    public const int Board_Size = 10;
    public const int NOT_VALID_INDEX = -1;

    ShipType[] shipInfo; // ���忡 ��ġ�� �� ����. ��ħ ����
    //Ŭ���� ���� �׸�����ǥ�� �α� 

    private void Awake()
    {
        shipInfo = new ShipType[Board_Size * Board_Size];
    }

    public bool shipDeployment(Ship ship, Vector2Int grid)//�Լ��� ��ġ�ϴ� �Լ� , �Ȱ��ļ� ������ true���� // gridPos �Լ� �Ӹ� ��ġ
    {
        Vector2Int[] gridPositions;
        bool result = IsShipDeployment_Available(ship, grid, out gridPositions);
        if (result)
        {
            foreach (var pos in gridPositions)
            {
                shipInfo[Grid_To_Index(pos)] = ship.ShipType;
            }
            Vector3 world = Grid_To_World(grid);
            ship.transform.position = world;// �� ��ġ ����
            ship.Deploy(gridPositions); // �� ��ġ�Լ� ����
        }
        return result;
    }
    public bool shipDeployment(Ship ship, Vector3 world)//�Լ��� ��ġ�ϴ� �Լ� , �Ȱ��ļ� ������ true���� // gridPos �Լ� �Ӹ� ��ġ
    {
        return shipDeployment(ship, World_To_Grid(world));
    }
    public bool IsShipDeployment_Available(Ship ship, Vector2Int grid, out Vector2Int[] resultPos) //Ư�� �� �� Ư�� ��ġ�� ��ġ�� �� �ִ��� Ȯ���ϴ� �Լ�  out = Ȯ���ϰ� ��ġ�� �� �ִ� ��ġ(true�� ����)
    {
        resultPos = new Vector2Int[ship.Size];// �� ũ�⸸ŭ ��� ������ �迭 ����

        Vector2Int dir = Vector2Int.zero;
        switch (ship.Direction)// ���� ���⿡ ���� �׸�����ǥ�� ���� ���� ����
        {
            case ShipDirection.North:
                dir = Vector2Int.up;
                break;
            case ShipDirection.East:
                dir = Vector2Int.left;
                break;
            case ShipDirection.South:
                dir = Vector2Int.down;
                break;
            case ShipDirection.West:
                dir = Vector2Int.right;
                break;
        }
        for (int i = 0; i < ship.Size; i++) //Ȯ���� ��ġ ����
        {
            resultPos[i] = grid + dir * i;
        }

        bool result = true;
        foreach (var pos in resultPos) //����� ��ġ �� �ϳ��� ���� ���� ����ų� �̹� �谡 ��ġ�Ǿ��ִٸ� ����
        {
            if (!Is_In_Board(pos) || IsShipDeployed(pos))//���� ���� ���̰ų� �̹� �谡 �ִٸ� 
            {
                result = false;
                break;
            }
        }

        return result;
    }
    public bool IsShipDeployment_Available(Ship ship, Vector2Int grid) //Ư�� �� �� Ư�� ��ġ�� ��ġ�� �� �ִ��� Ȯ���ϴ� �Լ�  out = Ȯ���ϰ� ��ġ�� �� �ִ� ��ġ(true�� ����)
    {
        return IsShipDeployment_Available(ship, grid, out _);
    }
    public bool IsShipDeployment_Available(Ship ship, Vector3 world) //Ư�� �� �� Ư�� ��ġ�� ��ġ�� �� �ִ��� Ȯ���ϴ� �Լ�  out = Ȯ���ϰ� ��ġ�� �� �ִ� ��ġ(true�� ����)
    {
        return IsShipDeployment_Available(ship, World_To_Grid(world), out _);
    }
    bool IsShipDeployed(Vector2Int grid)//Ư����ġ�� �谡 �ִ��� Ȯ���ϴ� �Լ� true
    {
        return shipInfo[Grid_To_Index(grid)] != ShipType.None;
    }


    public void UndoshipDeployment(Ship ship)
    {

    }
    public Vector2Int World_To_Grid(Vector3 worldPos)
    {
        worldPos.y = transform.position.y;

        Vector3 diff = worldPos - transform.position;
        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }
    public Vector3 Grid_To_World(int x, int y)
    {

        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }
    public Vector3 Grid_To_World(Vector2Int grid)
    {
        return Grid_To_World(grid.x, grid.y);
    }
    public Vector3 Index_To_World(int index)
    {

        return Grid_To_World(Index_To_Grid(index));
    }
    public Vector2Int Get_Mouse_Grid_Pos()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
        return World_To_Grid(world);
    }
    public bool Is_In_Board(Vector3 worldPos)
    {
        worldPos.y = transform.position.y;

        Vector3 diff = worldPos - transform.position;
        return diff.x >= 0.0f && diff.x < Board_Size && diff.z <= 0 && diff.z >= -Board_Size;
    }
    public static int Grid_To_Index(int x, int y)
    {
        int result = NOT_VALID_INDEX;
        if (Is_In_Board(x, y))
        {
            return result = x + y * Board_Size;
        }
        return result;
    }
    public static int Grid_To_Index(Vector2Int grid)
    {
        return Grid_To_Index(grid.x, grid.y);
    }


    public static Vector2Int Index_To_Grid(int index)
    {
        return new Vector2Int(index % Board_Size, index / Board_Size);
    }
    public static bool Is_In_Board(int x, int y)
    {
        return x > -1 && x < Board_Size && y > -1 && y < Board_Size;
    }
    public static bool Is_In_Board(Vector2Int grid)
    {
        return Is_In_Board(grid.x, grid.y);
    }
}