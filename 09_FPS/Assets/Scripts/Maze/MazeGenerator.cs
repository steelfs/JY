using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    protected int width;
    protected int height;

    protected Cell[] cells;
    protected Cell[] Exits;
    Cell goal;
    public Cell Goal => goal;
    int exitIndex = 0;
    public int ExitIndex => exitIndex;
    public Cell[] MakeMaze(int width, int height, int seed = -1)
    {
        this.width = width;
        this.height = height;
        if(seed != -1)
        {            
            Random.InitState(seed);
        }
        
        cells = new Cell[width * height];

        OnSpecificAlgorithmExcute();
        SetExitPoint(width, height);
        return cells;
    }
    void SetExitPoint(int width, int height)
    {
        Direction dir = (Direction)(1 << Random.Range(0, 4));
        int lastLine = cells.Length - width;
        int lastX = width - 1;

        int exit = 0;
        Vector2Int goalGrid;

        switch (dir)
        {
            case Direction.North:
                exit = Random.Range(0, width);
                goalGrid = IndexToGrid(exit);
                goalGrid.y--;
                break;
            case Direction.East:
                exit = Random.Range(0, height) * width + width - 1;
                goalGrid = IndexToGrid(exit);
                goalGrid.x++;
                break;
            case Direction.South:
                exit = Random.Range(0, width) + width * (height - 1);
                goalGrid = IndexToGrid(exit);
                goalGrid.y++;
                break;
            case Direction.West:
                exit = Random.Range(0, height) * width;
                goalGrid = IndexToGrid(exit);
                goalGrid.x--;

                break;
            default:
                goalGrid = Vector2Int.zero;
                break;
        }
        goal = new(goalGrid.x, goalGrid.y);
        ConnectPath(cells[exit], goal);

        
        //ConnectPath(cells[exit], goal);

        //for (int i = 0; i < cells.Length; i++)
        //{
        //    if (i < width)
        //    {
        //        //첫번째줄
        //    }
        //    else if (i > lastLine)
        //    {
        //        //마지막중 
        //    }
        //    else if (i % width == 0)
        //    {
        //        //왼쪽줄
        //    }
        //    else if(i % width == lastX)
        //    {

        //    }
        //}




        //Cell northWest = cells[0];
        //Cell northEast = cells[GridToIndex(width - 1, 0)];
        //Cell southEast = cells[GridToIndex(width - 1, height - 1)];
        //Cell southWest = cells[GridToIndex(0, width - 1)];
        //Exits = new Cell[] {northEast, northWest, southEast, southWest };

        //Cell chosen = Exits[Random.Range(0, Exits.Length)];

        //int[,] dir_ = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        //for (int i = 0; i < 4; i++)
        //{
        //    int x = chosen.X + dir_[i, 0];
        //    int y = chosen.Y + dir_[i, 1];
        //    if (!IsInGrid(x, y))
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                chosen.MakePath(Direction.East);
        //                break;
        //            case 1:
        //                chosen.MakePath(Direction.West);
        //                break;
        //            case 2:
        //                chosen.MakePath(Direction.North);
        //                break;
        //            case 3:
        //                chosen.MakePath(Direction.South);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
    }
    protected virtual void OnSpecificAlgorithmExcute()
    {
    }

    /// <summary>
    /// 두 셀 사이에 벽을 제거하는 함수
    /// </summary>
    /// <param name="from">시작셀</param>
    /// <param name="to">도착셀</param>
    protected void ConnectPath(Cell from, Cell to)
    {
        Vector2Int diff = new(to.X - from.X, to.Y - from.Y);

        if(diff.x > 0)
        {
            // 동쪽
            from.MakePath(Direction.East);
            to.MakePath(Direction.West);
        }
        else if(diff.x < 0)
        {
            // 서쪽
            from.MakePath(Direction.West);
            to.MakePath(Direction.East);
        }
        else if (diff.y > 0)
        {
            // 남쪽
            from.MakePath(Direction.South);
            to.MakePath(Direction.North);
        }
        else if (diff.y < 0)
        {
            // 북쪽
            from.MakePath(Direction.North);
            to.MakePath(Direction.South);
        }
    }

    protected bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    protected bool IsInGrid(Vector2Int grid)
    {
        return grid.x >= 0 && grid.y >= 0 && grid.x < width && grid.y < height;
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

}
