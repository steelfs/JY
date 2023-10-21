using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingVisualizer : MazeVisualizer
{
    Cell[] cells;
    public override void InitBoard(int x, int y)
    {
        // 알고리즘별 보드를 만들고 초기상태로 랜더
        RecursiveBackTracking backTracking = new RecursiveBackTracking();

        cells = backTracking.MakeCells(x, y);
    }
}
