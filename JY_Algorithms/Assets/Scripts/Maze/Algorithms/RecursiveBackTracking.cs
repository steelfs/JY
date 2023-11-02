using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

public class BackTrackingCell : Cell
{
    public BackTrackingCell prev { get; set; }
    public BackTrackingCell next { get; set; }
    public BackTrackingCell(int x, int y) : base(x, y)
    {

    }
}
public class RecursiveBackTracking : MazeGenerator
{
    /// <summary>
    /// �� �������� �ƴ� ���� Cell �� �迭�� ����� �Լ�
    /// </summary>
    /// <param name="width">cells�迭�� ����</param>
    /// <param name="height">�迭�� ����</param>
    /// <returns></returns>
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

    /// <summary>
    /// �̷θ� ����� �Լ�
    /// </summary>
    public override async void MakeMaze()
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
                BackTrackingCell chosen = neighborsList[0];//next�� null �� �ƴѵ��� ����Ǵ� ��찡 �ִ�.
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
                BackTrackingCell cell = stack.Pop();
                await Task.Delay(100);
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(cell.X, cell.Y));
            }
            else
            {
                current.next = next;
                next.prev = current;
                
                stack.Push(next);
                confirmedList.Add(next);
                GameManager.Visualizer.AddToConnectOrder(current, next);
                await Task.Delay(100);
                on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));
                on_Set_NextMaterial?.Invoke(GridToIndex(next.X, next.Y));
                //Ʃ�ø���Ʈ�� �߰�
            }
        }
        foreach(BackTrackingCell cell in cells)
        {
            on_Set_DefaultMaterial?.Invoke(GridToIndex(cell.X, cell.Y));
            await Task.Delay(30);
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
