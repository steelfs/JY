using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;

    public BackTrackingCell(int x, int y) : base(x, y) // base���� ���� ��  ���� 
    {
        visited = false;
    }
  
}
public class BackTracking : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells[GridToIndex(x, y)] = new BackTrackingCell(x, y);
            }
        }

        BackTrackingCell start = cells[GridToIndex(0, 0)] as BackTrackingCell;
        start.visited = true;


        //��͹� ȣ��
        MakeRecursive(start.X, start.Y);

        //Recursive BackTracking
        //�������� ����, ����
        //�ش� ����  �湮 ǥ��
        //�����ϰ� �� ���� �� �� ������ ����.(�湮�� ���� ���� ����)
        // �� ���� ��� �湮�ߴٸ� ���� ���� ���ư���.
        // ���������� ������������ ���ư��� �˰��� ����
        //
    }

    void MakeRecursive(int x, int y)
    {
        BackTrackingCell current = cells[GridToIndex(x, y)] as BackTrackingCell;
        Vector2Int[] dirs = { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
        Util.Shuffle(dirs);
        //shuffle
        foreach (Vector2Int dir in dirs)
        {
            Vector2Int pos = new Vector2Int(x + dir.x, y + dir.y);
            if (IsInGrid(pos.x, pos.y))
            {
                BackTrackingCell neighbor = cells[GridToIndex(pos.x, pos.y)] as BackTrackingCell;
                if (!neighbor.visited)
                {
                    neighbor.visited = true;
                    ConnectPath(current, neighbor);

                    MakeRecursive(neighbor.X, neighbor.Y);

                    //���ȣ���� �ڷ� ���ư��� �ڵ尡 ���  �ڿ������� �ڷ� ���ư��� �ȴ�.
                    // �� >  (1, 6) => (1, 7) ���� ���� neighbor.visited == true �� �Ǹ� �ڷ� ���ư��� �ڵ尡 ��� �ٽ� 1, 6���� ���ư��� �ȴ�. 
                }
            }
        }
    }
}
