using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Runner : TestBase
{
    public BackTrackingVisualizer BackTrackingVisualizer;
    public CellVisualizer cell;
    public Direction direction;
    public int width;
    public int height;
    public int fromIndex;
    public int toIndex;
    //protected override void Test1(InputAction.CallbackContext _)
    //{
    //    BackTrackingVisualizer.MakeBoard(width, height);
    //}
    //protected override void Test2(InputAction.CallbackContext context)
    //{
    //    BackTrackingVisualizer.ConnectPath(BackTrackingVisualizer.Cells[fromIndex], BackTrackingVisualizer.Cells[toIndex]);
    //}
    //protected override void Test3(InputAction.CallbackContext context)
    //{
    //    BackTrackingVisualizer.DisconnectPath(BackTrackingVisualizer.Cells[fromIndex], BackTrackingVisualizer.Cells[toIndex]);
    //}
    //protected override void Test4(InputAction.CallbackContext context)
    //{
    //    BackTrackingVisualizer.InitBoard();
    //}
}
