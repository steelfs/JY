using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    Tilemap background;//���� ��� (��üũ�� Ȯ�ο�) Ÿ�ϸ�
    Tilemap obstacle;//��, ��ֹ� Ȯ�ο� Ÿ�ϸ�

    Spawner[] spawners;

    GridMap gridMap;//Ÿ�ϸ����� ������ �׸����
    public GridMap GridMap { get => gridMap; }

    private void Awake()
    {
        Transform grid = transform.parent;
        Transform tilemap = grid.GetChild(0);
        background = tilemap.GetComponent<Tilemap>();
        tilemap = grid.GetChild(1);
        obstacle = tilemap.GetComponent<Tilemap>();

        gridMap = new GridMap(background, obstacle);
        spawners = GetComponentsInChildren<Spawner>();
    }

    /// <summary>
    /// //���������� ������ �̸� ã�µ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="spawner">���������ѿ����� ã�� ������</param>
    /// <returns>�������� ������ ���� ������ ������ ����Ʈ</returns>
    public List<Node> CalculateSpawnArea(Spawner spawner)
    {
        List<Node> result = new List<Node>();

        Vector2Int min = gridMap.WorldToGrid(spawner.transform.position);
        Vector2Int max = gridMap.WorldToGrid(spawner.transform.position + (Vector3)spawner.size);

        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                if (!gridMap.IsWall(x, y))
                {
                    result.Add(gridMap.GetNode(x, y));
                }
            }
        }
        return result;
    }

    public Vector2 GridToWorld(int x, int y)
    {
        return gridMap.GridToWorld(new Vector2Int(x, y));
    }
}
