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

    public override void MakeMaze()
    {
        BackTrackingCell start = cells[Random.Range(0, cells.Length)] as BackTrackingCell;

        Stack<BackTrackingCell> stack = new Stack<BackTrackingCell>();
        stack.Push(start);
        while(stack.Count > 0)
        {
            BackTrackingCell current = stack.Peek();
            List<BackTrackingCell> neighbors = GetNeighbors(current);
            BackTrackingCell next = null;
            foreach (var cell in neighbors)
            {
                if (stack.Contains(cell))
                {
                    continue;
                }
                else
                {
                    next = cell;
                    break;
                }
            }
            if (next == null)
            {
                stack.Pop();
            }
            else
            {
                stack.Push(next);
                GameManager.BackTrackingVisualizer.AddToConnectOrder(current, next);
                //튜플리스트에 추가
            }
        }
    }
    List<BackTrackingCell> GetNeighbors(BackTrackingCell current)
    {
        List<BackTrackingCell> neighbors = new List<BackTrackingCell>();
        int[,] dirs = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        for (int i = 0; i < 4; i++)
        {
            int X = current.X + dirs[i, 0];
            int Y = current.Y + dirs[i, 1];

            if (IsInGrid(X, Y))
            {
                neighbors.Add(cells[GridToIndex(X, Y)] as BackTrackingCell);
            }
        }
        return neighbors;

    }


}
