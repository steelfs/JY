using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingVisualizer : MazeVisualizer
{
    Cell[] cells;
    public override void InitBoard(int x, int y)
    {
        // �˰��� ���带 ����� �ʱ���·� ����
        RecursiveBackTracking backTracking = new RecursiveBackTracking();

        cells = backTracking.MakeCells(x, y);
    }
}
