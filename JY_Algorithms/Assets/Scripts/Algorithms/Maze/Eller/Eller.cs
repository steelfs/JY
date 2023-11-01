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
    EllerCell previous_From = null;
    EllerCell previous_To = null;
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
                EllerCell current = cells[GridToIndex(x, y)] as EllerCell;
                EllerCell next = cells[GridToIndex(Mathf.Min(x + 1, width - 1), y)] as EllerCell;
                if (y == height - 1)
                {
                    MergeCell(current, next, 1.1f);
                }
                else if (current.group != next.group)
                {
                    MergeCell(current, next, 0.7f);
                }
                row.Add(current);
                row.Add(next);
                await Task.Delay(500);
            }
            row = row.Distinct().ToList();
            foreach (EllerCell cell in row)
            {
                on_Set_DefaultMaterial?.Invoke(GridToIndex(cell.X, cell.Y));
                await Task.Delay(50);
            }
            if (y < height - 1)
            {
                await MergeCellColumn(row);
            }
        }
    }
    async Task MergeCellColumn(List<EllerCell> row)
    {
       
        for (int i = 0; i < row.Count; i++)
        {
            List<EllerCell> set = null;
            if (sets.ContainsKey(row[i].group))
            {
                set = sets[row[i].group];
            }
            else
            {
                continue;
            }
            if (set.Count > 1)//그룹에 추가된 갯수가 1개 이상일 때 최소 하나는 무조건 머지
            {
                int repeat = set.Count;
                for (int j = 0; j < repeat; j++)
                {
                    EllerCell current = set[j];
                    EllerCell below = cells[GridToIndex(set[j].X, set[j].Y + 1)] as EllerCell;
                    if (j == 0)
                    {
                        MergeCell(current, below, 1.1f);
                        await Task.Delay(500);
                    }
                    else
                    {
                        MergeCell(current, below, 0.3f);
                        await Task.Delay(500);
                    }
                }
            }
            else//100% 확률로 머지
            {
                EllerCell current = set[0];
                EllerCell below = cells[GridToIndex(set[0].X, set[0].Y + 1)] as EllerCell;
                MergeCell(current, below, 1.1f);

                await Task.Delay(500);
            }
            sets.Remove(set[0].group);//같은 그룹을 또다시 도는 것을 방지 하기 위해 추가했는데
            //이 코드로 인해 가로방향 으로 머지될 때 연결되어있는 셀들의 그룹을 모두 같은 그룹으로 병합해야하는데 
            //리스트가 이미 지워진 문제 발생
        }
    }

   
    void MergeCell(EllerCell from, EllerCell to, float successRate)
    {
        if (previous_From != null)
        {
            on_Set_DefaultMaterial?.Invoke(GridToIndex(previous_From.X, previous_From.Y));
            on_Set_DefaultMaterial?.Invoke(GridToIndex(previous_To.X, previous_To.Y));
        }
        on_Set_NextMaterial?.Invoke(GridToIndex(from.X, from.Y));
        on_Set_NextMaterial?.Invoke(GridToIndex(to.X, to.Y));
        previous_From = from;
        previous_To = to;
        if (!sets.ContainsKey(from.group))//리스트가 없을경우 생성
        {
            sets[from.group] = new List<EllerCell>();
        }
        if (UnityEngine.Random.value < successRate)
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

