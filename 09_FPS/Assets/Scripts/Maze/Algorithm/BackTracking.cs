using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;

    public BackTrackingCell(int x, int y) : base(x, y) // base먼저 실행 후  실행 
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
        //시작지점 고르기, 랜덤
        //해당 셀에  방문 표시
        //랜덤하게 네 방향 중 한 방향을 고른다.(방문한 적이 없는 셀만)
        // 네 방향 모두 방문했다면 이전 셀로 돌아간다.
        // 최종적으로 시작지점까지 돌아가면 알고리즘 종료
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
