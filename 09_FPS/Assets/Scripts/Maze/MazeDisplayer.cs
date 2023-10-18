using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MazeDisplayer : TestBase
{
    public GameObject cellPrefab;
    BackTracking backTracking;
    int i = 0;

    protected override void Awake()
    {
        base.Awake();
        backTracking = new BackTracking();
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        Cell[] cells;

        cells = backTracking.MakeMaze(10, 10, (int)DateTime.Now.Ticks);
        Draw(cells);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        Clear();
    }
    public void Draw(Cell cell)
    {
        GameObject obj = Instantiate(cellPrefab, transform);
        obj.transform.Translate(cell.X * CellDisplayer.CellSize, 0, -cell.Y * CellDisplayer.CellSize);
        obj.gameObject.name = $"Cell_{cell.X}_{cell.Y}";

        CellDisplayer displayer = obj.GetComponent<CellDisplayer>();
        displayer.RefreshWall(cell.Path);
    }
    public void Draw(Cell[] data)
    {
        BackTrackingCell backTrackingCell;
        foreach (Cell cell in data)
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * CellDisplayer.CellSize, 0, -cell.Y * CellDisplayer.CellSize);
            obj.gameObject.name = $"Cell_{cell.X}_{cell.Y}";

            CellDisplayer displayer = obj.GetComponent<CellDisplayer>();
            displayer.RefreshWall(cell.Path);
            backTrackingCell = cell as BackTrackingCell;
            if (!backTrackingCell.visited)
            {
                Debug.Log($"{backTrackingCell}_{backTrackingCell.X}_{backTrackingCell.Y}");
            }
        }
    }
    public void Clear()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);// 생략시 무한루프 때는데 시간이 오래걸려서 child에서 제외되지 않음
            Destroy(child.gameObject);
        }
    }
}
