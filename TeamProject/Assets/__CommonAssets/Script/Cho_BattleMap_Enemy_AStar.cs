using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 승근씨 AStar 약간 손본 로직 
/// 생성된 데이터만 받아서 처리하도록 변경 
/// 기존 다른 컴포넌트의 로직을 이용해야되서 독립적으로 사용할수가없었어서 수정.
/// </summary>
public static class Cho_BattleMap_Enemy_AStar
{

    /// <summary>
    /// 두타일 사이의 최단거리 의 타일리스트를 반환
    /// </summary>
    /// <param name="map"></param>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static List<Tile> PathFind(Tile[] map, int sizeX, int sizeY, Tile start, Tile end, float moveSize = 1000.0f)
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
                        if (adjoinTile.ExistType != Tile.TileExistType.None)                // 인접한 타일이 None이 아닐 때
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
                        if (current.G + distance > moveSize) continue; //이동범위 벗어난경우 

                        if (adjoinTile.G > current.G + distance)
                        {
                            if (adjoinTile.parent == null)
                            {
                                open.Add(adjoinTile);
                            }
                            adjoinTile.H = GetHeuristic(adjoinTile, end);
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
        path = new List<Tile>();
        while (close.Count > 0) 
        {
            if (current.H > close[0].H) 
            {
                current = close[0];
            }
            close.RemoveAt(0);
        }
        while (current.parent != null)
        {
            path.Add(current);
            current = current.parent;
        }

        //Debug.Log($"{current.width},{current.length}, {end.width}, {end.length}");
        path.Reverse();

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

  

    /// <summary>
    /// 현재 위치지점에서 사거리 기준 공격 가능한 범위 의 좌표리스트를 가져오고 해당 좌표에 
    /// 공격 가능한 유닛이 있는지 반환하는 함수 
    /// </summary>
    /// <param name="currentNode">현재위치 타일 정보</param>
    /// <param name="attackCheck">공격가능한 거리 값</param>
    /// <returns>공격 가능한 유닛이 있으면 타일값을 반환하고 없으면 null 반환</returns>
    public static Tile SetEnemyAttackSize(Tile currentNode, float attackCheck)
    {
        List<Tile> openList = new List<Tile>();   // 탐색이 필요한 노드 리스트 
        List<Tile> closeList = new List<Tile>();  // 이미 계산이 완료되서 더이상 탐색을 안할 리스트 
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
        Tile playerTile = SpaceSurvival_GameManager.Instance.PlayerTeam[0].currentTile;
        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; //도착지점이 없는상태라서 맥스값 넣으니 제대로 안돌아간다.
            node.AttackCheckG = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.AttackCheckG = 0.0f; //내위치는 g 가 0이다

        while (openList.Count > 0)
        {
            currentNode = openList[0];
            openList.Remove(currentNode); // 탐색가능한 목록에서 현재 탐색중인 목록을 제거하고 
            closeList.Add(currentNode);   // 탐색종료한 리스트에 현재 목록을 담는다.

            if (currentNode.AttackCheckG > attackCheck) //G 값이 현재 이동 가능한 거리보다 높으면  더이상 탐색이 필요없음으로 
            {
                continue; //다음거 탐색 
            }
            else // 이동가능한 거리고
            {
                //플레이어 가 있는 위치면 
                if (currentNode.width == playerTile.width && currentNode.length == playerTile.length) 
                {
                    return currentNode; // 플레이어 위치반환
                }
            }

            OpenListAdd(mapTiles, tileSizeX, tileSizeY, currentNode, openList, closeList); //주변 8방향의 노드를 찾아서 G값 수정하고  오픈리스트에 담을수있으면 담는다.
            openList.Sort();            //찾은 G값중 가장 작은값부터 재탐색이된다.
        }
        return null;
    }
    private static void OpenListAdd(Tile[] mapTiles, int tileSizeX, int tileSizeY, Tile currentNode, List<Tile> open, List<Tile> close)
    {
        Tile adjoinTile;
        float sideDistance = 1.0f;
        float diagonalDistance = 1.414f;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // 사이드 검색 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //사이드 검색
                    continue;

                adjoinTile = Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length + y, tileSizeX);    // 인접한 타일 가져오기

                if (adjoinTile == currentNode)                                          // 인접한 타일이 (0, 0)인 경우
                    continue;
                if (adjoinTile.ExistType == Tile.TileExistType.Prop)                // 인접한 타일이 장애물일때
                    continue;
                bool isDiagonal = (x * y != 0);                                     // 대각선 유무 확인
                if (isDiagonal &&                                                   // 대각선이고 현재 타일의 상하좌우가 벽일 때
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length, tileSizeX).ExistType == Tile.TileExistType.Prop ||
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width, currentNode.Length + y, tileSizeX).ExistType == Tile.TileExistType.Prop
                    )
                    continue;
                //대각선 체크 이유는 이동도 안되는데 공격은 되면 안될거같아서 남겨뒀다.
                float distance;
                if (isDiagonal)
                {
                    distance = diagonalDistance;
                }
                else
                {
                    distance = sideDistance;
                }

                if (adjoinTile.AttackCheckG > currentNode.AttackCheckG + distance)
                {
                    open.Add(adjoinTile);
                    adjoinTile.AttackCheckG = currentNode.AttackCheckG + distance;
                }
            }
        }

    }



}
