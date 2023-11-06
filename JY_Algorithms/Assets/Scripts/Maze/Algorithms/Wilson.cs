using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Sprites;
using UnityEngine;

public class WilsonCell : Cell
{
    public WilsonCell Next { get; set; } = null;
    public WilsonCell Prev { get; set; } = null;
    public WilsonCell(int x, int y) : base(x, y)
    {

    }
}
public class Wilson : MazeGenerator
{
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new WilsonCell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new WilsonCell(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        
        Dictionary<WilsonCell, WilsonCell> confirmedSet = new Dictionary<WilsonCell, WilsonCell> ();//Ž���ӵ��� ���̱� ���� �߰��� ���ε� �̰����� ���� Dictionary�� ���� ���� ���ߴ�. 
        List<WilsonCell> confirmed = new List<WilsonCell> ();
        List<WilsonCell> notConfirmed = new List<WilsonCell>(cells as WilsonCell[]);
        WilsonCell[] arr = notConfirmed.ToArray();
        Util.Shuffle(arr);
        notConfirmed = arr.ToList();
        Stack<WilsonCell> path = new Stack<WilsonCell> ();//�����̴� ��θ� ������ ����
        Dictionary<WilsonCell, WilsonCell> pathSet = new Dictionary<WilsonCell, WilsonCell> ();//���� ������ � ���⶧���� �˻��ӵ��� �� ���� ���� ��. �̰� ���� �ʹ� ���ߴ�.


        WilsonCell first = cells[UnityEngine.Random.Range(0, cells.Length)] as WilsonCell;//ó�� �������� ����
        confirmedSet.Add(first, first);
        confirmed.Add(first);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(first.X, first.Y));//��Ƽ���� ���� ��ȣ
        notConfirmed.Remove(first);



        WilsonCell start = notConfirmed[0];// ������ Shuffle�� ���� ���ұ� ������ �ϳ��� �̾� �ᵵ ������ ���� �������� �ȴ�.
        path.Push(start);//��� ���ؿ� �߰�
        pathSet.Add(start, start);

        while(notConfirmed.Count > 0)
        {
            WilsonCell current = path.Peek();//��� �� ���� �ֱٿ� �߰��Ȱ��� ��������
            WilsonCell[] neighbors = GetNeighbors(current)as WilsonCell[];// �����¿� ���� �����´�.(���� ���� ���� �ٱ����� ����)
            WilsonCell next = null;//������ ���� �� �����ϰ� �ϳ��� �������� ����
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
                    WilsonCell picked = path.Pop();

                    int pickedIndex = GridToIndex(picked.X, picked.Y);
                    on_Set_DefaultMaterial?.Invoke(pickedIndex);//��ο��� �����Ʊ� ������ ��ο� �߰��� ��Ƽ����� �����ƴ� ���� �ٽ� Default��Ƽ����� �ٲ۴�.
                    await Task.Delay(100);
                    pathSet.Remove(picked);
                }
            }
            else if (confirmedSet.ContainsKey(next))//�������� �������� ���
            {
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(next.X, next.Y));
                foreach (WilsonCell picked in path)//path�� �߰��Ǿ��ִ� ������ Ȯ�� ����Ʈ�� �߰���Ű�鼭 ��Ƽ�� �ٲ��ش�.
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
                    WilsonCell newStartPoint = notConfirmed[0];
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

