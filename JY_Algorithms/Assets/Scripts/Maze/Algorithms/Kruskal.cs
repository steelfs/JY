using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class UnionFind
{
    int[] parent = null;
    public UnionFind(int cap)
    {
        parent = new int[cap];
        for (int i = 0; i < parent.Length; i++)
        {
            parent[i] = i;
        }
    }
    public int Find(int index)
    {
        if (parent[index] != index)
        {
            parent[index] = Find(parent[index]);
        }
        return parent[index];
    }
    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);
        if (rootX > rootY)
        {
            parent[rootX] = rootY;
        }
        else if (rootX < rootY)
        {
            parent[rootY] = rootX;
        }
        else
        {
            return;
        }
    }

}
public class Kruskal_Cell : Cell
{
    public int group = 0;
    public Kruskal_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Kruskal : MazeGenerator
{
    Kruskal_Cell previousFrom;//머티리얼 변경용
    Kruskal_Cell previousTo;
    UnionFind unionFind;
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
                unionFind = new(width * height);
                cell.group = index;//고유한 인덱스 부여
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        List<(Kruskal_Cell, Kruskal_Cell)> remainCells = new List<(Kruskal_Cell, Kruskal_Cell)> ();
        for (int i = 0; i < cells.Length; i++)//u+연결할 수 있는 모든 경우의 수를 저장
        {
            Kruskal_Cell ftom = cells[i] as Kruskal_Cell;
            Kruskal_Cell[] neighbors = GetNeighbors(ftom);
            foreach(Kruskal_Cell to in neighbors)
            {
                remainCells.Add((ftom, to));
            }
        }
        while(remainCells.Count > 0)
        {
            int index = Random.Range(0, remainCells.Count);
            (Kruskal_Cell from, Kruskal_Cell to) = remainCells[index];
            remainCells.RemoveAt(index);
            int fromIndex = GridToIndex(from.X, from.Y);
            int toIndex = GridToIndex(to.X, to.Y);

            int fromUnion = unionFind.Find(fromIndex);
            int toUnion = unionFind.Find(toIndex);
            if (fromUnion != toUnion)
            {
                if (previousFrom != null)
                {
                    on_Set_DefaultMaterial?.Invoke(GridToIndex(previousFrom.X, previousFrom.Y));//이전에 표시한 머티리얼 초기화
                    on_Set_DefaultMaterial?.Invoke(GridToIndex(previousTo.X, previousTo.Y));
                }
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(from.X, from.Y));//현재 작업할 셀의 머티리얼 표시
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(to.X, to.Y));
                previousFrom = from;
                previousTo = to;
                GameManager.Visualizer.ConnectPath(cells[fromIndex], cells[toIndex]);
                await Task.Delay(200);
                unionFind.Union(fromIndex, toIndex);
                GameManager.Visualizer.AddToConnectOrder(cells[GridToIndex(from.X, from.Y)], cells[GridToIndex(to.X, to.Y)]);
            }
        }
        GameManager.Visualizer.InitBoard();
    }
}
