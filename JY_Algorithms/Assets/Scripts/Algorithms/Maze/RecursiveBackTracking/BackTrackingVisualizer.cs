using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingVisualizer : MazeVisualizer
{

    public override void MakeBoard(int x, int y)
    {
        // �˰��� ���带 ����� �ʱ���·� ����
        RecursiveBackTracking backTracking = new RecursiveBackTracking();

        Cells = backTracking.MakeCells(x, y);
        RenderBoard(x, y, Cells);
    }
    public override void InitBoard()
    {
        base.InitBoard();
    }
    public override void StartConnect()
    {
        base.StartConnect();
    }
    public override void StopConnect()
    {
        base.StopConnect();
    }
    public override void RenderBoard(int width, int height, Cell[] cells)
    {
        base.RenderBoard(width, height, cells);
    }
}
