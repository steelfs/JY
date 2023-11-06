using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prim_Cell : Cell
{
    public Prim_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Prim : MazeGenerator
{
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
                cells[index] = new EllerCell(x, y);
                Prim_Cell cell = cells[index] as Prim_Cell;
            }
        }
        return cells;
    }
    public override void MakeMaze()
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

                }
            }
        }



    }
    void MergeCell(Prim_Cell from, Prim_Cell to)
    {
        GameManager.Visualizer.ConnectPath(from, to);
        GameManager.Visualizer.AddToConnectOrder(from, to);
    }

}
