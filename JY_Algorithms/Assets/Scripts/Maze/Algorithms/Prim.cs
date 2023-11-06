using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Prim_Cell : Cell
{
    public Prim_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Prim : MazeGenerator
{
    Prim_Cell previous_From;
    Prim_Cell previous_To;
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Prim_Cell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Prim_Cell(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        HashSet<Prim_Cell>inMaze = new HashSet<Prim_Cell>(cells as Prim_Cell[]);
        List<Prim_Cell> frontiers = new List<Prim_Cell> ();

        Prim_Cell first = cells[Random.Range(0, cells.Length)] as Prim_Cell;
        inMaze.Add(first);
        Prim_Cell[] neighbors = GetNeighbors(first);
        foreach (Prim_Cell neighbor in neighbors)
        {
            frontiers.Add(neighbor);
        }
        while (frontiers.Count > 0)
        {
            Prim_Cell chosen = frontiers[Random.Range(0, frontiers.Count)];

            neighbors = GetNeighbors(chosen);
            foreach (Prim_Cell neighbor in neighbors)
            {
                if (inMaze.Contains(neighbor))
                {
                    MergeCell(neighbor, chosen);
                    await Task.Delay(200);
                    frontiers.Remove(chosen);
                    inMaze.Add(chosen);
                }
            }
            foreach (Prim_Cell neighbor in neighbors)
            {
                if (inMaze.Contains(neighbor))
                {
                    continue;
                }
                else
                {
                    frontiers.Add(neighbor);
                }
            }
        }



    }
    void MergeCell(Prim_Cell from, Prim_Cell to)
    {
        if (previous_From != null)
        {
            on_Set_DefaultMaterial?.Invoke(GridToIndex(previous_From.X, previous_From.Y));
            on_Set_DefaultMaterial?.Invoke(GridToIndex(previous_To.X, previous_To.Y));
        }
        on_Set_NextMaterial?.Invoke(GridToIndex(from.X, from.Y));
        on_Set_NextMaterial?.Invoke(GridToIndex(to.X, to.Y));
        previous_From = from;
        previous_To = to;
        GameManager.Visualizer.ConnectPath(from, to);
        GameManager.Visualizer.AddToConnectOrder(from, to);
    }

}
