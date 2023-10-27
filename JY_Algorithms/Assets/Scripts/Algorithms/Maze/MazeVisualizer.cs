using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public enum MazeType
{
    None,
    BackTracking,
    Wilson
}
public class MazeVisualizer : MonoBehaviour
{
    RecursiveBackTracking backTracking;
    public RecursiveBackTracking BackTracking => backTracking;  

    MazeType mazeType;
    public MazeType MazeType
    {
        get { return mazeType; }
        set { mazeType = value; }
    }

    public GameObject cellPrefab;
    public const int CellSize = 5;
    Cell[] cells = null;
    public bool IsExistCells => cells != null;
    int maxProgress => connectOrder.Count - 1;
    int progress = 0;

    int Progress
    {
        get => progress;
        set
        {
            progress = value;
            if (connectOrder.Count > 0)// �̷ΰ� �������� ����
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

    Vector3 sixPos = new Vector3(22.5f, 0, -26);
    Vector3 sevenPos = new Vector3(25.5f, 0, -30);
    public Cell[] Cells
    {
        get => cells;
        set { cells = value; }
    }
    public virtual void MakeBoard(int x, int y)
    {
        switch (mazeType)
        {
            case MazeType.None:
                break;
            case MazeType.BackTracking:
                backTracking = new RecursiveBackTracking();
                Cells = backTracking.MakeCells(x, y);

                RenderBoard(x, y, Cells);
                break;
            case MazeType.Wilson:
                break;
            default: 
                break;
        }
    }
    public virtual void InitBoard()
    {
        foreach (Cell cell in cells)
        {
            cell.ResetPath();
        }
    }

    /// <summary>
    /// ���������� �����ϴ� �Լ��� ���� �ð����� ��� �����ϴ� �Լ�
    /// </summary>
    public void PlayMaze()
    {
        InvokeRepeating("MoveToNext", 0f, 0.15f);
    }

    /// <summary>
    /// �̷��� ����� �Ͻ����� �ϴ� �Լ� 
    /// </summary>
    public void StopMaze()
    {
        CancelInvoke();
    }

    /// <summary>
    /// �̷��� ó���������� �ѹ��� �̵��ϴ� �Լ� 
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
    /// �̷��� �� �� �������� �ѹ��� �̵��ϴ� �Լ�
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
    /// �̷ΰ� ��� ������� �� ����� ����� ����Ʈ���� ������� �����ϴ� �Լ�
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
    /// �� �ΰ��� �޾Ƽ� ����(�� ��Ȱ��ȭ)�ϴ� �Լ�
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void ConnectPath(Cell from, Cell to)
    {
        int diffX = from.X - to.X;
        int diffY = from.Y - to.Y;
        if (diffX < 0)// from =���� , to = ������
        {
            from.OpenWall(Direction.East);
            to.OpenWall(Direction.West);
        }
        else if (diffX > 0)// 
        {
            from.OpenWall(Direction.West);
            to.OpenWall(Direction.East);
        }
        else if (diffY > 0)// from = �Ʒ���, to = ����
        {
            from.OpenWall(Direction.North);
            to.OpenWall(Direction.South);
        }
        else if (diffY < 0)// from = ����, to = �Ʒ���
        {
            from.OpenWall(Direction.South);
            to.OpenWall(Direction.North);
        }
    }

    /// <summary>
    /// �� �� ���� �޾Ƽ� ������ ����(�� Ȱ��ȭ) �Լ�
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void DisconnectPath(Cell from, Cell to)
    {
        int diffX = from.X - to.X;
        int diffY = from.Y - to.Y;
        if (diffX < 0)// from =���� , to = ������
        {
            from.CloseWall(Direction.East);
            to.CloseWall(Direction.West);
        }
        else if (diffX > 0)// 
        {
            from.CloseWall(Direction.West);
            to.CloseWall(Direction.East);
        }
        else if (diffY > 0)// from = �Ʒ���, to = ����
        {
            from.CloseWall(Direction.North);
            to.CloseWall(Direction.South);
        }
        else if (diffY < 0)// from = ����, to = �Ʒ���
        {
            from.CloseWall(Direction.South);
            to.CloseWall(Direction.North);
        }
    }

    /// <summary>
    /// �˰������� �̷θ� ��� ���� �� �� �������� ������� �̷��� ��ġ�� �°� 
    /// �����ϴ� �Լ�
    /// </summary>
    /// <param name="width">������ X</param>
    /// <param name="height">������ Y</param>
    /// <param name="cells">���带 �����ϴ� cell �� �迭</param>
    public void RenderBoard(int width, int height, Cell[] cells)// delegate�� �����ϱ� ���� cell�� �迭�� �޴´�.
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
                cell.transform.Translate(x * CellSize, 0, -y * CellSize, Space.Self);// ������ ���÷����̼��� 180�� ���ȱ� ������ ���������� ���

                int index = (y * width) + x;
                CellVisualizer cellVisualizer = cell.GetComponent<CellVisualizer>();
                Cell currentCell = cells[index];
                currentCell.on_RefreshWall = cellVisualizer.RefreshWalls;//UI ������Ʈ ����
            }
        }
    }


    /// <summary>
    /// �̷��� ������(cells �迭)�� null �� ����� 
    /// �����յ� ��� �����ϴ� �Լ�
    /// </summary>
    public void DestroyBoard()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Cell cell = cells[i];
            cell.ResetPath();
            cell.on_RefreshWall = null;
            cell.Path = 0;
            //x, y �� ������ �ʱ�ȭ�Ǳ� ������ ����
            cell = null;
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Cells = null;//���� �迭�� null �� �����
        connectOrder.Clear();
        Progress = 0;
        StopMaze();
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
