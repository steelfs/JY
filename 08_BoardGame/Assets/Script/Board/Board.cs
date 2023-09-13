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

    public bool shipDeployment(Ship ship, Vector2Int gridPos)//함선을 배치하는 함수 , 안겹쳐서 성공시 true리턴 // gridPos 함선 머리 위치
    {
        return false;
    }

    public bool IsShipDeployment(Ship ship, Vector2Int gridPos, out Vector2Int[] resultPos) //특정 배 가 특정 위치에 배치될 수 있는지 확인하는 함수  out = 확실하게 배치할 수 있는 위치(true일 때만)
    {
        resultPos = null;
        return false;
    }
    public bool IsShipDeployment(Ship ship, Vector2Int gridPos) //특정 배 가 특정 위치에 배치될 수 있는지 확인하는 함수  out = 확실하게 배치할 수 있는 위치(true일 때만)
    {
        return false;
    }
    public bool IsShipDeployment(Ship ship, Vector3 world) //특정 배 가 특정 위치에 배치될 수 있는지 확인하는 함수  out = 확실하게 배치할 수 있는 위치(true일 때만)
    {
        return false;
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
        if (Is_Valid_Grid_Pos(x, y))
        {
            return result = x + y * Board_Size;
        }
        return result;
    }

    public static Vector2Int Index_To_Grid(int index)
    {
        return new Vector2Int(index % Board_Size, index / Board_Size);
    }
    public static bool Is_Valid_Grid_Pos(int x, int y)
    {
        return x > -1 && x < Board_Size && y > -1 && y < Board_Size;
    }
    public static bool Is_Valid_Grid_Pos(Vector2Int grid)
    {
        return Is_Valid_Grid_Pos(grid.x, grid.y);
    }
}
