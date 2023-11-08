using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator_ : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;

    private const int N = 1, S = 2, E = 4, W = 8;
    private Dictionary<int, Vector2Int> DX = new Dictionary<int, Vector2Int>
    {
        {E, new Vector2Int(1, 0)}, {W, new Vector2Int(-1, 0)}, {N, new Vector2Int(0, -1)}, {S, new Vector2Int(0, 1)}
    };
    private Dictionary<int, int> OPPOSITE = new Dictionary<int, int> { { E, W }, { W, E }, { N, S }, { S, N } };

    private List<List<int>> grid;
    private System.Random rand = new System.Random();

    void Start()
    {
        StartCoroutine(GenerateMaze());
    }

    private void InitializeGrid()
    {
        grid = new List<List<int>>();
        for (int i = 0; i < height; i++)
        {
            grid.Add(new List<int>(new int[width]));
        }
    }

    private IEnumerator GenerateMaze()
    {
        InitializeGrid();

        int x = rand.Next(width), y = rand.Next(height);
        grid[y][x] |= N;

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(new Vector2Int(x, y));

        while (stack.Count > 0)
        {
            x = stack.Peek().x;
            y = stack.Peek().y;
            List<int> directions = new List<int> { N, S, E, W };

            bool moved = false;
            while (directions.Count > 0 && !moved)
            {
                int dir = directions[rand.Next(directions.Count)];
                directions.Remove(dir);

                int nx = x + DX[dir].x, ny = y + DX[dir].y;
                if (nx >= 0 && ny >= 0 && nx < width && ny < height && grid[ny][nx] == 0)
                {
                    grid[y][x] |= dir;
                    grid[ny][nx] |= OPPOSITE[dir];
                    stack.Push(new Vector2Int(nx, ny));
                    moved = true;
                }
            }

            if (!moved)
            {
                stack.Pop();
            }

            yield return null; // This will make Unity wait for the next frame before continuing
        }

        PrintMaze(); // Or any other method to display the maze
    }

    private void PrintMaze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Create walls based on the grid value
                // For example, if a cell's value does not have a South flag, create a South wall
                // You would instantiate wallPrefab as needed here
            }
        }
    }
}
