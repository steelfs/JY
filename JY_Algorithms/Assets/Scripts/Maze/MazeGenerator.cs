using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    public int width;
    public int height;
    public Cell[] cells = null;

    public Action<int> on_Set_PathMaterial;
    public Action<int> on_Set_DefaultMaterial;
    public Action<int> on_Set_ConfirmedMaterial;
    public Action<int> on_Set_NextMaterial;
    public virtual Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Cell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Cell(x, y);
            }
        }
        return cells;
    }
    public virtual void MakeMaze()
    {

    }
    protected bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
    public int GridToIndex(int x, int y)
    {
        return (y * width) + x;
    }
    public int GridToIndex(Vector2Int grid)
    {
        return GridToIndex(grid.x, grid.y);
    }
    public Vector2Int IndexToGrid(int index)
    {
        int y = index / height;
        int x = index % height;
        return new Vector2Int(x, y);
    }
    protected T[] GetNeighbors<T>(T current) where T : Cell
    {
        int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        List<T> neighbors = new List<T>();
        for (int i = 0; i < 4; i++)
        {
            int X = current.X + dir[i, 0];
            int Y = current.Y + dir[i, 1];
            if (IsInGrid(X, Y))
            {
                neighbors.Add((T)cells[GridToIndex(X, Y)]);
            }
        }
        return neighbors.ToArray();
    }
}
