using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public BackTrackingCell(int x, int y) : base(x, y)
    {

    }
}
public class RecursiveBackTracking : MazeGenerator
{
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new BackTrackingCell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new BackTrackingCell(x, y);
            }
        }
        return cells;
    }

}
