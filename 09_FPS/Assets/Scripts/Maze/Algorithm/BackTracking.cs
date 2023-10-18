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
                cells[GridToIndex(x, y)] = new BackTrackingCell (x, y);
            }
        }
        BackTrackingCell startCell = cells[0] as BackTrackingCell;
        startCell.visited = true;
        int visitCount = 1;

        Stack<BackTrackingCell> stack = new Stack<BackTrackingCell>();
        stack.Push(startCell);
        while(stack.Count > 0 && visitCount < cells.Length)
        {
            BackTrackingCell current = stack.Peek();
            List<BackTrackingCell> unvisitedList = GetUnvisitedList(current);
            if (unvisitedList.Count > 0)
            {
                BackTrackingCell chosen = unvisitedList[Random.Range(0, unvisitedList.Count)];
                chosen.visited = true;
                ConnectPath(current, chosen);
                stack.Push(chosen);
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

    List<BackTrackingCell> GetUnvisitedList(BackTrackingCell current)
    {
        List<BackTrackingCell> unvisitedList = new List<BackTrackingCell>();
        
        int[,] dir = { {-1, 0 }, {1, 0 }, {0, -1 }, {0, 1 } };
        for (int i = 0; i < 4; i++)
        {
            int index = GridToIndex(current.X + dir[i, 0], current.Y + dir[i, 1]);
            if (IsInGrid(index))
            {
                BackTrackingCell neighbor = cells[index] as BackTrackingCell;
                if (!neighbor.visited)
                {
                    unvisitedList.Add(neighbor);
                }
            }
        }
        return unvisitedList;
    }
}
