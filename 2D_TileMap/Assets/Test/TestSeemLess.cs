using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSeemLess : TestBase
{
    public int x = 0;
    public int y = 0;
    WorldManager world;
    Player player;
    private void Start()
    {
        world = GameManager.Inst.WorldManager;
        player = GameManager.Inst.Player;
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        world.TestLoadScene(x, y);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        world.TestUnLoadScene(x, y);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        world.TestRefresh(x, y);
    }
}
