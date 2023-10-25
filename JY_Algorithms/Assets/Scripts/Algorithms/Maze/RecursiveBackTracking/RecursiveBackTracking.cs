using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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
        List<BackTrackingCell> confirmedList = new List<BackTrackingCell>();
        stack.Push(start);
        confirmedList.Add(start);
        while(stack.Count > 0)
        {
            BackTrackingCell current = stack.Peek();
            List<BackTrackingCell> neighbors = GetNeighbors(current);
            BackTrackingCell next = null;
            while(neighbors.Count > 0)
            {
                BackTrackingCell chosen = neighbors[Random.Range(0, neighbors.Count - 1)];//next가 null 이 아닌데도 실행되는 경우가 있다.
                if (confirmedList.Contains(chosen))
                {
                    neighbors.Remove(chosen);
                }
                else
                {
                    next = chosen;
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
                confirmedList.Add(next);
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
