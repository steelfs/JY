using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test02_Board : TestBase
{
    // 1. Board클래스 함수 완성
    // 2. 클릭시 그리드좌표 출력
    // 3. 클릭 위치 보드 안, 밖 확인 출력
    // 4. 클릭시 클릭한 지점의 그리드 중점 출력

    Board board;
    private void Start()
    {
        board = FindObjectOfType<Board>();
    }
    protected override void LClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);

        Vector2Int grid = board.Get_Mouse_Grid_Pos();
        Debug.Log($"그리드 좌표 : {grid.x}, {grid.y}");

        if (board.Is_In_Board(world))
        {
            Debug.Log("보드 안쪽");
        }
        else
        {
            Debug.Log("보드 바깥쪽");
        }

        Vector3 center = board.Grid_To_World(grid);
        Debug.Log($"그리드 원점 :{center.x}, {center.z}");
    }
}
