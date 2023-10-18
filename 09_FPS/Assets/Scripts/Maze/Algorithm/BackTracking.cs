using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;
    public BackTrackingCell(int x, int y) : base(x, y) // base���� ���� ��  ���� 
    {
        visited = false;
    }
    public void FindPath()
    {
        visited = true;
        for (int i = 1; i < 4; i++)
        {
            
        }
    }
}
public class BackTracking : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells[(y * width) + x] = new Cell(x, y);
            }
        }
        BackTrackingCell cell = cells[50] as BackTrackingCell;
        
        for (int y = -1; y < 2; y++)
        {
            for(int x = -1; x < 2; x++)
            {
                int grid = GridToIndex(cell.X + x, cell.Y + y);
                BackTrackingCell neighbor = cells[grid] as BackTrackingCell;
                if (!neighbor.visited)
                {
                    neighbor.FindPath();

                }
            }

        }
        
        //Recursive BackTracking
        //�������� ����, ����
        //�ش� ����  �湮 ǥ��
        //�����ϰ� �� ���� �� �� ������ ����.(�湮�� ���� ���� ����)
        // �� ���� ��� �湮�ߴٸ� ���� ���� ���ư���.
        // ���������� ������������ ���ư��� �˰��� ����
        //
    }
}
