using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


public class WilsonCell : Cell
{
    public int serial = 0;
    const int NotSet = -1;
    public WilsonCell next = null;
    public WilsonCell(int x, int y) : base(x, y)
    {
        serial = NotSet;
    }
}
public class Wilson : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        //생성 후 시리얼 부여 
        Dictionary<int, WilsonCell> confirmed = new Dictionary<int, WilsonCell>();
        int serial = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                WilsonCell cell = new WilsonCell(x,y);
                cells[GridToIndex(x, y)] = cell;
                cell.serial = serial++;
            }
        }
        WilsonCell arrived = cells[UnityEngine.Random.Range(0, cells.Length)] as WilsonCell;
        confirmed[arrived.serial] = arrived;// 추가된 리스트에 추가
           
        //MoveToAdded 제작 필요
        WilsonCell start = GetNewStartPoint(confirmed);


        Dictionary<int, WilsonCell> path = new Dictionary<int, WilsonCell>();

        Move(start, path, confirmed);

     
    }

   

    void Move(WilsonCell current, Dictionary<int, WilsonCell> path, Dictionary<int, WilsonCell> added )
    {
        WilsonCell next = null;
        path[current.serial] = current; //처음은 그냥 추가
        while (!added.ContainsKey(current.serial))//도착지점이면 종료
        {
            next = GetNeighbor(current);//랜덤으로 하나 뽑아와서
            if (path.ContainsKey(next.serial))// 이미 거쳐온 곳이면 스킵
            {
                continue;
            }
            path[next.serial] = next;//경로에 추가

            current.next = next;
            current = next;
             
            //current를 저장후 다음 셀을 next기준으로 정하면 한번에 두칸을 움직이는 형상이 나타난다.
            //path에 포함되어있지 않아야하고, 
        }


    }
    private WilsonCell GetNewStartPoint(Dictionary<int, WilsonCell> Added)//미로에 추가되지 않은 곳 중 랜덤한 셀을 출발지로 리턴
    {
        List<WilsonCell> unVisited = new List<WilsonCell>();
        for (int i = 0; i < cells.Length; i++)
        {
            WilsonCell cell = cells[i] as WilsonCell;
            if (!Added.ContainsValue(cell))
            {
                unVisited.Add(cell);
            }
        }
        return unVisited[UnityEngine.Random.Range(0, unVisited.Count)];
    }


    WilsonCell GetNeighbor(WilsonCell current)
    {
        List<WilsonCell> neighbors = new List<WilsonCell>();
        int[,] dir = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

        for (int i = 0; i < 4; i++)
        {
            int x = current.X + dir[i, 0];
            int y = current.Y + dir[i, 1];
            if (IsInGrid(x, y))
            {
                WilsonCell cell = cells[GridToIndex(x, y)] as WilsonCell;
                neighbors.Add(cell);
            }
        }

        return neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
    }
}

 
    

