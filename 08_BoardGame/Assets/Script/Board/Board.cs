using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
 

    public const int Board_Size = 10;
    public const int NOT_VALID_INDEX = -1;

    ShipType[] shipInfo; // 보드에 배치된 배 정보. 겹침 방지
    //클릭한 지점 그리드좌표로 로그 

    private void Awake()
    {
        shipInfo = new ShipType[Board_Size * Board_Size];
    }

    public bool shipDeployment(Ship ship, Vector2Int grid)//함선을 배치하는 함수 , 안겹쳐서 성공시 true리턴 // gridPos 함선 머리 위치
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
            ship.transform.position = world;// 배 위치 변경
            ship.Deploy(gridPositions); // 배 배치함수 실행
        }
        return result;
    }
    public bool shipDeployment(Ship ship, Vector3 world)//함선을 배치하는 함수 , 안겹쳐서 성공시 true리턴 // gridPos 함선 머리 위치
    {
        return shipDeployment(ship, World_To_Grid(world));
    }
    public bool IsShipDeployment_Available(Ship ship, Vector2Int grid, out Vector2Int[] resultPos) //특정 배 가 특정 위치에 배치될 수 있는지 확인하는 함수  out = 확실하게 배치할 수 있는 위치(true일 때만)
    {
        resultPos = new Vector2Int[ship.Size];// 배 크기만큼 결과 저장할 배열 생성

        Vector2Int dir = Vector2Int.zero;
        switch (ship.Direction)// 배의 방향에 따라 그리드좌표를 구할 방향 지정
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
        for (int i = 0; i < ship.Size; i++) //확인할 위치 저장
        {
            resultPos[i] = grid + dir * i;
        }

        bool result = true;
        foreach (var pos in resultPos) //저장된 위치 중 하나라도 보드 밖을 벗어나거나 이미 배가 배치되어있다면 실패
        {
            if (!Is_In_Board(pos) || IsShipDeployed(pos))//보드 범위 밖이거나 이미 배가 있다면 
            {
                result = false;
                break;
            }
        }

        return result;
    }
    public bool IsShipDeployment_Available(Ship ship, Vector2Int grid) //특정 배 가 특정 위치에 배치될 수 있는지 확인하는 함수  out = 확실하게 배치할 수 있는 위치(true일 때만)
    {
        return IsShipDeployment_Available(ship, grid, out _);
    }
    public bool IsShipDeployment_Available(Ship ship, Vector3 world) //특정 배 가 특정 위치에 배치될 수 있는지 확인하는 함수  out = 확실하게 배치할 수 있는 위치(true일 때만)
    {
        return IsShipDeployment_Available(ship, World_To_Grid(world), out _);
    }
    bool IsShipDeployed(Vector2Int grid)//특정위치에 배가 있는지 확인하는 함수 true
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
