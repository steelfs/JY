using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Division_Cell_Test : Cell
{
    public Division_Cell_Test(int x, int y) : base(x, y)
    {

    }
}
public class Division_Test : MazeGenerator_Test
{
    Stack<List<Division_Cell_Test>> division_Cells = new Stack<List<Division_Cell_Test>>();

}
