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
    Dictionary<int, List<Kruskal_Cell>> sets = new Dictionary<int, List<Kruskal_Cell>>();//�׷��� Ű�� �ָ� ���� �׷��� ������ ����Ʈ�� �ٷ� ������ �� �ִ� ��ųʸ�
    List<Kruskal_Cell> notInMaze = new List<Kruskal_Cell>();//���� �̷ο� �߰����� ���� ������ ����Ʈ
    Kruskal_Cell previousFrom;//��Ƽ���� �����
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

                cell.group = index;//������ �ε��� �ο�
                sets[index] = new List<Kruskal_Cell> { cell };
                notInMaze.Add(cell);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        List<(Kruskal_Cell, Kruskal_Cell)> remainCells = new List<(Kruskal_Cell, Kruskal_Cell)> ();
        for (int i = 0; i < cells.Length; i++)
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

            if (unionFind.Find(fromIndex) != unionFind.Find(toIndex))
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
            }

        }

        //while(notInMaze.Count > 0)// �̷ο� ���Ե��� ������ ���� ���� ó��
        //{
        //    GetNeighborsAndMerge(true);
        //    await Task.Delay(300);
        //}
        //while(sets.Count > 1)
        //{
        //    GetNeighborsAndMerge(false);
        //    await Task.Delay(300);
        //}
        //GameManager.Visualizer.InitBoard();
    }
    void GetNeighborsAndMerge(bool getFromNotInMaze)
    {
        Kruskal_Cell[] neighbors; 
        List<Kruskal_Cell> neighbors_List;
        Kruskal_Cell from = null;
        Kruskal_Cell to = null;
        while (to == null)
        {
            if (getFromNotInMaze)
            {
                from = notInMaze[Random.Range(0, notInMaze.Count)];
            }
            else
            {
                from = cells[Random.Range(0, cells.Length)] as Kruskal_Cell;
            }
            neighbors = GetNeighbors(from);//�����¿� �� ���ؿ���
            neighbors_List = neighbors.ToList();
            while (neighbors_List.Count > 0)
            {
                to = neighbors_List[Random.Range(0, neighbors_List.Count)];//���ؿ� ���� �� ������ �� ����
                if (to.group != from.group)//�׷��� �ٸ��� toCell �� null �� �ƴ�ä�� ���� ����
                {
                    break;
                }
                else
                {
                    neighbors_List.Remove(to);
                    to = null;
                }
            }
        }
        MergeCell(from, to);//����
    }
    void MergeCell(Kruskal_Cell from, Kruskal_Cell to)
    {
        GameManager.Visualizer.ConnectPath(from, to);
        GameManager.Visualizer.AddToConnectOrder(from, to);
        if (previousFrom != null)
        {
            on_Set_DefaultMaterial?.Invoke(GridToIndex(previousFrom.X, previousFrom.Y));//������ ǥ���� ��Ƽ���� �ʱ�ȭ
            on_Set_DefaultMaterial?.Invoke(GridToIndex(previousTo.X, previousTo.Y));
        }
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(from.X, from.Y));//���� �۾��� ���� ��Ƽ���� ǥ��
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(to.X, to.Y));
        previousFrom = from;
        previousTo = to;

        notInMaze.Remove(from);
        notInMaze.Remove(to);
        List<Kruskal_Cell> fromSet = sets[from.group];//����Ʈ ��������
        List<Kruskal_Cell> toSet = sets[to.group];

        //ī��Ʈ�� �������� ū�ʿ� �����Ѵ�.  ������� �����ص� ������µ� ������ Ŀ���� �ð��� ���ݴ� Ŀ���� ���� ����ߴ�.
        if (fromSet.Count > toSet.Count)
        {
            int toGroup = to.group;
            foreach(Kruskal_Cell toCell in toSet)
            {
                toCell.group = from.group;
                fromSet.Add(toCell);
            }
            sets.Remove(toGroup);
        }
        else
        {
            int fromGroup = from.group;
            foreach(Kruskal_Cell fromCell in fromSet)
            {
                fromCell.group = to.group;
                toSet.Add(fromCell);
            }
            sets.Remove(fromGroup);
        }

    }
}
