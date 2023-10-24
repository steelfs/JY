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
    public WilsonCell prev = null;
    public WilsonCell(int x, int y) : base(x, y)
    {
        serial = NotSet;
    }
}
public class Wilson : MazeGenerator
{
    readonly int[,] dir = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };// 전역변수로 빼둬도 괜찮을 것같다.

    protected override void OnSpecificAlgorithmExcute()
    {
        //생성 후 시리얼 부여 
        Dictionary<int, WilsonCell> confirmedList = new Dictionary<int, WilsonCell>();
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
        confirmedList[arrived.serial] = arrived;// 확정된 리스트에 추가
           
        while(confirmedList.Count < cells.Length)
        {
            WilsonCell startCell = GetNewStartPoint(confirmedList);
            MoveToNext(startCell, confirmedList);
        }

        foreach(WilsonCell cell in confirmedList.Values)
        {
            if (cell.next != null)
            {
                ConnectPath(cell, cell.next);
            }
        }
    }
  
    private WilsonCell GetNewStartPoint(Dictionary<int, WilsonCell> Added)//미로에 추가되지 않은 곳 중 랜덤한 셀을 출발지로 리턴
    {
        List<WilsonCell> unVisited = new List<WilsonCell>();
        for (int i = 0; i < cells.Length; i++)
        {
            WilsonCell cell = cells[i] as WilsonCell;
            if (!Added.ContainsKey(cell.serial))//ContainsValue or key 함수는 O(n) 시간복잡도
            {
                unVisited.Add(cell);
            }
        }
        return unVisited[UnityEngine.Random.Range(0, unVisited.Count)];
    }
    void MoveToNext(WilsonCell current, Dictionary<int, WilsonCell> confirmedList)
    {
        Stack<WilsonCell> path = new Stack<WilsonCell>();
        WilsonCell nextCell = null;
        path.Push(current);//처음건 무조건 추가
        while (true)
        {
            nextCell = GetNextCell(current);//current를 기준으로 네 방향중 하나를 가져옴
            if (!IsAddedAtConfirmedList(nextCell, confirmedList))//가져온 셀이  컨펌리스트에 추가되어있지 않으면(도착지가 아니면)
            {
                if (!path.Contains(nextCell))//이미 지나온 길이 아니면
                {
                    current.next = nextCell;
                    nextCell.prev = current;
                    path.Push(nextCell);
                    current = nextCell;
                }
                else//이미 지나온 길이면
                {
                    WilsonCell prevCell = current;
                    while(nextCell != prevCell)// 다음칸으로 이동하려던 셀이 스텍의 이전 셀과 같아질 떄까지 계속 Pop
                    {
                        prevCell = path.Pop();//peek으로 빼고 Pop하는것도 고려해볼만 한것같다/
                        current = prevCell;
                    }
                    path.Push(prevCell);// while 루프를 빠져나왔을 때 다시 넣어줘야한다.
                }
            }
            else// 가져온 셀이 확정리스트에 추가되어있을 경우 path를 확정 리스트에 추가하고 루프를 종료한다.
            {
                current.next = nextCell;
                nextCell.prev = current;
                foreach(WilsonCell cell in path)
                {
                    confirmedList.Add(cell.serial, cell);
                    //기존의 리스트에 추가되어있던것들 중 마지막것의 prev, next가 설정되지 않는 문제가 있다.
                }
                break; //while 루프 종료
                //컨펌에 추가하고 종료
            }
        }
        
    }
    WilsonCell GetNextCell(WilsonCell current)
    {
        List<WilsonCell> neighbors = new List<WilsonCell>();

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
        if (current.prev != null)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                if(neighbors[i] == current.prev)
                {
                    neighbors.RemoveAt(i);
                }
            }
        }
        return neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
    }
    bool IsAddedAtConfirmedList(WilsonCell cell, Dictionary<int, WilsonCell> confirmedList)
    {
        bool result = false;
        if (confirmedList.ContainsKey(cell.serial))
        {
            result = true;
        }
        return result;
    }
}

 
    

