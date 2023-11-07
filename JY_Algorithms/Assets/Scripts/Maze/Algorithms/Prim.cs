using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Prim_Cell : Cell
{
    public Prim_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Prim : MazeGenerator
{
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Prim_Cell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Prim_Cell(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        HashSet<Prim_Cell>inMaze = new HashSet<Prim_Cell>(cells.Length);
        //���� ����� Ŀ���� ������ �ε����� ��Ҹ� �������µ� �ð��� ���� �ҿ�Ǹ�
        //frontiers ����inMaze ó�� HashSet�� �߰��� ����� �� �� �ִ�.
        List<Prim_Cell> frontiers = new List<Prim_Cell> ();

        Prim_Cell first = cells[Random.Range(0, cells.Length)] as Prim_Cell;
        inMaze.Add(first);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(first.X, first.Y));
        Prim_Cell[] neighbors = GetNeighbors(first);
        foreach (Prim_Cell neighbor in neighbors)
        {
            frontiers.Add(neighbor);
            on_Set_NextMaterial?.Invoke(GridToIndex(neighbor.X, neighbor.Y));
        }
        while (frontiers.Count > 0)
        {
            Prim_Cell chosen = frontiers[Random.Range(0, frontiers.Count)];
            on_Set_ConfirmedMaterial?.Invoke(GridToIndex(chosen.X, chosen.Y));
            neighbors = GetNeighbors(chosen);//�����¿� ������ ���� ���� �͵� ��� ��������
            List<Prim_Cell> tempList = new List<Prim_Cell>();// ������ �� �� �̷ο� �߰��� ���� �ϳ��̻��� ��� �� ����Ʈ�� ���� �� �������� ���� �ȴ�.
            foreach (Prim_Cell neighbor in neighbors)
            {
                if (inMaze.Contains(neighbor))//���� �̷ο� �߰��Ǿ��ִ� �� �̶�� �ĺ�����Ʈ�� �߰�
                {
                    tempList.Add(neighbor);
                }
                else
                {
                    if (!frontiers.Contains(neighbor))//�ƴ϶�� �ĺ�����Ʈ�� �߰�
                    {
                        frontiers.Add(neighbor);
                        on_Set_NextMaterial?.Invoke(GridToIndex(neighbor.X, neighbor.Y));
                    }
                }
            }
            await Task.Delay(200);

            Prim_Cell cell = tempList[Random.Range(0, tempList.Count)];
            MergeCell(chosen, cell);//����
            frontiers.Remove(chosen);//���õ� ���� frontiers ���� �����ϰ�
            inMaze.Add(chosen);//���õ� ���� inMaze �� �߰��Ѵ�.
        }
        GameManager.Visualizer.InitBoard();


    }
    void MergeCell(Prim_Cell from, Prim_Cell to)
    {
      
        GameManager.Visualizer.ConnectPath(from, to);
        GameManager.Visualizer.AddToConnectOrder(from, to);
    }

}
