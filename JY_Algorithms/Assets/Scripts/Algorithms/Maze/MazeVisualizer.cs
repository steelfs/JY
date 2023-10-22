using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;
    public const int CellSize = 5;
    Cell[] cells;
    public List<Cell> connectOrder = new List<Cell>();
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
    public virtual void ConnectPath(Cell from, Cell to)
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
        else if (diffY > 0)// from = ����, to = �Ʒ���
        {
            from.OpenWall(Direction.South);
            to.OpenWall(Direction.North);
        }
        else if (diffY < 0)// from = �Ʒ���, to = ����
        {
            from.OpenWall(Direction.North);
            to.OpenWall(Direction.South);
        }
    }
    public virtual void DisconnectPath(Cell from, Cell to)
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
        else if (diffY > 0)// from = ����, to = �Ʒ���
        {
            from.CloseWall(Direction.South);
            to.CloseWall(Direction.North);
        }
        else if (diffY < 0)// from = �Ʒ���, to = ����
        {
            from.CloseWall(Direction.North);
            to.CloseWall(Direction.South);
        }
    }
    public virtual void RenderBoard(int width, int height, Cell[] cells)// delegate�� �����ϱ� ���� cell�� �迭�� �޴´�.
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.name = $"Cell_{x}_{y}";
                cell.transform.Translate(x * CellSize, 0, -y * CellSize);

                int index = (y * width) + x;
                CellVisualizer cellVisualizer = cell.GetComponent<CellVisualizer>();
                Cell currentCell = cells[index];
                currentCell.on_RefreshWall += cellVisualizer.RefreshWalls;
            }
        }
    }
    public void AddToConnectOrder(Cell cell)
    {
        connectOrder.Add(cell);
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
