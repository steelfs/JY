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
            int index = UnityEngine.Random.Range(0, i + 1);
            (source[index], source[i]) = (source[i], source[index]);
        }
    }
    //public static Vector2Int WorldToGrid()
    //{

    //}
    public static Vector3 GridToWorld(Vector2Int grid)
    {
        CellVisualizer cellVisualizer = GameManager.Visualizer.CellVisualizers[GridToIndex(grid.x, grid.y)];
        Vector3 result = cellVisualizer.transform.position;

        return result;
    }
    public static Vector2Int WorldToGrid(Vector3 world)
    {
        int x = Mathf.RoundToInt(world.x);
        int y = Mathf.RoundToInt((world.z * -1));

        if(x % MazeVisualizer_Test.CellSize > 2)
        {
            x = (x / MazeVisualizer_Test.CellSize) + 1;
        }
        else
        {
            x = (x / MazeVisualizer_Test.CellSize);
        }
        if (y % MazeVisualizer_Test.CellSize > 2)
        {
            y = (y / MazeVisualizer_Test.CellSize) + 1;
        }
        else
        {
            y = (y / MazeVisualizer_Test.CellSize);
        }

        return new Vector2Int(x, y);
    }
    public static Vector2Int GetRandomGrid()//플레이어가 스폰될 랜덤한 포지션과 로테이션값을 out으로 주는 함수
    {
        Cell[] cells = GameManager.Kruskal.cells;
        Cell cell = cells[UnityEngine.Random.Range(0, cells.Length)];
        Vector2Int result = new Vector2Int(cell.X, cell.Y);

        return result;
    }
    public static Vector2Int[] Get_StartingPoints()
    {
        int width = GameManager.Kruskal.width;
        int height = GameManager.Kruskal.height;
        Cell[] cells = GameManager.Kruskal.cells;
        Vector2Int[] positions = new Vector2Int[4];  
        Cell north_West = cells[0];
        Cell northEast = cells[width - 1];
        Cell southWest = cells[width * (height - 1)];
        Cell southEast = cells[(width * height) - 1];
        positions[0] = new Vector2Int(north_West.X, north_West.Y);
        positions[1] = new Vector2Int(northEast.X, northEast.Y);
        positions[2] = new Vector2Int(southWest.X, southWest.Y);
        positions[3] = new Vector2Int(southEast.X, southEast.Y);

        Shuffle(positions);
        return positions;
    }
    //gridToWorld, WorldToGrid 작성할 차례
    public static Vector3 GetRandomRotation(int x, int y)
    {
        int index = GridToIndex(x, y);
        CellVisualizer cellVisualizer = GameManager.Visualizer.CellVisualizers[index];
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
    public static Vector2Int[] GetNeighbors(int x, int y)
    {
        int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for (int i = 0; i < 4; i++)
        {
            int X = x + dir[i, 0];
            int Y = y + dir[i, 1];
            if (IsInGrid(X, Y))
            {
                neighbors.Add(new Vector2Int(X, Y));
            }
        }
        return neighbors.ToArray();
    }
    public static bool IsNeighbor(Vector2Int from, Vector2Int to, int condition)
    {
        if (from.x != to.x && from.y != to.y)
        {
            return false;
        }
        bool result = false;
        int diffX = from.x - to.x;
        int diffY = from.y - to.y;
        if ((diffX > -condition && diffX < condition) && (diffY > -condition && diffY < condition))
        {
            result = true;
        }
        return result;
    }
}
