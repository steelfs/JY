using Cinemachine;
using System;
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

    BombMark bombMark;//보드가 공격당한 위치를 시각적으로 보여주는 클래스
    bool[] isAttacked;//공격받으면 true로 성정되는 배열

    public Dictionary<ShipType, Action> on_ShipAttacked;// 배 종류별로 공격당했을 때 실행될 델리게이트를 가지는 Dictionary
    //이진트리방식, 시간복잡도 = 배열보다는 느리지만 List보다는 빠르다.
    private void Awake()
    {
        shipInfo = new ShipType[Board_Size * Board_Size];
        isAttacked = new bool[Board_Size * Board_Size];
        bombMark = GetComponentInChildren<BombMark>();

        on_ShipAttacked = new Dictionary<ShipType, Action>(ShipManager.Inst.ShipType_Count + 1);
        on_ShipAttacked[ShipType.None] = null;// 연결된 함수는 없으나 null이라도 할당해 놓지 않으면  on_ShipAttacked[ShipType.None] ?.Invoke(); 했을 때 터지게 된다.
        on_ShipAttacked[ShipType.Carrier] = null;
        on_ShipAttacked[ShipType.BattleShip] = null;
        on_ShipAttacked[ShipType.Destroyer] = null;
        on_ShipAttacked[ShipType.SubMarine] = null;
        on_ShipAttacked[ShipType.PatrolBoat] = null;



    }

    public void ResetBoard(Ship[] ships)
    {
        foreach (var ship in ships)
        {
            UndoshipDeployment(ship);

        }

        for (int i = 0; i < isAttacked.Length; i++)
        {
            isAttacked[i] = false;
        }
        bombMark.ResetBombMArk(); 
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

    //보드가 공격받았을 때 실행될 함수 grid = 공격받은 그리드 위치// 공격이 배에 맞았으면 true아니면 false
    public bool OnAttacked(Vector2Int grid)
    {
        bool result = false;
        if (Is_In_Board(grid))
        {
            int index = Grid_To_Index(grid);
            if (isAttackable(index))
            {
                isAttacked[index] = true;
                if (shipInfo[index] != ShipType.None)//배가있으면 공격성공
                {
                    result = true;
                    on_ShipAttacked[shipInfo[index]]?.Invoke();// 공격당한 배의 델리게이트 실행
                }
                
                bombMark.SetBombMark(Grid_To_World(grid), result);

            }

        }


        return result;
    }

    public bool isAttackable(int index)//지정된 위치가 공격가능한지 확인 하는 함수 
    {
        return !isAttacked[index];
    }

    public bool isAttackable(Vector2Int grid)//지정된 위치가 공격가능한지 확인 하는 함수 
    {
        return isAttackable(Grid_To_Index(grid));
    }
    public void UndoshipDeployment(Ship ship)
    {
        if (ship.IsDeployed)//이미 배치되어있으면 
        {
            foreach (var pos in ship.Positions)
            {
                shipInfo[Grid_To_Index(pos)] = ShipType.None;
            }
            ship.UnDeploy();//배치 취소
            ship.gameObject.SetActive(false);
        }
    }
    public ShipType GetShipType(Vector2Int grid)// 파라미터로 받은 위치의 배 정보 받아오는 함수 
    {
        return shipInfo[Grid_To_Index(grid)];
    }
    public ShipType GetShipType(Vector3 world)
    {
        return GetShipType(World_To_Grid(world));
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
    public bool IsAttackSuccessPosition(Vector2Int grid)//특정 위치가 공격 성공한 위치인지 확인하는 함수 
    {
        int index = Grid_To_Index(grid);//확인할 위치
        return index != NOT_VALID_INDEX && isAttacked[index] && shipInfo[index] != ShipType.None; // 유효한 인덱스 이면서 공격받았고, ShipType이 None이 아니면(배가 있으면) 공격성공 판정
    }
    public static int Grid_To_Index(int x, int y)
    {
        int result = NOT_VALID_INDEX;
        if (Is_In_Board(x, y))
        {
            result = x + y * Board_Size;
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
