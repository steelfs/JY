using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;


public enum MazeType
{
    None,
    BackTracking,
    Wilson,
    Eller,
    kruskal,
    Prim,
    Division
}
public class MazeVisualizer_Test : MonoBehaviour
{
    BackTracking_Test backTracking;
    Wilson_Test wilson;
    Eller_Test eller;
    Kruskal_Test kruskal;
    Prim_Test prim;
    Division_Test division;
    CellVisualizer_Test[] cellVisualizers;

    MazeType mazeType;
    public MazeType MazeType
    {
        get { return mazeType; }
        set { mazeType = value; }
    }

    public GameObject cellPrefab;
    public const int CellSize = 5;
    Cell[] cells = null;
    int width;
    int height;
    public bool IsExistCells => cells != null;
    int maxProgress => connectOrder.Count - 1;
    int progress = 0;

    int Progress
    {
        get => progress;
        set
        {
            progress = value;
            if (connectOrder.Count > 0)// 미로가 생성됐을 때만
            {
                if (progress > connectOrder.Count - 1)
                {
                    StopMaze();
                    progress = maxProgress;
                }
                else if (progress < 0)
                {
                    progress = 0;
                }
            }
        }
    }
    public List<(Cell, Cell)> connectOrder = new List<(Cell, Cell)>();

    Vector3 six_By_Six = new Vector3(22.5f, 0, -26);
    Vector3 seven_By_Seven = new Vector3(25.5f, 0, -30);
    Vector3 eight_By_Eight = new Vector3(27, 0, -32);

    /// <summary>
    /// MakeBoard 함수에서 현재 설정된 MazeType에 맞게 Cells의 배열을 만든다.
    /// 그 후 설정된 MazeType에 맞는 미로를 MazeGenerator.MakeMaze 함수를 이용해 만든다.
    /// </summary>
    public Cell[] Cells
    {
        get => cells;
        set { cells = value; }
    }

    private void Start()
    {
        seconds = new WaitForSeconds(0.02f);
    }
    public virtual void MakeBoard(int x, int y)
    {
        width = x;
        height = y;
        switch (mazeType)
        {
            case MazeType.None:
                break;
            case MazeType.BackTracking:
                backTracking = new BackTracking_Test();
                backTracking.on_Set_PathMaterial = On_Path_Material;
                backTracking.on_Set_NextMaterial = On_SetNext_Material;
                backTracking.on_Set_ConfirmedMaterial = On_SetConfirmed_Material;
                backTracking.on_Set_DefaultMaterial = On_SetDefault_Material;
                Cells = backTracking.MakeCells(x, y);
                RenderBoard(x, y, Cells);
                break;
            case MazeType.Wilson:
                wilson = new Wilson_Test();
                wilson.on_Set_PathMaterial = On_Path_Material;
                wilson.on_Set_NextMaterial = On_SetNext_Material;
                wilson.on_Set_ConfirmedMaterial = On_SetConfirmed_Material;
                wilson.on_Set_DefaultMaterial = On_SetDefault_Material;
                Cells = wilson.MakeCells(x, y);
                RenderBoard(x, y, Cells);
                break;
            case MazeType.Eller:
                eller = new Eller_Test();
                eller.on_Set_PathMaterial = On_Path_Material;
                eller.on_Set_NextMaterial = On_SetNext_Material;
                eller.on_Set_ConfirmedMaterial = On_SetConfirmed_Material;
                eller.on_Set_DefaultMaterial = On_SetDefault_Material;
                Cells = eller.MakeCells(x, y); 
                RenderBoard (x, y, Cells);
                break;
            case MazeType.kruskal:
                kruskal = new Kruskal_Test();
                kruskal.on_Set_PathMaterial = On_Path_Material;
                kruskal.on_Set_NextMaterial = On_SetNext_Material;
                kruskal.on_Set_ConfirmedMaterial = On_SetConfirmed_Material;
                kruskal.on_Set_DefaultMaterial = On_SetDefault_Material;
                Cells = kruskal.MakeCells(x, y);
                RenderBoard(x, y, Cells);
                break;
            case MazeType.Prim:
                prim = new Prim_Test();
                prim.on_Set_PathMaterial = On_Path_Material;
                prim.on_Set_NextMaterial = On_SetNext_Material;
                prim.on_Set_ConfirmedMaterial = On_SetConfirmed_Material;
                prim.on_Set_DefaultMaterial = On_SetDefault_Material;
                Cells = prim.MakeCells(x, y);
                RenderBoard(x, y, Cells);
                break;
            case MazeType.Division:
                division = new Division_Test();
                division.on_Set_PathMaterial = On_Path_Material;
                division.on_Set_NextMaterial = On_SetNext_Material;
                division.on_Set_ConfirmedMaterial = On_SetConfirmed_Material;
                division.on_Set_DefaultMaterial = On_SetDefault_Material;
                Cells = division.MakeCells(x, y);
                RenderBoard(x, y, Cells);
                InitBoard_Division();
                break;
            default: 
                break;
        }
    }
  
    public void MakeMaze()
    {
        switch (MazeType)
        {
            case MazeType.None:
                break;
            case MazeType.BackTracking:
                backTracking.MakeMaze();
                break;
            case MazeType.Wilson:
                wilson.MakeMaze();
                break;
            case MazeType.Eller:
                eller.MakeMaze();
                break;
            case MazeType.kruskal:
                kruskal.MakeMaze();
                break;
            case MazeType.Prim:
                prim.MakeMaze();
                break;
            case MazeType.Division:
                division.MakeMaze();
                break;
            default:
                break;
        }
    }
    public virtual async void InitBoard()
    {
        foreach (Cell cell in cells)
        {
            cell.ResetPath();
            On_SetConfirmed_Material((cell.Y * width) + cell.X);
            await Task.Delay(20);
        }
        foreach (Cell cell in cells)
        {
            On_SetDefault_Material((cell.Y * width) + cell.X);
            await Task.Delay(20);
        }
        //StartCoroutine(InitCoroutine());
    }
    WaitForSeconds seconds;
    //IEnumerator InitCoroutine()
    //{
    //    foreach (Cell cell in cells)
    //    {
    //        cell.ResetPath();
    //        On_SetConfirmed_Material((cell.Y * width) + cell.X);
    //        yield return seconds;
    //    }
    //    foreach (Cell cell in cells)
    //    {
    //        On_SetDefault_Material((cell.Y * width) + cell.X);
    //        yield return seconds;
    //    }
    //}
    /// <summary>
    /// 셀프리팹을 연결하는 함수를 일정 시간마다 계속 진행하는 함수
    /// </summary>
    public void PlayMaze()
    {
        InvokeRepeating("MoveToNext", 0f, 0.1f);
    }

    /// <summary>
    /// 미로의 재생을 일시정지 하는 함수 
    /// </summary>
    public void StopMaze()
    {
        CancelInvoke();
    }

    /// <summary>
    /// 미로의 처음지점으로 한번에 이동하는 함수 
    /// </summary>
    public void MoveToStartPoint()
    {
        foreach(var (item1, item2) in connectOrder)
        {
            DisconnectPath(item1, item2);
            Progress = 0;
        }
    }
    /// <summary>
    /// 미로의 맨 끝 지점으로 한번에 이동하는 함수
    /// </summary>
    public void MoveToEndPoint()
    {
        foreach (var (item1, item2) in connectOrder)
        {
            ConnectPath(item1, item2);
            Progress = cells.Length - 2;
        }
    }

    /// <summary>
    /// 미로가 모두 만들어진 후 결과로 저장된 리스트에서 순서대로 연결하는 함수
    /// </summary>
    public void MoveToNext()
    {
        ConnectPath(connectOrder[Progress].Item1, connectOrder[Progress].Item2);
        Progress++;
    }
 
    public void MoveToPrev()
    {
        DisconnectPath(connectOrder[Progress].Item1, connectOrder[Progress].Item2);
        Progress--;
    }

    /// <summary>
    /// 셀 두개를 받아서 연결(벽 비활성화)하는 함수
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void ConnectPath(Cell from, Cell to)
    {
        if (to == null || from == null)
        {
            return;
        }
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

    /// <summary>
    /// 셀 두 개를 받아서 연결을 끊는(벽 활성화) 함수
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void DisconnectPath(Cell from, Cell to)
    {
        if (to == null || from == null)
        {
            return;
        }
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

    /// <summary>
    /// 알고리즘으로 미로를 모두 만든 후 셀 프리팹을 만들어진 미로의 위치에 맞게 
    /// 생성하는 함수
    /// </summary>
    /// <param name="width">보드의 X</param>
    /// <param name="height">보드의 Y</param>
    /// <param name="cells">보드를 구성하는 cell 의 배열</param>
    public void RenderBoard(int width, int height, Cell[] cells)// delegate를 연결하기 위해 cell의 배열도 받는다.
    {
        cellVisualizers = new CellVisualizer_Test[cells.Length];
        if (width > 7)
        {
            transform.position = eight_By_Eight;
        }
        else if(width > 6)
        {
            transform.position = seven_By_Seven;
        }
        else if (width > 5)
        {
            transform.position = six_By_Six;
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
                CellVisualizer_Test cellVisualizer = cell.GetComponent<CellVisualizer_Test>();
                cellVisualizers[index] = cellVisualizer;
                Cell currentCell = cells[index];
                currentCell.on_RefreshWall = cellVisualizer.RefreshWalls;//UI 업데이트 연결
            }
        }
    }


    const byte N = 1, E = 2, S = 4, W = 8;
    async void InitBoard_Division()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                cells[index].Path = 15;
                if (index < width)
                {
                    if (index == 0)
                    {
                        cells[index].Path &= unchecked((byte)~N);
                        cells[index].Path &= unchecked((byte)~W);
                    }
                    else if (index == width - 1)
                    {
                        cells[index].Path &= unchecked((byte)~N);
                        cells[index].Path &= unchecked((byte)~E);
                    }
                    else
                    {
                        cells[index].Path &= unchecked((byte)~N);
                    }
                }
                else if (index % width == 0)
                {
                    if (!(y == height - 1))
                    {
                        cells[index].Path &= unchecked((byte)~W);
                    }
                    else
                    {
                        cells[index].Path &= unchecked((byte)~W);
                        cells[index].Path &= unchecked((byte)~S);
                    }
                }
                else if (index % width == width - 1)
                {
                    if (!(index == cells.Length - 1))
                    {
                        cells[index].Path &= unchecked((byte)~E);
                    }
                    else
                    {
                        cells[index].Path &= unchecked((byte)~S);
                        cells[index].Path &= unchecked((byte)~E);
                    }
                }
                else if (index > (width * (height - 1)))
                {
                    cells[index].Path &= unchecked((byte)~S);
                }

                cellVisualizers[index].RefreshWalls(cells[index].Path);
                await Task.Delay(100);
            }
            
        }
    }
    int GridToIndex(int x, int y)
    {
        return (y * width) + x;
    }
    void On_Path_Material(int index)
    {
        CellVisualizer_Test cellVisualizer = cellVisualizers[index];
        cellVisualizer.OnSet_Path_Material();
    }
    void On_SetDefault_Material(int index)
    {
        CellVisualizer_Test cellVisualizer = cellVisualizers[index];
        cellVisualizer.OnSet_Default_Material();
    }
  
    void On_SetNext_Material(int index)
    {
        CellVisualizer_Test cellVisualizer = cellVisualizers[index];
        cellVisualizer.OnSet_Next_Material();
    }
    void On_SetConfirmed_Material(int index)
    {
        CellVisualizer_Test cellVisualizer = cellVisualizers[index];
        cellVisualizer.OnSet_Confirmed_Material();
    }
    /// <summary>
    /// 미로의 데이터(cells 배열)을 null 로 만들고 
    /// 프리팹도 모두 삭제하는 함수
    /// </summary>
    public void DestroyBoard()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Cell cell = cells[i];
            cell.ResetPath();
            cell.on_RefreshWall = null;
            cell.Path = 0;
            //x, y 는 생성시 초기화되기 때문에 생략
            cell = null;
            Pooled_Obj pooled_Obj= transform.GetChild(0).GetComponent<Pooled_Obj>();
            pooled_Obj.ReturnToPool();

        }
        Cells = null;//셀의 배열을 null 로 만들기
        connectOrder.Clear();
        Progress = 0;
        StopMaze();
    }
    public void AddToConnectOrder(Cell from, Cell to)
    {
        connectOrder.Add((from, to));
    }
    
}
