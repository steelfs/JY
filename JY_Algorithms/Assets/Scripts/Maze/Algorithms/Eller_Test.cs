using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EllerCell_Test : Cell
{
    public int group { get; set; } = -1;
    public EllerCell_Test(int x, int y) : base( x, y)
    {

    }
}
public class Eller_Test : MazeGenerator_Test
{
    
    Dictionary<int, List<EllerCell_Test>> sets = new Dictionary<int, List<EllerCell_Test>>();
    EllerCell_Test previous_From = null;
    EllerCell_Test previous_To = null;
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new EllerCell_Test[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new EllerCell_Test(x, y);
                EllerCell_Test cell = cells[index] as EllerCell_Test;
                cell.group = index;//고유한 인덱스 부여
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        List<EllerCell_Test> row = new List<EllerCell_Test>();//가로 한 줄
        for (int y = 0; y < height; y++)
        {
            row.Clear();
            for (int x = 0; x < width; x++)
            {
                EllerCell_Test current = cells[GridToIndex(x, y)] as EllerCell_Test;
                EllerCell_Test next = cells[GridToIndex(Mathf.Min(x + 1, width - 1), y)] as EllerCell_Test;
                if (y == height - 1 && current.group != next.group) //마지막줄에 같은 그룹이 아닐경우 100% 머지
                {
                    MergeCell(current, next, 1.1f);
                }
                else if (current.group != next.group)//같은 그룹이 아니면 70% 머지
                {
                    MergeCell(current, next, 0.7f);
                }
                row.Add(current);
                row.Add(next);
                await Task.Delay(100);
            }
            row = row.Distinct().ToList();//중복된 요소 제거
            foreach (EllerCell_Test cell in row)
            {
                on_Set_DefaultMaterial?.Invoke(GridToIndex(cell.X, cell.Y));//머티리얼 초기화
                await Task.Delay(50);
            }
            if (y < height - 1)//마지막 줄이 아닐때만 세로 머지
            {
                await MergeCellColumn(row);
            }
        }
        GameManager.Visualizer_Test.InitBoard();
    }
    async Task MergeCellColumn(List<EllerCell_Test> row)
    {
        HashSet<int> processedGroups = new HashSet<int>();//특정 그룹이 이미 처리되었는지 확인하기 위한 HashSet
        List<EllerCell_Test> set = null;//sets Dictionary에 Value값인 리스트를 담기 위한 리스트
        for (int i = 0; i < row.Count; i++)
        {
            if (processedGroups.Contains(row[i].group))//이미 처리된 리스트라면 스킵
            {
                continue;
            }
            set = sets[row[i].group];//sets에 저장된 리스트 가져오기
            for (int t = 0; t < set.Count; t++)
            {
                if (set[t].Y != row[0].Y)//불러온 리스트에서 Y값이 현제 처리하려는 줄과 같지 않으면 제거
                {
                    set.Remove(set[t--]);
                }
            }
            
            if (set.Count > 1)//그룹에 추가된 갯수가 1개 이상일 때
            {
                int repeat = set.Count;
                for (int j = 0; j < repeat; j++)
                {
                    EllerCell_Test current = set[j];
                    EllerCell_Test below = cells[GridToIndex(set[j].X, set[j].Y + 1)] as EllerCell_Test;
                    if (j == 0)//첫번째 것은 100% 머지
                    {
                        MergeCell(current, below, 1.1f);
                        await Task.Delay(100);
                    }
                    else//나머지 셀은 30%확률로 머지
                    {
                        MergeCell(current, below, 0.3f);
                        await Task.Delay(100);
                    }
                }
            }
            else//리스트에 셀이 하나만 있을 경우 100% 확률로 머지
            {
                EllerCell_Test current = set[0];
                EllerCell_Test below = cells[GridToIndex(set[0].X, set[0].Y + 1)] as EllerCell_Test;
                MergeCell(current, below, 1.1f);

                await Task.Delay(100);
            }
            processedGroups.Add(set[0].group);//이 리스트가 이미 처리되었음을 저장
           
        }
    }

   
    void MergeCell(EllerCell_Test from, EllerCell_Test to, float successRate)
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
            sets[from.group] = new List<EllerCell_Test>();
        }
        if (UnityEngine.Random.value < successRate)
        {
            if (sets.ContainsKey(to.group))
            {
                List<EllerCell_Test> existGroup = sets[to.group];
                foreach(EllerCell_Test cell in existGroup)//머지된 리스트의 요소들을 모두 from의 group으로 설정
                {
                    cell.group = from.group;
                    sets[from.group].Add(cell);
                }
                sets[from.group].Add(from);
            }
            else
            {
                to.group = from.group;
                sets[from.group].Add(from);
                sets[from.group].Add(to);
            }
            GameManager.Visualizer_Test.ConnectPath(from, to);
            GameManager.Visualizer_Test.AddToConnectOrder(from, to);
        }
        else
        {
            sets[from.group].Add(from);
            if (!sets.ContainsKey(to.group))
            {
                sets[to.group] = new List<EllerCell_Test>();
            }
            sets[to.group].Add(to);
            
        }
        sets[from.group] = sets[from.group].Distinct().ToList();//중복된 요소 제거하기
        if (sets.ContainsKey(to.group))
        {
            sets[to.group] = sets[to.group].Distinct().ToList();
        }
    }


}

