using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator 
{
    public int width;
    public int height;
    public Cell[] cells = null;

    public virtual Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
    
        return null;
    }
    public virtual void MakeMaze()
    {

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
}
