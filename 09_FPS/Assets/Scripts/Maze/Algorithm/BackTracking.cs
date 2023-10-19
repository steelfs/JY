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
                cells[GridToIndex(x, y)] = new BackTrackingCell (x, y);
            }
        }
        BackTrackingCell startCell = cells[0] as BackTrackingCell;
        startCell.visited = true;

        List<(int, int)> marker = new List<(int, int)> ();//��� ����׿� 

        Stack<BackTrackingCell> stack = new Stack<BackTrackingCell>();
        stack.Push(startCell);
        while(stack.Count > 0)
        {
            BackTrackingCell current = stack.Peek();
            List<BackTrackingCell> unvisitedList = GetUnvisitedList(current);
            if (unvisitedList.Count > 0)
            {
                BackTrackingCell chosen = unvisitedList[Random.Range(0, unvisitedList.Count)];

                marker.Add((chosen.X, chosen.Y));

                chosen.visited = true;
                ConnectPath(current, chosen);
                stack.Push(chosen);
            }
            else
            {
                stack.Pop();
            }

        }
        foreach ((int x, int y) in marker)
        {
            Debug.Log($"x_{x}  y_{y}");
        }
        //x_{x}  y_{y}//
        //Recursive BackTracking
        //�������� ����, ����
        //�ش� ����  �湮 ǥ��
        //�����ϰ� �� ���� �� �� ������ ����.(�湮�� ���� ���� ����)
        // �� ���� ��� �湮�ߴٸ� ���� ���� ���ư���.
        // ���������� ������������ ���ư��� �˰��� ����
        //
    }

    List<BackTrackingCell> GetUnvisitedList(BackTrackingCell current)
    {
        List<BackTrackingCell> unvisitedList = new List<BackTrackingCell>();
        
        int[,] dir = { {-1, 0 }, {1, 0 }, {0, -1 }, {0, 1 } };
        for (int i = 0; i < 4; i++)
        {
            int x = current.X + dir[i, 0];
            int y = current.Y + dir[i, 1];
            if (IsInGrid(x, y))
            {
                BackTrackingCell neighbor = cells[GridToIndex(x, y)] as BackTrackingCell;
                if (!neighbor.visited)
                {
                    unvisitedList.Add(neighbor);
                }
            }
        }
        return unvisitedList;
    }
}
