using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    public static List<Tile> PathFind(MapTest map, Tile start, Tile end)
    {
        const float sideDistance = 1.0f;
        const float diagonalDistance = 1.414f;

        List<Tile> path = null;
        
        List<Tile> open = new List<Tile>();
        List<Tile> close = new List<Tile>();

        map.ClearTile();

        Tile current = start;
        current.G = 0;
        current.H = GetHeuristic(current, end);
        open.Add(current);

        Tile adjoinTile;                            // 인접한 타일의 변수


        while (open.Count > 0)
        {
            open.Sort();
            current = open[0];
            open.RemoveAt(0);

            if (current != end)
            {
                close.Add(current);

                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        if (current.Width + x < 0 || current.Width + x > map.sizeX - 1 ||
                            current.Length + y < 0 || current.Length + y > map.sizeY - 1)
                            continue;

                        adjoinTile = map.GetTile(current.Width + x, current.Length + y);    // 인접한 타일 가져오기

                        if (adjoinTile == current)                                          // 인접한 타일이 (0, 0)인 경우
                            continue;
                        if (adjoinTile.ExistType != Tile.TileExistType.None)                // 인접한 타일이 None이 아닐 때
                            continue;
                        if (close.Exists( (inClose) => inClose == adjoinTile ))             // close리스트에 있을 때
                            continue;

                        bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                        if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
                            (map.IsWall(current.Width + x, current.Length) ||
                            map.IsWall(current.Width, current.Length + y)))
                            continue;

                        float distance;
                        if (isDiagonal)
                        {
                            distance = diagonalDistance;
                        }
                        else
                        {
                            distance = sideDistance;
                        }

                        if (adjoinTile.G > current.G + distance)
                        {
                            if (adjoinTile.parent == null)
                            {
                                adjoinTile.H = GetHeuristic(adjoinTile, end);
                                open.Add(adjoinTile);
                            }
                            adjoinTile.G = current.G + distance;
                            adjoinTile.parent = current;
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }

        if (current == end)
        {
            path = new List<Tile>();
            while (current.parent != null)
            {
                path.Add(current);
                current = current.parent;
            }

            path.Reverse();
        }

        return path;
    }

    private static float GetHeuristic(Tile current, Tile end)
    {
        return Mathf.Abs(end.Width - current.Width) + Mathf.Abs(end.Length - current.Length);
    }








   

}
