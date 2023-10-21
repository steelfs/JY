using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Runner : TestBase
{
    public MazeVisualizer BackTrackingVisualizer;
    public CellVisualizer cell;
    public Direction direction;
    protected override void Test1(InputAction.CallbackContext _)
    {
        cell.RefreshWalls((byte)direction);
    }
}
