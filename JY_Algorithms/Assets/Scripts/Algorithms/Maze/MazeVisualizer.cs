using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;
    public const int CellSize = 5;
    Cell[] cells = null;
    public bool IsExistCells => cells != null;
    int progress = 0;
    public List<(Cell, Cell)> connectOrder = new List<(Cell, Cell)>();

    Vector3 sixPos = new Vector3(22.5f, 0, -26);
    Vector3 sevenPos = new Vector3(25.5f, 0, -30);
    public Cell[] Cells
    {
        get => cells;
        set { cells = value; }
    }
    public virtual void MakeBoard(int x, int y)
    {
   
    }
    public virtual void InitBoard()
    {
        foreach (Cell cell in cells)
        {
            cell.ResetPath();
        }
    }
    public void PlayMaze()
    {
        InvokeRepeating("MoveToNext", 0f, 0.2f);
    }
    public void StopMaze()
    {
        CancelInvoke();
    }
    public void MoveToStartPoint()
    {
        foreach(var (item1, item2) in connectOrder)
        {
            DisconnectPath(item1, item2);
            progress = 0;
        }
    }
    public void MoveToEndPoint()
    {
        foreach (var (item1, item2) in connectOrder)
        {
            ConnectPath(item1, item2);
            progress = cells.Length - 2;
        }
    }

    public void MoveToNext()
    {
        ConnectPath(connectOrder[progress].Item1, connectOrder[progress].Item2);
        progress = Mathf.Min(progress + 1, cells.Length - 2);
    }
    public void MoveToPrev()
    {
        DisconnectPath(connectOrder[progress].Item1, connectOrder[progress].Item2);
        progress = Mathf.Max(progress - 1, 0);
    }
    public void ConnectPath(Cell from, Cell to)
    {
        int diffX = from.X - to.X;
        int diffY = from.Y - to.Y;
        if (diffX < 0)// from =왼쪽 , to = 오른쪽
        {
            from.OpenWall(Direction.East);
            to.OpenWall(Direction.West);
        }
        else if (diffX > 0)// 
        {
            from.OpenWall(Direction.West);
            to.OpenWall(Direction.East);
        }
        else if (diffY > 0)// from = 아랫쪽, to = 윗쪽
        {
            from.OpenWall(Direction.North);
            to.OpenWall(Direction.South);
        }
        else if (diffY < 0)// from = 윗쪽, to = 아랫쪽
        {
            from.OpenWall(Direction.South);
            to.OpenWall(Direction.North);
        }
    }
    public void DisconnectPath(Cell from, Cell to)
    {
        int diffX = from.X - to.X;
        int diffY = from.Y - to.Y;
        if (diffX < 0)// from =왼쪽 , to = 오른쪽
        {
            from.CloseWall(Direction.East);
            to.CloseWall(Direction.West);
        }
        else if (diffX > 0)// 
        {
            from.CloseWall(Direction.West);
            to.CloseWall(Direction.East);
        }
        else if (diffY > 0)// from = 아랫쪽, to = 윗쪽
        {
            from.CloseWall(Direction.North);
            to.CloseWall(Direction.South);
        }
        else if (diffY < 0)// from = 윗쪽, to = 아랫쪽
        {
            from.CloseWall(Direction.South);
            to.CloseWall(Direction.North);
        }
    }
    public void RenderBoard(int width, int height, Cell[] cells)// delegate를 연결하기 위해 cell의 배열도 받는다.
    {
        if(width > 6)
        {
            transform.position = sevenPos;
        }
        else if (width > 5)
        {
            transform.position = sixPos;
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cell = GameManager.Pools.GetObject(PoolObjectType.Cell, transform);
                cell.name = $"Cell_{x}_{y}";
                cell.transform.localRotation = Quaternion.Euler(0, 180, 0);
                cell.transform.localPosition = Vector3.zero;
                cell.transform.Translate(x * CellSize, 0, -y * CellSize, Space.Self);// 위에서 로컬로테이션을 180도 돌렸기 때문에 역방향으로 계산

                int index = (y * width) + x;
                CellVisualizer cellVisualizer = cell.GetComponent<CellVisualizer>();
                Cell currentCell = cells[index];
                currentCell.on_RefreshWall = cellVisualizer.RefreshWalls;//UI 업데이트 연결
            }
        }
    }
    public void DestroyBoard()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Cell cell = cells[i];
            cell.ResetPath();
            cell.on_RefreshWall = null;
            cell.Path = 0;
            //x, y 는 생성시 초기화되기 때문에 생략
            cell = null;
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Cells = null;//셀의 배열을 null 로 만들기
        connectOrder.Clear();
        progress = 0;
    }
    public void AddToConnectOrder(Cell from, Cell to)
    {
        connectOrder.Add((from, to));
    }
    public virtual void StartConnect()
    {

    }
    public virtual void StopConnect()
    {

    }
    //    //void InitBoard
    //    void Clear Board
    //    void ConnectPath
    //void DisconnectPath
    //IEnumurator StartConnect
    //void StopConnect
}
