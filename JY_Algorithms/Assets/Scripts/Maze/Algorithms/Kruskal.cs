using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class Kruskal_Cell : Cell
{
    public int group = 0;
    public Kruskal_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Kruskal : MazeGenerator
{
    Dictionary<int, List<Kruskal_Cell>> sets = new Dictionary<int, List<Kruskal_Cell>>();
    List<Kruskal_Cell> notInMaze = new List<Kruskal_Cell>();
   
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Kruskal_Cell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Kruskal_Cell(x, y);
                Kruskal_Cell cell = cells[index] as Kruskal_Cell;
                cell.group = index;//고유한 인덱스 부여

                sets[index] = new List<Kruskal_Cell> { cell };
                notInMaze.Add(cell);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        while(notInMaze.Count > 0)
        {
            Kruskal_Cell chosen = notInMaze[Random.Range(0, notInMaze.Count)];
            Kruskal_Cell[] neighbors = GetNeighbors(chosen);
            await Task.Delay(30);
        }
    }
  
}
