using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

public class BackTrackingCell_Test : Cell
{
    public BackTrackingCell_Test prev { get; set; }
    public BackTrackingCell_Test next { get; set; }
    public BackTrackingCell_Test(int x, int y) : base(x, y)
    {

    }
}
public class BackTracking_Test : MazeGenerator_Test
{
    /// <summary>
    /// 셀 프리팹이 아닌 내부 Cell 의 배열을 만드는 함수
    /// </summary>
    /// <param name="width">cells배열의 가로</param>
    /// <param name="height">배열의 새로</param>
    /// <returns></returns>
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new BackTrackingCell_Test[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new BackTrackingCell_Test(x, y);
            }
        }
        return cells;
    }

    /// <summary>
    /// 미로를 만드는 함수
    /// </summary>
    public override async void MakeMaze()
    {
        int[] resultArr = new int[4];
        BackTrackingCell_Test start = cells[Random.Range(0, cells.Length)] as BackTrackingCell_Test;

        Stack<BackTrackingCell_Test> stack = new Stack<BackTrackingCell_Test>();
        List<BackTrackingCell_Test> confirmedList = new List<BackTrackingCell_Test>();
        stack.Push(start);
        confirmedList.Add(start);
        while(stack.Count > 0)
        {
            BackTrackingCell_Test current = stack.Peek();
            List<BackTrackingCell_Test> neighborsList = GetNeighborsArr(current).ToList();

            BackTrackingCell_Test next = null;
            while(neighborsList.Count > 0)
            {
                BackTrackingCell_Test chosen = neighborsList[0];//next가 null 이 아닌데도 실행되는 경우가 있다.
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
                BackTrackingCell_Test cell = stack.Pop();
                await Task.Delay(100);
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(cell.X, cell.Y));
            }
            else
            {
                current.next = next;
                next.prev = current;
                
                stack.Push(next);
                confirmedList.Add(next);
                GameManager.Visualizer_Test.AddToConnectOrder(current, next);
                await Task.Delay(100);
                on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));
                on_Set_NextMaterial?.Invoke(GridToIndex(next.X, next.Y));
                //튜플리스트에 추가
            }
        }
        foreach(BackTrackingCell_Test cell in cells)
        {
            on_Set_DefaultMaterial?.Invoke(GridToIndex(cell.X, cell.Y));
            await Task.Delay(30);
        }
    }
    BackTrackingCell_Test[] GetNeighborsArr(BackTrackingCell_Test current)
    {
        BackTrackingCell_Test[] neighbors;// = new BackTrackingCell[4];
        int[,] dirs = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        List<BackTrackingCell_Test> list = new List<BackTrackingCell_Test>();
        for (int i = 0; i < 4; i++)
        {
            int X = current.X + dirs[i, 0];
            int Y = current.Y + dirs[i, 1];

            if (IsInGrid(X, Y))
            {
                list.Add(cells[GridToIndex(X, Y)] as BackTrackingCell_Test);
                //neighbors[i] = (cells[GridToIndex(X, Y)] as BackTrackingCell);
            }
        }

        neighbors = list.ToArray();
        Util.Shuffle(neighbors);
        return neighbors;
    }

  
}
