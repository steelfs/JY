using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeGenerator
{
    protected int width;
    protected int height;

    protected Cell[] cells; 

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
    protected void ConnectPath(Cell from, Cell to)
    {
        Vector2Int diff = new(to.X - from.X, to.Y - from.Y);
        if (diff.x > 0)
        {
            //悼率
            from.MakePath(Direction.East);
            to.MakePath(Direction.West);
        }
        else if (diff.x < 0)
        {
            from.MakePath(Direction.West);
            to.MakePath(Direction.East);
            //辑率 
        }
        else if (diff.y > 0)
        {
            from.MakePath(Direction.South);
            to.MakePath(Direction.North);
            //巢率 
        }
        else if(diff.y < 0)
        {
            from.MakePath(Direction.North);
            to.MakePath(Direction.South);
            //合率 
        }
    }

    protected bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
    protected Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index / width);
    }
    protected int GridToIndex(int x, int y)
    {
        return x + y * width;
    }
    protected int GridToIndex(Vector2Int grid)
    {
        return grid.x + grid.y * width;
    }
    //protected vect
}
