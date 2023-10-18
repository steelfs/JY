using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public BackTrackingCell(int x, int y) : base(x, y) // base먼저 실행 후  실행 
    {

    }
}
public class BackTracking : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        
    }
}
