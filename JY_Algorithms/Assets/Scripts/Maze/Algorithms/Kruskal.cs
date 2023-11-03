using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class Kruskal_Cell : Cell
{
    public int group = 0;
    public Kruskal_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Kruskal : MazeGenerator
{
    Dictionary<int, List<Kruskal_Cell>> sets = new Dictionary<int, List<Kruskal_Cell>>();
    List<Kruskal_Cell> notInMaze = new List<Kruskal_Cell>();
   
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Kruskal_Cell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Kruskal_Cell(x, y);
                Kruskal_Cell cell = cells[index] as Kruskal_Cell;
                cell.group = index;//고유한 인덱스 부여

                sets[index] = new List<Kruskal_Cell> { cell };
                notInMaze.Add(cell);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        while(notInMaze.Count > 0)// 미로에 편입되지 않은것 부터 먼저 처리
        {
            Kruskal_Cell from = notInMaze[Random.Range(0, notInMaze.Count)];

            Kruskal_Cell[] neighbors = GetNeighbors(from);
            List<Kruskal_Cell> neighbors_List = neighbors.ToList();
            Kruskal_Cell to = null;
            while(neighbors_List.Count > 0)
            {
                to = neighbors_List[Random.Range(0, neighbors_List.Count)];
                if(to.group != from.group)
                {
                    break;
                }
                else
                {
                    neighbors_List.Remove(to);
                    to = null;
                    continue;
                }
            }
            if (to == null)
                continue;
            else
            {
                MergeCell(from, to);
            }
            await Task.Delay(300);
        }
        while(sets.Count > 1)
        {
            Kruskal_Cell from = cells[Random.Range(0, cells.Length)] as Kruskal_Cell;

            Kruskal_Cell[] neighbors = GetNeighbors(from);
            List<Kruskal_Cell> neighbors_List = neighbors.ToList();
            Kruskal_Cell to = null;
            while (neighbors_List.Count > 0)
            {
                to = neighbors_List[Random.Range(0, neighbors_List.Count)];
                if (to.group != from.group)
                {
                    break;
                }
                else
                {
                    neighbors_List.Remove(to);
                    to = null;
                    continue;
                }
            }
            if (to == null)
                continue;
            else
            {
                MergeCell(from, to);
            }
            await Task.Delay(300);
        }
    }
    void GetNeighborsAndMerge(Kruskal_Cell from)
    {

    }
    void MergeCell(Kruskal_Cell from, Kruskal_Cell to)
    {
        GameManager.Visualizer.ConnectPath(from, to);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(from.X, from.Y));
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(to.X, to.Y));
        notInMaze.Remove(from);
        notInMaze.Remove(to);
        List<Kruskal_Cell> fromSet = sets[from.group];
        List<Kruskal_Cell> toSet = sets[to.group];
        if (fromSet.Count > toSet.Count)
        {
            int toGroup = to.group;
            foreach(Kruskal_Cell toCell in toSet)
            {
                toCell.group = from.group;
                fromSet.Add(toCell);
            }
            sets.Remove(toGroup);
        }
        else
        {
            int fromGroup = from.group;
            foreach(Kruskal_Cell fromCell in fromSet)
            {
                fromCell.group = to.group;
                toSet.Add(fromCell);
            }
            sets.Remove(fromGroup);
        }

    }
}
