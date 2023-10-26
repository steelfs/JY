using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        int[] resultArr = new int[4];
        BackTrackingCell start = cells[Random.Range(0, cells.Length)] as BackTrackingCell;

        Stack<BackTrackingCell> stack = new Stack<BackTrackingCell>();
        List<BackTrackingCell> confirmedList = new List<BackTrackingCell>();
        stack.Push(start);
        confirmedList.Add(start);
        while(stack.Count > 0)
        {
            BackTrackingCell current = stack.Peek();
            List<BackTrackingCell> neighborsList = GetNeighborsArr(current).ToList();

            BackTrackingCell next = null;
            while(neighborsList.Count > 0)
            {
                BackTrackingCell chosen = neighborsList[0];//next가 null 이 아닌데도 실행되는 경우가 있다.
                if (confirmedList.Contains(chosen))
                {
                    neighborsList.RemoveAt(0);
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
    BackTrackingCell[] GetNeighborsArr(BackTrackingCell current)
    {
        BackTrackingCell[] neighbors;// = new BackTrackingCell[4];
        int[,] dirs = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        List<BackTrackingCell> list = new List<BackTrackingCell>();
        for (int i = 0; i < 4; i++)
        {
            int X = current.X + dirs[i, 0];
            int Y = current.Y + dirs[i, 1];

            if (IsInGrid(X, Y))
            {
                list.Add(cells[GridToIndex(X, Y)] as BackTrackingCell);
                //neighbors[i] = (cells[GridToIndex(X, Y)] as BackTrackingCell);
            }
        }

        neighbors = list.ToArray();
        Util.Shuffle(neighbors);
        return neighbors;
    }

  
}
