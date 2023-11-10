using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Sprites;
using UnityEngine;

public class WilsonCell_Test : Cell
{
    public WilsonCell_Test Next { get; set; } = null;
    public WilsonCell_Test Prev { get; set; } = null;
    public WilsonCell_Test(int x, int y) : base(x, y)
    {

    }
}
public class Wilson_Test : MazeGenerator_Test
{
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new WilsonCell_Test[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new WilsonCell_Test(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        
        Dictionary<WilsonCell_Test, WilsonCell_Test> confirmedSet = new Dictionary<WilsonCell_Test, WilsonCell_Test> ();//Ž���ӵ��� ���̱� ���� �߰��� ���ε� �̰����� ���� Dictionary�� ���� ���� ���ߴ�. 
        List<WilsonCell_Test> confirmed = new List<WilsonCell_Test> ();
        List<WilsonCell_Test> notConfirmed = new List<WilsonCell_Test>(cells as WilsonCell_Test[]);
        WilsonCell_Test[] arr = notConfirmed.ToArray();
        Util.Shuffle(arr);
        notConfirmed = arr.ToList();
        Stack<WilsonCell_Test> path = new Stack<WilsonCell_Test> ();//�����̴� ��θ� ������ ����
        Dictionary<WilsonCell_Test, WilsonCell_Test> pathSet = new Dictionary<WilsonCell_Test, WilsonCell_Test> ();//���� ������ � ���⶧���� �˻��ӵ��� �� ���� ���� ��. �̰� ���� �ʹ� ���ߴ�.


        WilsonCell_Test first = cells[UnityEngine.Random.Range(0, cells.Length)] as WilsonCell_Test;//ó�� �������� ����
        confirmedSet.Add(first, first);
        confirmed.Add(first);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(first.X, first.Y));//��Ƽ���� ���� ��ȣ
        notConfirmed.Remove(first);



        WilsonCell_Test start = notConfirmed[0];// ������ Shuffle�� ���� ���ұ� ������ �ϳ��� �̾� �ᵵ ������ ���� �������� �ȴ�.
        path.Push(start);//��� ���ؿ� �߰�
        pathSet.Add(start, start);

        while(notConfirmed.Count > 0)
        {
            WilsonCell_Test current = path.Peek();//��� �� ���� �ֱٿ� �߰��Ȱ��� ��������
            WilsonCell_Test[] neighbors = GetNeighbors(current)as WilsonCell_Test[];// �����¿� ���� �����´�.(���� ���� ���� �ٱ����� ����)
            WilsonCell_Test next = null;//������ ���� �� �����ϰ� �ϳ��� �������� ����
            while(next == null)
            {
                next = neighbors[UnityEngine.Random.Range(0, neighbors.Length)];
                if (next == current.Prev)
                {
                    next = null;
                }
            }


            on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));
            on_Set_NextMaterial?.Invoke(GridToIndex(next.X, next.Y));//��Ƽ���� ����
            await Task.Delay(100); //0.1�� ������
            if (pathSet.ContainsKey(next))//�̹� ������ ����� ���
            {
                while (path.Peek() != next) //����� ���� ���� ����  �������� ������ ���� �ٸ��� ����ؼ� Pop���� ���ÿ��� ������.
                {
                    WilsonCell_Test picked = path.Pop();

                    int pickedIndex = GridToIndex(picked.X, picked.Y);
                    on_Set_DefaultMaterial?.Invoke(pickedIndex);//��ο��� �����Ʊ� ������ ��ο� �߰��� ��Ƽ����� �����ƴ� ���� �ٽ� Default��Ƽ����� �ٲ۴�.
                    await Task.Delay(100);
                    pathSet.Remove(picked);
                }
            }
            else if (confirmedSet.ContainsKey(next))//�������� �������� ���
            {
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(next.X, next.Y));
                foreach (WilsonCell_Test picked in path)//path�� �߰��Ǿ��ִ� ������ Ȯ�� ����Ʈ�� �߰���Ű�鼭 ��Ƽ�� �ٲ��ش�.
                {
                    confirmedSet[picked] = picked;
                    confirmed.Add(picked);
                    on_Set_ConfirmedMaterial?.Invoke(GridToIndex(picked.X, picked.Y));//path�� 
                    await Task.Delay(100);
                    notConfirmed.Remove(picked);
                    pathSet.Remove(picked);

                    //GameManager.Visualizer.AddToConnectOrder(picked, picked.Next);
                    //stack�̶� ������ �Ųٷ� ������ �̰Ŵ�� ������ ������ ���Ƽ� �׳� �����ߴ�.
                    //confirmed�� ��ųʸ��� �ƴ϶� �׳� ����Ʈ������ ��� ������ �ѹ��� �����ϸ� �� ��
                }
                path.Clear();
                current.Next = next;
                next.Prev = current;
                if (notConfirmed.Count > 0)
                {
                    WilsonCell_Test newStartPoint = notConfirmed[0];
                    path.Push(newStartPoint);
                    pathSet[newStartPoint] = newStartPoint;
                }
            }
            else
            {
                current.Next = next;
                next.Prev = current;
                path.Push(next);
                pathSet[next] = next;
            }
        }
        int i = 0;
        while(i < cells.Length)
        {
            on_Set_DefaultMaterial?.Invoke(i);
            await Task.Delay(30);
            i++;
        }
        for ( i = 0; i < confirmed.Count ; i++)
        {
            GameManager.Visualizer.AddToConnectOrder(confirmed[i], confirmed[i].Next);
        }
    }
  
    //protected override Cell[] GetNeighbors(Cell current)
    //{
    //    int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
    //    WilsonCell[] dirs = null;
    //    List<WilsonCell> neighbors = new List<WilsonCell>();
    //    WilsonCell current_ = current as WilsonCell;
    //    for (int i = 0; i < 4; i++)
    //    {
    //        int X = current.X + dir[i, 0];
    //        int Y = current.Y + dir[i, 1];
    //        if (IsInGrid(X, Y))
    //        {
    //            WilsonCell newCell = cells[GridToIndex(X, Y)] as WilsonCell;

    //            if (current_.Prev == newCell)
    //                continue;

    //            neighbors.Add(newCell);
    //        }
    //    }
    //    dirs = neighbors.ToArray();
    //    Util.Shuffle(dirs);
    //    return dirs;
    //}


}
