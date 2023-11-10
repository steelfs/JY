using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Prim_Cell_Test : Cell
{
    public Prim_Cell_Test(int x, int y) : base(x, y)
    {

    }
}
public class Prim_Test : MazeGenerator_Test
{
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Prim_Cell_Test[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Prim_Cell_Test(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        HashSet<Prim_Cell_Test>inMaze = new HashSet<Prim_Cell_Test>(cells.Length);
        //���� ����� Ŀ���� ������ �ε����� ��Ҹ� �������µ� �ð��� ���� �ҿ�Ǹ�
        //frontiers ����inMaze ó�� HashSet�� �߰��� ����� �� �� �ִ�.
        List<Prim_Cell_Test> frontiers = new List<Prim_Cell_Test> ();

        Prim_Cell_Test first = cells[Random.Range(0, cells.Length)] as Prim_Cell_Test;
        inMaze.Add(first);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(first.X, first.Y));
        Prim_Cell_Test[] neighbors = GetNeighbors(first);
        foreach (Prim_Cell_Test neighbor in neighbors)
        {
            frontiers.Add(neighbor);
            on_Set_NextMaterial?.Invoke(GridToIndex(neighbor.X, neighbor.Y));
        }
        while (frontiers.Count > 0)
        {
            Prim_Cell_Test chosen = frontiers[Random.Range(0, frontiers.Count)];
            on_Set_ConfirmedMaterial?.Invoke(GridToIndex(chosen.X, chosen.Y));
            neighbors = GetNeighbors(chosen);//�����¿� ������ ���� ���� �͵� ��� ��������
            List<Prim_Cell_Test> tempList = new List<Prim_Cell_Test>();// ������ �� �� �̷ο� �߰��� ���� �ϳ��̻��� ��� �� ����Ʈ�� ���� �� �������� ���� �ȴ�.
            foreach (Prim_Cell_Test neighbor in neighbors)
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

            Prim_Cell_Test cell = tempList[Random.Range(0, tempList.Count)];
            MergeCell(chosen, cell);//����
            frontiers.Remove(chosen);//���õ� ���� frontiers ���� �����ϰ�
            inMaze.Add(chosen);//���õ� ���� inMaze �� �߰��Ѵ�.
        }
        GameManager.Visualizer_Test.InitBoard();


    }
    void MergeCell(Prim_Cell_Test from, Prim_Cell_Test to)
    {
      
        GameManager.Visualizer_Test.ConnectPath(from, to);
        GameManager.Visualizer_Test.AddToConnectOrder(from, to);
    }

}
