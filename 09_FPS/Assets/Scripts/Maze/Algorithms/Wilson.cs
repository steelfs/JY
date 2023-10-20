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
        Dictionary<int, WilsonCell> added = new Dictionary<int, WilsonCell>();
        Dictionary<int, WilsonCell> notAdded = new Dictionary<int, WilsonCell>();
        int serial = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                WilsonCell cell = new WilsonCell(x,y);
                cells[GridToIndex(x, y)] = cell;
                cell.serial = serial++;

                notAdded[cell.serial] = cell;
            }
        }
        WilsonCell arrived = cells[UnityEngine.Random.Range(0, cells.Length)] as WilsonCell;
        added[arrived.serial] = arrived;// 추가된 리스트에 추가
           



        WilsonCell start = GetRandomCell(notAdded);
        Dictionary<int, WilsonCell> path = new Dictionary<int, WilsonCell>();

        Move(start, path, added);

        MakePath(path);
        //미로에 추가된 부분과, 추가되지않은 그룹을 두개로 만든다
        //셀을 모두 만든 후 추가되지 않은 그룹에 추가한다.
        // 그 중 두개를 골라서 하나는 추가된 그룹에 추가하고 하나는 출발지점으로 설정한다.
        // 사방향 중 랜덤하게, 방문하지 않은 곳으로 움직이면서 딕셔너리에 추가한다.
        //목적지에 도착시 경로를 미로에 추가시킨다.
    }

    void MakePath(Dictionary<int, WilsonCell> path)
    {
        foreach(WilsonCell cell in path.Values)
        {

        }
        for (int i = 0; i < path.Count - 2; i++)
        {
            WilsonCell current = path[i];
            WilsonCell next = path[i + 1];
            ConnectPath(current, next);

        }
    }

    void Move(WilsonCell current, Dictionary<int, WilsonCell> path, Dictionary<int, WilsonCell> added )
    {
        List<WilsonCell> list = new List<WilsonCell>();
            
        WilsonCell next = current;
        while (!added.ContainsKey(current.serial))//도착지점이 아니면
        {
            next = GetNeighbor(next);//랜덤으로 하나 뽑아와서
            if (path.ContainsKey(next.serial))// 이미 거쳐온 곳이면 스킵
            {
                continue;
            }
            path.Add(next.serial, next);

            current.next = next;
            current = next;
             
            //path에 포함되어있지 않아야하고, 
        }


    }
    private WilsonCell GetRandomCell(Dictionary<int, WilsonCell> notAdded_)//미로에 추가되지 않은 곳 중 랜덤한 셀을 출발지로 리턴
    {
        WilsonCell result = null;
        List<WilsonCell> unVisited = new List<WilsonCell>();
        foreach (WilsonCell cell in notAdded_.Values)
        {
            unVisited.Add(cell);
        }
        WilsonCell[] arr = unVisited.ToArray();
        Util.Shuffle(arr);
        result = arr[UnityEngine.Random.Range(0, arr.Length)];

        return result;
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
        WilsonCell[] arr = neighbors.ToArray();

        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }
}

 
    

