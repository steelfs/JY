using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Prim_Cell : Cell
{
    public Prim_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Prim : MazeGenerator
{
    public override Cell[] MakeCells(int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new Prim_Cell[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index] = new Prim_Cell(x, y);
            }
        }
        return cells;
    }
    public override async void MakeMaze()
    {
        HashSet<Prim_Cell>inMaze = new HashSet<Prim_Cell>(cells.Length);
        //만약 사이즈가 커져서 랜덤한 인덱스의 요소를 가져오는데 시간이 많이 소요되면
        //frontiers 에도inMaze 처럼 HashSet을 추가로 만들어 줄 수 있다.
        List<Prim_Cell> frontiers = new List<Prim_Cell> ();

        Prim_Cell first = cells[Random.Range(0, cells.Length)] as Prim_Cell;
        inMaze.Add(first);
        on_Set_ConfirmedMaterial?.Invoke(GridToIndex(first.X, first.Y));
        Prim_Cell[] neighbors = GetNeighbors(first);
        foreach (Prim_Cell neighbor in neighbors)
        {
            frontiers.Add(neighbor);
            on_Set_NextMaterial?.Invoke(GridToIndex(neighbor.X, neighbor.Y));
        }
        while (frontiers.Count > 0)
        {
            Prim_Cell chosen = frontiers[Random.Range(0, frontiers.Count)];
            on_Set_ConfirmedMaterial?.Invoke(GridToIndex(chosen.X, chosen.Y));
            neighbors = GetNeighbors(chosen);//상하좌우 셀들중 보드 안쪽 것들 모두 가져오기
            List<Prim_Cell> tempList = new List<Prim_Cell>();// 인접한 셀 중 미로에 추가된 셀이 하나이상일 경우 이 리스트에 저장 후 랜덤으로 고르게 된다.
            foreach (Prim_Cell neighbor in neighbors)
            {
                if (inMaze.Contains(neighbor))//만약 미로에 추가되어있는 셀 이라면 후보리스트에 추가
                {
                    tempList.Add(neighbor);
                }
                else
                {
                    if (!frontiers.Contains(neighbor))//아니라면 후보리스트에 추가
                    {
                        frontiers.Add(neighbor);
                        on_Set_NextMaterial?.Invoke(GridToIndex(neighbor.X, neighbor.Y));
                    }
                }
            }
            await Task.Delay(200);

            Prim_Cell cell = tempList[Random.Range(0, tempList.Count)];
            MergeCell(chosen, cell);//머지
            frontiers.Remove(chosen);//선택된 셀을 frontiers 에서 제거하고
            inMaze.Add(chosen);//선택된 셀을 inMaze 에 추가한다.
        }
        GameManager.Visualizer.InitBoard();


    }
    void MergeCell(Prim_Cell from, Prim_Cell to)
    {
      
        GameManager.Visualizer.ConnectPath(from, to);
        GameManager.Visualizer.AddToConnectOrder(from, to);
    }

}
