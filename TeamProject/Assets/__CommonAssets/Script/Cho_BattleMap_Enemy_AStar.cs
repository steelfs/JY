using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �±پ� AStar �ణ �պ� ���� 
/// ������ �����͸� �޾Ƽ� ó���ϵ��� ���� 
/// ���� �ٸ� ������Ʈ�� ������ �̿��ؾߵǼ� ���������� ����Ҽ�������� ����.
/// </summary>
public static class Cho_BattleMap_Enemy_AStar
{

    /// <summary>
    /// ��Ÿ�� ������ �ִܰŸ� �� Ÿ�ϸ���Ʈ�� ��ȯ
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

        Tile adjoinTile;                            // ������ Ÿ���� ����

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

                        adjoinTile = GetTile(map, current.Width + x, current.Length + y,sizeX);    // ������ Ÿ�� ��������
                        if (adjoinTile == current)                                          // ������ Ÿ���� (0, 0)�� ���
                            continue;
                        if (adjoinTile.ExistType != Tile.TileExistType.None)                // ������ Ÿ���� None�� �ƴ� ��
                            continue;
                        if (close.Exists((inClose) => inClose == adjoinTile))             // close����Ʈ�� ���� ��
                            continue;

                        bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                        if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
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
                        if (current.G + distance > moveSize) continue; //�̵����� ������ 

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
    /// ���� ��ġ�������� ��Ÿ� ���� ���� ������ ���� �� ��ǥ����Ʈ�� �������� �ش� ��ǥ�� 
    /// ���� ������ ������ �ִ��� ��ȯ�ϴ� �Լ� 
    /// </summary>
    /// <param name="currentNode">������ġ Ÿ�� ����</param>
    /// <param name="attackCheck">���ݰ����� �Ÿ� ��</param>
    /// <returns>���� ������ ������ ������ Ÿ�ϰ��� ��ȯ�ϰ� ������ null ��ȯ</returns>
    public static Tile SetEnemyAttackSize(Tile currentNode, float attackCheck)
    {
        List<Tile> openList = new List<Tile>();   // Ž���� �ʿ��� ��� ����Ʈ 
        List<Tile> closeList = new List<Tile>();  // �̹� ����� �Ϸ�Ǽ� ���̻� Ž���� ���� ����Ʈ 
        Tile[] mapTiles = SpaceSurvival_GameManager.Instance.BattleMap;
        int tileSizeX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int tileSizeY = SpaceSurvival_GameManager.Instance.MapSizeY;
        Tile playerTile = SpaceSurvival_GameManager.Instance.PlayerTeam[0].currentTile;
        foreach (Tile node in mapTiles)
        {
            node.H = 1000.0f; //���������� ���»��¶� �ƽ��� ������ ����� �ȵ��ư���.
            node.AttackCheckG = 1000.0f;
        }

        openList.Add(currentNode);

        currentNode.AttackCheckG = 0.0f; //����ġ�� g �� 0�̴�

        while (openList.Count > 0)
        {
            currentNode = openList[0];
            openList.Remove(currentNode); // Ž�������� ��Ͽ��� ���� Ž������ ����� �����ϰ� 
            closeList.Add(currentNode);   // Ž�������� ����Ʈ�� ���� ����� ��´�.

            if (currentNode.AttackCheckG > attackCheck) //G ���� ���� �̵� ������ �Ÿ����� ������  ���̻� Ž���� �ʿ�������� 
            {
                continue; //������ Ž�� 
            }
            else // �̵������� �Ÿ���
            {
                //�÷��̾� �� �ִ� ��ġ�� 
                if (currentNode.width == playerTile.width && currentNode.length == playerTile.length) 
                {
                    return currentNode; // �÷��̾� ��ġ��ȯ
                }
            }

            OpenListAdd(mapTiles, tileSizeX, tileSizeY, currentNode, openList, closeList); //�ֺ� 8������ ��带 ã�Ƽ� G�� �����ϰ�  ���¸���Ʈ�� ������������ ��´�.
            openList.Sort();            //ã�� G���� ���� ���������� ��Ž���̵ȴ�.
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
                if (currentNode.Width + x < 0 || currentNode.Width + x > tileSizeX - 1 || // ���̵� �˻� 
                    currentNode.Length + y < 0 || currentNode.Length + y > tileSizeY - 1) //���̵� �˻�
                    continue;

                adjoinTile = Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length + y, tileSizeX);    // ������ Ÿ�� ��������

                if (adjoinTile == currentNode)                                          // ������ Ÿ���� (0, 0)�� ���
                    continue;
                if (adjoinTile.ExistType == Tile.TileExistType.Prop)                // ������ Ÿ���� ��ֹ��϶�
                    continue;
                bool isDiagonal = (x * y != 0);                                     // �밢�� ���� Ȯ��
                if (isDiagonal &&                                                   // �밢���̰� ���� Ÿ���� �����¿찡 ���� ��
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width + x, currentNode.Length, tileSizeX).ExistType == Tile.TileExistType.Prop ||
                    Cho_BattleMap_AStar.GetTile(mapTiles, currentNode.Width, currentNode.Length + y, tileSizeX).ExistType == Tile.TileExistType.Prop
                    )
                    continue;
                //�밢�� üũ ������ �̵��� �ȵǴµ� ������ �Ǹ� �ȵɰŰ��Ƽ� ���ܵ״�.
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
