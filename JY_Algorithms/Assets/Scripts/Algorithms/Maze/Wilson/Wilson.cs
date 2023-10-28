using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WilsonCell : Cell
{
    public WilsonCell Next { get; set; } = null;
    public WilsonCell Prev { get; set; } = null;
    public WilsonCell(int x, int y) : base(x, y)
    {

    }
}
public class Wilson : MazeGenerator
{
    public Action<int> on_Set_PathMaterial;
    public Action<int> on_Set_DefaultMaterial;
    public Action<int> on_Set_ConfirmedMaterial;
    public Action<int> on_Set_NextMaterial;

    WaitForSeconds duration = new WaitForSeconds(1);
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new WilsonCell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new WilsonCell(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        Dictionary<WilsonCell, WilsonCell> confirmed = new Dictionary<WilsonCell, WilsonCell> ();
        List<WilsonCell> notConfirmed = new List<WilsonCell>(cells as WilsonCell[]);
        WilsonCell[] arr = notConfirmed.ToArray();
        Util.Shuffle(arr);
        notConfirmed = arr.ToList();
        Stack<WilsonCell> path = new Stack<WilsonCell> ();
        Dictionary<WilsonCell, WilsonCell> pathSet = new Dictionary<WilsonCell, WilsonCell> ();


        WilsonCell first = cells[UnityEngine.Random.Range(0, cells.Length)] as WilsonCell;
        confirmed.Add(first, first);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(first.X, first.Y));
        notConfirmed.Remove(first);



        WilsonCell start = notConfirmed[0] ;
        path.Push(start);
        pathSet.Add(start, start);

        int endPoint = 0;
        
        while(confirmed.Count < cells.Length)
        {
            WilsonCell current = path.Peek();
            WilsonCell[] neighbors = GetNeighbors(current)as WilsonCell[];
            WilsonCell next = neighbors[UnityEngine.Random.Range(0, neighbors.Length)];
            on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));
            on_Set_NextMaterial?.Invoke(GridToIndex(next.X, next.Y));
           await Task.Delay(500);
            if (pathSet.ContainsKey(next))//이미 지나온 경로일 경우
            {
                while (path.Peek() != next) 
                {
                    WilsonCell picked = path.Pop();

                    int pickedIndex = GridToIndex(picked.X, picked.Y);
                    on_Set_DefaultMaterial?.Invoke(pickedIndex);
                    await Task.Delay(200);
                    pathSet.Remove(picked);
                }
            }
            else if (confirmed.ContainsKey(next))//도착지에 도착했을 경우
            {
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(next.X, next.Y));
                foreach (WilsonCell picked in path)
                {
                    confirmed[picked] = picked;
                    on_Set_ConfirmedMaterial?.Invoke(GridToIndex(picked.X, picked.Y));
                    await Task.Delay(200);
                    notConfirmed.Remove(picked);
                    pathSet.Remove(picked);

                    GameManager.Visualizer.AddToConnectOrder(picked, picked.Next);
                    //stack이라 순서가 거꾸로 들어가지만 이거대로 나쁘지 않을것 같아서 그냥 진행했다.
                    //confirmed가 딕셔너리가 아니라 그냥 리스트였으면 모두 끝나고 한번에 진행하면 될 일
                }
                path.Clear();
                current.Next = next;
                next.Prev = current;

                WilsonCell newStartPoint = notConfirmed[0];
                path.Push(newStartPoint);
                pathSet[newStartPoint] = newStartPoint;
            }
            else
            {
                current.Next = next;
                next.Prev = current;
                path.Push(next);
                pathSet[next] = next;
            }
            

            endPoint++;
        }
    }
  
    protected override Cell[] GetNeighbors(Cell current)
    {
        int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        WilsonCell[] dirs = null;
        List<WilsonCell> neighbors = new List<WilsonCell>();
        WilsonCell current_ = current as WilsonCell;
        for (int i = 0; i < 4; i++)
        {
            int X = current.X + dir[i, 0];
            int Y = current.Y + dir[i, 1];
            if (IsInGrid(X, Y))
            {
                WilsonCell newCell = cells[GridToIndex(X, Y)] as WilsonCell;

                if (current_.Prev == newCell)
                    continue;

                neighbors.Add(newCell);
            }
        }
        dirs = neighbors.ToArray();
        Util.Shuffle(dirs);
        return dirs;
    }
}

