using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
 

    public const int Board_Size = 10;
    public const int NOT_VALID_INDEX = -1;
    //클릭한 지점 그리드좌표로 로그 

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
