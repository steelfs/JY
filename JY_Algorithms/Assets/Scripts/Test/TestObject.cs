using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : TestBase
{
    public Cell cell;
    public Direction direction;
    protected override void Awake()
    {
        base.Awake();
        cell = new Cell(1,1);
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        cell.OpenWall(direction);
        Debug.Log($"{direction} is Open");
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        cell.CloseWall(direction);

        Debug.Log($"{direction} is Close");
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        bool isOpen = cell.IsOpened(direction);
        Debug.Log($"{direction} = {isOpen}");
    }

}
