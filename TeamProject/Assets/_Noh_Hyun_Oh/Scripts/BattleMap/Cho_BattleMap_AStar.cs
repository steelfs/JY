using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 승근씨 AStar 약간 손본 로직 
/// 생성된 데이터만 받아서 처리하도록 변경 
/// 기존 다른 컴포넌트의 로직을 이용해야되서 독립적으로 사용할수가없었어서 수정.
/// </summary>
public static class Cho_BattleMap_AStar
{
    public static List<Tile> PathFind(Tile[] map, int sizeX, int sizeY, Tile start, Tile end)
    {
        int tileLength = sizeX * sizeY;
        const float sideDistance = 1.0f;
        const float diagonalDistance = 1.414f;

        List<Tile> path = null;

        List<Tile> open = new List<Tile>();
        List<Tile> close = new List<Tile>();

        for (int i = 0; i < tileLength; i++)
        {
            map[i].Clear();
        }

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
                        if (current.Width + x < 0 || current.Width + x > sizeX - 1 ||
                            current.Length + y < 0 || current.Length + y > sizeY - 1)
                            continue;

                        adjoinTile = GetTile(map, current.Width + x, current.Length + y,sizeX);    // 인접한 타일 가져오기

                        if (adjoinTile == current)                                          // 인접한 타일이 (0, 0)인 경우
                            continue;
                        if (adjoinTile.ExistType != Tile.TileExistType.Move && adjoinTile.ExistType != Tile.TileExistType.Item)                // 인접한 타일이 None이 아닐 때
                            continue;
                        if (close.Exists((inClose) => inClose == adjoinTile))             // close리스트에 있을 때
                            continue;

                        bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                        if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
                            (
                             GetTile(map,current.width + x,current.length,sizeX).ExistType == Tile.TileExistType.Prop ||
                             GetTile(map, current.width, current.Length + y, sizeX).ExistType == Tile.TileExistType.Prop
                            ))
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

    public static Tile GetTile(Tile[] map, int width, int length, int sizeX)
    {
        int index = sizeX * length + width;
        return map[index];
    }





}
