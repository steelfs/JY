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
public class Kruskal_Cell_Test : Cell
{
    public int group = 0;
    public Kruskal_Cell_Test(int x, int y) : base(x, y)
    {

    }
}
public class Kruskal_Test : MazeGenerator_Test
{
    Kruskal_Cell_Test previousFrom;//��Ƽ���� �����
    Kruskal_Cell_Test previousTo;
    UnionFind unionFind;
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Kruskal_Cell_Test[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Kruskal_Cell_Test(x, y);
                Kruskal_Cell_Test cell = cells[index] as Kruskal_Cell_Test;
                unionFind = new(width * height);
                cell.group = index;//������ �ε��� �ο�
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        List<(Kruskal_Cell_Test, Kruskal_Cell_Test)> remainCells = new List<(Kruskal_Cell_Test, Kruskal_Cell_Test)> ();
        for (int i = 0; i < cells.Length; i++)//u+������ �� �ִ� ��� ����� ���� ����
        {
            Kruskal_Cell_Test ftom = cells[i] as Kruskal_Cell_Test;
            Kruskal_Cell_Test[] neighbors = GetNeighbors(ftom);
            foreach(Kruskal_Cell_Test to in neighbors)
            {
                remainCells.Add((ftom, to));
            }
        }
        while(remainCells.Count > 0)
        {
            int index = Random.Range(0, remainCells.Count);
            (Kruskal_Cell_Test from, Kruskal_Cell_Test to) = remainCells[index];
            remainCells.RemoveAt(index);
            int fromIndex = GridToIndex(from.X, from.Y);
            int toIndex = GridToIndex(to.X, to.Y);

            int fromUnion = unionFind.Find(fromIndex);
            int toUnion = unionFind.Find(toIndex);
            if (fromUnion != toUnion)
            {
                if (previousFrom != null)
                {
                    on_Set_DefaultMaterial?.Invoke(GridToIndex(previousFrom.X, previousFrom.Y));//������ ǥ���� ��Ƽ���� �ʱ�ȭ
                    on_Set_DefaultMaterial?.Invoke(GridToIndex(previousTo.X, previousTo.Y));
                }
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(from.X, from.Y));//���� �۾��� ���� ��Ƽ���� ǥ��
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
