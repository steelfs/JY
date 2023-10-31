using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EllerCell : Cell
{
    public EllerCell prev { get; set; } = null;
    public EllerCell next { get; set; } = null;
    public int group { get; set; } = -1;
    public EllerCell(int x, int y) : base( x, y)
    {

    }
}
public class Eller : MazeGenerator
{
    Dictionary<int, List<EllerCell>> sets = new Dictionary<int, List<EllerCell>>();
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new EllerCell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new EllerCell(x, y);
                EllerCell cell = cells[index] as EllerCell;
                cell.group = index;
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        List<EllerCell> row = new List<EllerCell>();
        for (int y = 0; y < height; y++)
        {
            row.Clear();
            for (int x = 0; x < width - 1; x++)
            {
                EllerCell current = cells[GridToIndex(x, y)] as EllerCell;
                EllerCell next = cells[GridToIndex(x + 1, y)] as EllerCell;
                MergeCell(current, next, 0.5f);
                row.Add(current);
                row.Add(next);
                await Task.Delay(500);
            }
            row = row.Distinct().ToList();
            EllerCell[] arr = row.ToArray();
            Util.Shuffle(arr);
            MergeCellColumn(arr);
        }
    }
    void MergeCellColumn(EllerCell[] row)
    {
        EllerCell[] belows = new EllerCell[row.Length];
        for (int i = 0; i < row.Length; i++)
        {
            EllerCell below = cells[GridToIndex(row[i].X, row[i].Y + 1)] as EllerCell;
            belows[i] = below;
        }
        for (int i = 0; i < row.Length; i++)
        {
            int mergeCount = 0;
            for (int j = 0; j < belows.Length; j++)
            {
                if (sets.ContainsKey(belows[j].group))// 이미 하나 이상 윗쪽 그룹과 합쳐진 경우
                {
                    mergeCount++;
                }
                else
                {
                    //아직 합쳐지지 않은 경우
                    continue;
                }
            }
            if (mergeCount > 0)
            {

            }
        }
        // 호출하기 전 같은 행의 셀들을 모아서 한번 섞은 다음 차례로 이 함수를 호출한다.
        //만약 같은 행에 있는 셀들 줄 sets에 포함되어있는게 있다면 확률적으로 머지하고 없다면 100 프로확률로 머지한다.
    }
    void MergeCell(EllerCell from, EllerCell to, float successRate)
    {
        if (from.group != to.group)
        {
            if (UnityEngine.Random.value < successRate)
            {
                to.group = from.group;
                if (!sets.ContainsKey(from.group))
                {
                    sets[from.group] = new List<EllerCell>();
                    sets[from.group].Add(from);
                    sets[from.group].Add(to);
                }
                else 
                {
                    sets[from.group].Add(to);
                }
             
                //sets[from.group].Distinct().ToList();//중복된 아이템 제거하기
                GameManager.Visualizer.ConnectPath(from, to);
            }
        }
       
    }
}

