using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Division_Cell : Cell
{
    public Division_Cell(int x, int y) : base(x, y)
    {

    }
}
public class Division : MazeGenerator
{
    Stack<List<Division_Cell>> division_Cells = new Stack<List<Division_Cell>>();

}
