using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerHP : TestBase
{
    Player player;
    void Start()
    {
        player = GameManager.Inst.Player;
    }
    //protected override void Test1(InputAction.CallbackContext context)
    //{
    //    player.HP -= 17;
    //}
    //protected override void Test2(InputAction.CallbackContext context)
    //{
    //    player.HP += 11;
    //}
    //protected override void Test3(InputAction.CallbackContext context)
    //{
    //    player.HealthRegenerate(30, 1.0f);
    //}
    //protected override void Test4(InputAction.CallbackContext context)
    //{
    //    player.MP -= 17;
    //}
    //protected override void Test5(InputAction.CallbackContext context)
    //{ 
    //    player.MP += 11;
    //}
    protected override void Test6(InputAction.CallbackContext _)
    {
        player.HP -= 99;
        player.MP -= 150;
    }
}
