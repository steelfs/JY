using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Sprites;
using UnityEngine;

public class WilsonCell : Cell
{
    public WilsonCell Next { get; set; } = null;
    public WilsonCell Prev { get; set; } = null;
    public WilsonCell(int x, int y) : base(x, y)
    {

    }
}
public class Wilson : MazeGenerator
{
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new WilsonCell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new WilsonCell(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        
        Dictionary<WilsonCell, WilsonCell> confirmedSet = new Dictionary<WilsonCell, WilsonCell> ();//탐색속도를 높이기 위해 추가한 것인데 이곳에서 굳이 Dictionary를 쓰는 것은 과했다. 
        List<WilsonCell> confirmed = new List<WilsonCell> ();
        List<WilsonCell> notConfirmed = new List<WilsonCell>(cells as WilsonCell[]);
        WilsonCell[] arr = notConfirmed.ToArray();
        Util.Shuffle(arr);
        notConfirmed = arr.ToList();
        Stack<WilsonCell> path = new Stack<WilsonCell> ();//움직이는 경로를 저장할 스택
        Dictionary<WilsonCell, WilsonCell> pathSet = new Dictionary<WilsonCell, WilsonCell> ();//셀의 갯수가 몇개 없기때문에 검색속도는 별 차이 없을 것. 이것 역시 너무 과했다.


        WilsonCell first = cells[UnityEngine.Random.Range(0, cells.Length)] as WilsonCell;//처음 도착지점 설정
        confirmedSet.Add(first, first);
        confirmed.Add(first);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(first.X, first.Y));//머티리얼 변경 신호
        notConfirmed.Remove(first);



        WilsonCell start = notConfirmed[0];// 위에서 Shuffle로 섞어 놓았기 때문에 하나씩 뽑아 써도 랜덤한 셀을 가져오게 된다.
        path.Push(start);//경로 스텍에 추가
        pathSet.Add(start, start);

        while(notConfirmed.Count > 0)
        {
            WilsonCell current = path.Peek();//경로 중 가장 최근에 추가된것은 기준으로
            WilsonCell[] neighbors = GetNeighbors(current)as WilsonCell[];// 상하좌우 셀을 가져온다.(이전 셀과 보드 바깥쪽은 제외)
            WilsonCell next = null;//가져온 셀들 중 랜덤하게 하나를 다음셀로 설정
            while(next == null)
            {
                next = neighbors[UnityEngine.Random.Range(0, neighbors.Length)];
                if (next == current.Prev)
                {
                    next = null;
                }
            }


            on_Set_PathMaterial?.Invoke(GridToIndex(current.X, current.Y));
            on_Set_NextMaterial?.Invoke(GridToIndex(next.X, next.Y));//머티리얼 변경
            await Task.Delay(100); //0.1초 딜레이
            if (pathSet.ContainsKey(next))//이미 지나온 경로일 경우
            {
                while (path.Peek() != next) //경로중 가장 앞쪽 셀이  다음셀로 지정된 셀과 다르면 계속해서 Pop으로 스택에서 꺼낸다.
                {
                    WilsonCell picked = path.Pop();

                    int pickedIndex = GridToIndex(picked.X, picked.Y);
                    on_Set_DefaultMaterial?.Invoke(pickedIndex);//경로에서 해제됐기 때문에 경로에 추가된 머티리얼로 설정됐던 것을 다시 Default머티리얼로 바꾼다.
                    await Task.Delay(100);
                    pathSet.Remove(picked);
                }
            }
            else if (confirmedSet.ContainsKey(next))//도착지에 도착했을 경우
            {
                on_Set_ConfirmedMaterial?.Invoke(GridToIndex(next.X, next.Y));
                foreach (WilsonCell picked in path)//path에 추가되어있던 셀들을 확정 리스트에 추가시키면서 머티얼도 바꿔준다.
                {
                    confirmedSet[picked] = picked;
                    confirmed.Add(picked);
                    on_Set_ConfirmedMaterial?.Invoke(GridToIndex(picked.X, picked.Y));//path에 
                    await Task.Delay(100);
                    notConfirmed.Remove(picked);
                    pathSet.Remove(picked);

                    //GameManager.Visualizer.AddToConnectOrder(picked, picked.Next);
                    //stack이라 순서가 거꾸로 들어가지만 이거대로 나쁘지 않을것 같아서 그냥 진행했다.
                    //confirmed가 딕셔너리가 아니라 그냥 리스트였으면 모두 끝나고 한번에 진행하면 될 일
                }
                path.Clear();
                current.Next = next;
                next.Prev = current;
                if (notConfirmed.Count > 0)
                {
                    WilsonCell newStartPoint = notConfirmed[0];
                    path.Push(newStartPoint);
                    pathSet[newStartPoint] = newStartPoint;
                }
            }
            else
            {
                current.Next = next;
                next.Prev = current;
                path.Push(next);
                pathSet[next] = next;
            }
        }
        int i = 0;
        while(i < cells.Length)
        {
            on_Set_DefaultMaterial?.Invoke(i);
            await Task.Delay(30);
            i++;
        }
        for ( i = 0; i < confirmed.Count ; i++)
        {
            GameManager.Visualizer.AddToConnectOrder(confirmed[i], confirmed[i].Next);
        }
    }
  
    //protected override Cell[] GetNeighbors(Cell current)
    //{
    //    int[,] dir = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
    //    WilsonCell[] dirs = null;
    //    List<WilsonCell> neighbors = new List<WilsonCell>();
    //    WilsonCell current_ = current as WilsonCell;
    //    for (int i = 0; i < 4; i++)
    //    {
    //        int X = current.X + dir[i, 0];
    //        int Y = current.Y + dir[i, 1];
    //        if (IsInGrid(X, Y))
    //        {
    //            WilsonCell newCell = cells[GridToIndex(X, Y)] as WilsonCell;

    //            if (current_.Prev == newCell)
    //                continue;

    //            neighbors.Add(newCell);
    //        }
    //    }
    //    dirs = neighbors.ToArray();
    //    Util.Shuffle(dirs);
    //    return dirs;
    //}


}

