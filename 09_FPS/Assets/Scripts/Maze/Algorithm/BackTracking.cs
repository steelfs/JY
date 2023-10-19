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
  
}
public class BackTracking : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells[GridToIndex(x, y)] = new BackTrackingCell(x, y);
            }
        }

        BackTrackingCell start = cells[GridToIndex(0, 0)] as BackTrackingCell;
        start.visited = true;

        Stack<BackTrackingCell> stack = new Stack<BackTrackingCell>();
        stack.Push(start);

        while(stack.Count > 0)
        {
            BackTrackingCell current = stack.Peek();
            List<BackTrackingCell> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                BackTrackingCell neighbor = neighbors[Random.Range(0, neighbors.Count)];
                ConnectPath(current, neighbor);
                neighbor.visited = true;

                stack.Push(neighbor);
            }
            else
            {
                stack.Pop();
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

    List<BackTrackingCell> GetUnvisitedNeighbors(BackTrackingCell current)
    {
        List<BackTrackingCell> neighbors = new List<BackTrackingCell>(4);

        int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for(int i = 0; i < 4; i++)
        {
            int x = current.X + dir[i, 0];
            int y = current.Y + dir[i, 1];
            if (IsInGrid(x, y))
            {
                BackTrackingCell neighbor = cells[GridToIndex(x, y)] as BackTrackingCell;
                if (!neighbor.visited)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
        return neighbors;
    }
}
