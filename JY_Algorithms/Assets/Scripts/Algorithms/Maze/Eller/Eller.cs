using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

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
            for (int x = 0; x < width; x++)
            {
                if(IsInGrid(x - 1, y))
                {
                    on_Set_DefaultMaterial?.Invoke(GridToIndex(x - 1, y));
                }
                EllerCell current = cells[GridToIndex(x, y)] as EllerCell;
                EllerCell next = cells[GridToIndex(Mathf.Min(x + 1, width - 1), y)] as EllerCell;
                on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));
                on_Set_PathMaterial?.Invoke(GridToIndex(next.X, next.Y));

                MergeCell(current, next, 0.5f);
                row.Add(current);
                row.Add(next);
                await Task.Delay(1000);
            }
            row = row.Distinct().ToList();
            EllerCell[] arr = row.ToArray();
            foreach (EllerCell cell in arr)
            {
                on_Set_DefaultMaterial?.Invoke(GridToIndex(cell.X, cell.Y));
                await Task.Delay(50);
            }
            Util.Shuffle(arr);
            await MergeCellColumn(arr);
        }
    }
    //병합되지 않은 셀 역시 sets에 포함되어야 한다.
    //j가 항상 0이어서 같은 셀을 기준으로 sets에 추가되어있는지 확인하는 문제
    async Task MergeCellColumn(EllerCell[] row)
    {
        EllerCell[] belows = new EllerCell[row.Length];
        for (int i = 0; i < row.Length; i++)
        {
            EllerCell below = cells[GridToIndex(row[i].X, row[i].Y + 1)] as EllerCell;//index out of range
            belows[i] = below;
        }
        foreach(List<EllerCell> set in sets.Values.ToList())
        {
            int i = 0;
            while (i < set.Count)
            {
                EllerCell current = set[i];
                EllerCell belowCell = cells[GridToIndex(set[i].X, set[i].Y + 1)] as EllerCell;
                if (set.Count != 1)//카운트가 남았으면 50%확률로 머지
                {
                    MergeCell(current, belowCell, 0.5f);
                }
                else//카운트가 1이면 (마지막 셀이면) 100%확률로 머지
                {
                    MergeCell(current, belowCell, 1);
                }
                on_Set_DefaultMaterial?.Invoke(GridToIndex(set[Mathf.Max(0, i - 1)].X, set[Mathf.Max(0, i - 1)].Y));// 이전에 셋팅된 머티리얼 되돌리기
                on_Set_DefaultMaterial?.Invoke(GridToIndex(set[Mathf.Max(0, i - 1)].X, set[Mathf.Max(0, i - 1)].Y + 1));
                on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));//현재 진행될 셀의 머티리얼 설정
                on_Set_PathMaterial?.Invoke(GridToIndex(belowCell.X, belowCell.Y));
                await Task.Delay(1000);
                i++;
            }
        }
        
    }
    void MergeCell(EllerCell from, EllerCell to, float successRate)
    {
        if (!sets.ContainsKey(from.group))//리스트가 없을경우 생성
        {
            sets[from.group] = new List<EllerCell>();
        }
        if (UnityEngine.Random.value < successRate)//50%확률로 머지
        {
            to.group = from.group;
            sets[from.group].Add(from);
            sets[from.group].Add(to);

            //sets[from.group].Distinct().ToList();//
            GameManager.Visualizer.ConnectPath(from, to);
        }
        else
        {
            sets[from.group].Add(from);
            if (!sets.ContainsKey(to.group))
            {
                sets[to.group] = new List<EllerCell>();
            }
            sets[to.group].Add(to);
        }
        sets[from.group] = sets[from.group].Distinct().ToList();//중복된 요소 제거하기
        sets[to.group] = sets[to.group].Distinct().ToList();
    }


}

