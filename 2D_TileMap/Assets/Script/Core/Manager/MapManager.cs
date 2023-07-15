using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    Tilemap background;//맵의 배경 (전체크기 확인용) 타일맵
    Tilemap obstacle;//벽, 장애물 확인용 타일맵

    Spawner[] spawners;

    GridMap gridMap;//타일맵으로 생성한 그리드맵
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
    /// //스폰가능한 영역을 미리 찾는데 사용하는 함수
    /// </summary>
    /// <param name="spawner">스폰가능한영역을 찾을 스포너</param>
    /// <returns>스포너의 영역중 스폰 가능한 지역의 리스트</returns>
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
