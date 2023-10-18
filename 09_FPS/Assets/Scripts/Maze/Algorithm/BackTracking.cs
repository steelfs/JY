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
        
        cell.FindPath();
        //Recursive BackTracking
        //시작지점 고르기, 랜덤
        //해당 셀에  방문 표시
        //랜덤하게 네 방향 중 한 방향을 고른다.(방문한 적이 없는 셀만)
        // 네 방향 모두 방문했다면 이전 셀로 돌아간다.
        // 최종적으로 시작지점까지 돌아가면 알고리즘 종료
        //
    }
}
