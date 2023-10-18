using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeGenerator
{
    protected int width;
    protected int height;

    public Cell[] cells; 

    public Cell[] MakeMaze(int width, int height, int seed = -1)
    {
        this.width = width;
        this.height = height;
        if (seed != -1)
        {
            Random.InitState(seed);
        }

        cells = new Cell[width * height];
        OnSpecificAlgorithmExcute();

        return cells;

    }



    protected virtual void OnSpecificAlgorithmExcute()
    {

    }
    public void ConnectPath(Cell from, Cell to)
    {
        Vector2Int diff = new(to.X - from.X, to.Y - from.Y);
        if (diff.x > 0)
        {
            //����
            from.MakePath(Direction.East);
            to.MakePath(Direction.West);
        }
        else if (diff.x < 0)
        {
            from.MakePath(Direction.West);
            to.MakePath(Direction.East);
            //���� 
        }
        else if (diff.y > 0)
        {
            from.MakePath(Direction.South);
            to.MakePath(Direction.North);
            //���� 
        }
        else if(diff.y < 0)
        {
            from.MakePath(Direction.North);
            to.MakePath(Direction.South);
            //���� 
        }
    }

    public bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
    public bool IsInGrid(int index)
    {
        Vector2Int grid = IndexToGrid(index);
        return IsInGrid(grid.x, grid.y);
    }
    public Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index / width);
    }
    public int GridToIndex(int x, int y)
    {
        return x + y * width;
    }
    public int GridToIndex(Vector2Int grid)
    {
        return grid.x + grid.y * width;
    }
    //protected vect
}
