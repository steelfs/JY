using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

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
                if(IsInGrid(x - 1, y))
                {
                    on_Set_DefaultMaterial?.Invoke(GridToIndex(x - 1, y));
                }
                EllerCell current = cells[GridToIndex(x, y)] as EllerCell;
                EllerCell next = cells[GridToIndex(x + 1, y)] as EllerCell;
                on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));
                on_Set_PathMaterial?.Invoke(GridToIndex(next.X, next.Y));

                MergeCell(current, next, 0.5f);
                row.Add(current);
                row.Add(next);
                await Task.Delay(2000);
            }
            row = row.Distinct().ToList();
            EllerCell[] arr = row.ToArray();
            foreach (EllerCell cell in arr)
            {
                on_Set_DefaultMaterial?.Invoke(GridToIndex(cell.X, cell.Y));
                await Task.Delay(50);
            }
            Util.Shuffle(arr);
            MergeCellColumn(arr);
        }
    }
    async void MergeCellColumn(EllerCell[] row)
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
            on_Set_PathMaterial?.Invoke(GridToIndex(row[i].X, row[i].Y));
            on_Set_PathMaterial?.Invoke(GridToIndex(belows[i].X, belows[i].Y));
            if (mergeCount > 1)
            {
                MergeCell(row[i], belows[i], 1.0f);//100프로 확률로 머지
            }
            else
            {
                MergeCell(row[i], belows[i], 0.5f);
            }
            await Task.Delay(2000);
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

