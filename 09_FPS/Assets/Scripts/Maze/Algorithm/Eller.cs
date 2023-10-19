using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class EllerCell : Cell
{
    public int group;
    public int value;
 
    public EllerCell(int x, int y) : base(x, y)
    {

    }
}
public class Eller : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        for (int y = 0; y < width; y++)
        {
            for(int x = 0; x < height; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new EllerCell(x, y);
                EllerCell cell = cells[GridToIndex(x, y)] as EllerCell;
                cell.value = index;
            }
        }

        for (int y = 0; y < height; y++)
        {
            List<List<EllerCell>> ellerList = new List<List<EllerCell>>();
            for(int x = 0; x < width - 1; x++)
            {
                EllerCell current = cells[GridToIndex(x, y)] as EllerCell;
                EllerCell next = GetNextCell(current);

                float randomChance = 0.5f;
                if (current.value != next.value && UnityEngine.Random.value > randomChance)
                {
                    ConnectPath(current, next);
                    next.value = current.value;
                }
            }
            


        }




        //한줄 만들기
        // 1.1 첫째줄은 가로 길이만큼 셀을 만들고  각각 유니크한 집합에 포함시킨다.
        // 1.2 나머지 줄은 윗쪽 줄에 벽이 없으면(뚫려있으면) 윗쪽줄과 같은 집합을 사용한다.
        // 1.3 윗쪽 줄에 벽이 있으면 고유한(개별) 집합에 포함시킨다.

        //2. 옆칸과 서로 집합이 다를 경우 랜덤하게 벽을 제거하고(연결하고)  왼쪽셀 기준으로 같은 집합으로 만든다.(현재 줄 에서만)
        //2.2 서로 같은 집합일 경우 뚫지 않고 패스

        //3. (아래쪽 벽 제거 )한 집합 당 최소 하나 이상의 구멍이 뚫려야한다.

        //4. 마지막 줄이 될 때 까지 1~3번 반복
        //5. 마지막 줄 처리
        //5.1 줄 생성은 똑같이 처리
        //5.2 합치는 작업은 셋트가 다르면 무조건 합친다.
    }
    EllerCell GetNextCell(EllerCell current)
    {
        return cells[GridToIndex(current.X + 1, current.Y)] as EllerCell;
    }
    List<EllerCell> GetUnvisitedNeighbors(EllerCell current)
    {
        List<EllerCell> neighbors = new List<EllerCell>();

        int[,] dirs = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        for (int i = 0; i < 4; i++)
        {
            int x = current.X + dirs[i, 0];
            int y = current.Y + dirs[i, 1];
            if (IsInGrid(x, y))
            {
                EllerCell neighbor = cells[GridToIndex(x, y)] as EllerCell;
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }
}

