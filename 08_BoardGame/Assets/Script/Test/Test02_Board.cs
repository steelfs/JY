using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test02_Board : TestBase
{
    // 1. BoardŬ���� �Լ� �ϼ�
    // 2. Ŭ���� �׸�����ǥ ���
    // 3. Ŭ�� ��ġ ���� ��, �� Ȯ�� ���
    // 4. Ŭ���� Ŭ���� ������ �׸��� ���� ���

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
        Debug.Log($"�׸��� ��ǥ : {grid.x}, {grid.y}");

        if (board.Is_In_Board(world))
        {
            Debug.Log("���� ����");
        }
        else
        {
            Debug.Log("���� �ٱ���");
        }

        Vector3 center = board.Grid_To_World(grid);
        Debug.Log($"�׸��� ���� :{center.x}, {center.z}");
    }
}
