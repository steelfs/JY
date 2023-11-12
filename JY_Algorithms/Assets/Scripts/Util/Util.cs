using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class Util
{
    public static void Shuffle<T>(T[] source)
    {
        for(int i=source.Length-1; i>-1; i--)
        {
            int index = UnityEngine.Random.Range(0, i);
            (source[index], source[i]) = (source[i], source[index]);
        }
    }
    //public static Vector2Int WorldToGrid()
    //{

    //}
    public static Vector3 GridToWorld(Vector2Int grid)
    {
        CellVisualizer_Test cellVisualizer = GameManager.Visualizer.CellVisualizers[GridToIndex(grid.x, grid.y)];
        Vector3 result = cellVisualizer.transform.position;

        return result;
    }
    public static Vector2Int WorldToGrid(Vector3 world)
    {
        int x = (int)(world.x / MazeVisualizer_Test.CellSize);
        int y = (int)(world.z / -MazeVisualizer_Test.CellSize);
        return new Vector2Int(x, y);
    }
    public static Vector2Int GetRandomGrid()//플레이어가 스폰될 랜덤한 포지션과 로테이션값을 out으로 주는 함수
    {
        Cell[] cells = GameManager.Kruskal.cells;
        Cell cell = cells[UnityEngine.Random.Range(0, cells.Length)];
        Vector2Int result = new Vector2Int(cell.X, cell.Y);

        return result;
    }
    //gridToWorld, WorldToGrid 작성할 차례
    public static Vector3 GetRandomRotation(int x, int y)
    {
        int index = GridToIndex(x, y);
        CellVisualizer_Test cellVisualizer = GameManager.Visualizer.CellVisualizers[index];
        List<Vector3> angles = new List<Vector3>();
        for (int i = 1; i < 5; i++)
        {
            if (cellVisualizer.transform.GetChild(i).gameObject.activeSelf == false)
            {
                angles.Add(new Vector3(0, 90 * (i - 1), 0));
            }
        }
        Vector3 result = angles[UnityEngine.Random.Range(0, angles.Count)];
        return result;
    }
    public static bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < GameManager.Kruskal.width && y < GameManager.Kruskal.height;
    }
    public static int GridToIndex(int x, int y)
    {
        return (y * GameManager.Kruskal.width) + x;
    }
    public static int GridToIndex(Vector2Int grid)
    {
        return GridToIndex(grid.x, grid.y);
    }
    public static Vector2Int IndexToGrid(int index)
    {
        int y = index / GameManager.Kruskal.height;
        int x = index % GameManager.Kruskal.height;
        return new Vector2Int(x, y);
    }
    public static T[] GetNeighbors<T>(T current) where T : Cell
    {
        int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        List<T> neighbors = new List<T>();
        for (int i = 0; i < 4; i++)
        {
            int X = current.X + dir[i, 0];
            int Y = current.Y + dir[i, 1];
            if (IsInGrid(X, Y))
            {
                neighbors.Add((T)GameManager.Kruskal.cells[GridToIndex(X, Y)]);
            }
        }
        return neighbors.ToArray();
    }
}
