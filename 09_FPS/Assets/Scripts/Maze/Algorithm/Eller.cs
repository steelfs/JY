using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EllerCell : Cell
{
    public int group;
 
    public EllerCell(int x, int y) : base(x, y)
    {

    }
}
public class Eller : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        Dictionary<int, List<EllerCell>> dic = new Dictionary<int, List<EllerCell>>();
        int key = 0;
        for (int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)//가로 한줄 찍기
            {
                EllerCell cell = new EllerCell(x, y);
                cell.group = key++;
                int index = GridToIndex(x, y);
                cells[index] = cell;

                dic[cell.group] = new List<EllerCell> { cell };
                if (IsInGrid(x, y - 1))
                {
                    EllerCell aboveCell = cells[GridToIndex(x, y - 1)] as EllerCell;
                    if (aboveCell != null)
                    {
                        if (aboveCell.IsPath(Direction.South))
                        {
                            ConnectPath(cell, aboveCell);
                            cell.group = aboveCell.group;
                            dic[aboveCell.group].Add(cell);
                        }
                    }
                }
         
            }

            for (int x  = 0; x < width - 1; x++)//가로 뚫기
            {
                EllerCell currentCell = cells[GridToIndex(x, y)] as EllerCell;
                EllerCell nextCell = cells[GridToIndex(x + 1, y)] as EllerCell;

                if (currentCell.group != nextCell.group)
                {
                    float random = 0.5f;
                    if (UnityEngine.Random.value > random)
                    {
                        ConnectPath(currentCell, nextCell);

                        List<EllerCell> nextGroup = dic[nextCell.group];
                        dic.Remove(nextCell.group);

                        foreach (var nextSet in nextGroup)
                        {
                            nextSet.group = currentCell.group;
                            dic[currentCell.group].Add(nextSet);
                            x++;
                        }


                    }
                }
            }
            if (y < height - 1)
            {
                foreach (var set in dic.Values)
                {
                    EllerCell cell = set[UnityEngine.Random.Range(0, set.Count)];
                    cell.MakePath(Direction.South);
                    //ConnectPath를 해야하나 아랫쪽셀이 아직 만들어지지 않았다.
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

