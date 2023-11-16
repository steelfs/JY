using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : TestBase
{
    public Vector3 world;
    public int amount;
    WaitForSeconds sec;
    protected override void Awake()
    {
        base.Awake();
        sec = new WaitForSeconds(2);
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < 100000; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Cell")))
            {
                CellVisualizer cell = hitInfo.transform.GetComponentInParent<CellVisualizer>();
            }
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Test1 Time: {stopwatch.ElapsedMilliseconds} ms");
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < 100000; i++)
        {
            Vector2 mouse = Mouse.current.position.ReadValue();
            Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

            Vector2Int grid = Util.WorldToGrid(world);//클릭한 곳의 그리드상 위치
            CellVisualizer to = GameManager.Visualizer.CellVisualizers[Util.GridToIndex(grid.x, grid.y)];
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Test2 Time: {stopwatch.ElapsedMilliseconds} ms");
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
    }

    void Test1()
    {
  
    }
    void Test2()
    {
    }
}
