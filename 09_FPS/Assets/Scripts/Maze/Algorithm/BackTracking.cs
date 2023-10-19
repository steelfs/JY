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


        //재귀문 호출
        MakeRecursive(start.X, start.Y);

        //Recursive BackTracking
        //시작지점 고르기, 랜덤
        //해당 셀에  방문 표시
        //랜덤하게 네 방향 중 한 방향을 고른다.(방문한 적이 없는 셀만)
        // 네 방향 모두 방문했다면 이전 셀로 돌아간다.
        // 최종적으로 시작지점까지 돌아가면 알고리즘 종료
        //
    }

    void MakeRecursive(int x, int y)
    {
        BackTrackingCell current = cells[GridToIndex(x, y)] as BackTrackingCell;
        Vector2Int[] dirs = { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
        Util.Shuffle(dirs);
        //shuffle
        foreach (Vector2Int dir in dirs)
        {
            Vector2Int pos = new Vector2Int(x + dir.x, y + dir.y);
            if (IsInGrid(pos.x, pos.y))
            {
                BackTrackingCell neighbor = cells[GridToIndex(pos.x, pos.y)] as BackTrackingCell;
                if (!neighbor.visited)
                {
                    neighbor.visited = true;
                    ConnectPath(current, neighbor);

                    MakeRecursive(neighbor.X, neighbor.Y);

                    //재귀호출은 뒤로 돌아가는 코드가 없어도  자연스럽게 뒤로 돌아가게 된다.
                    // 예 >  (1, 6) => (1, 7) 에서 오는 neighbor.visited == true 가 되면 뒤로 돌아가는 코드가 없어도 다시 1, 6으로 돌아가게 된다. 
                }
            }
        }
    }
}
