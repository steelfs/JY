using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test11_2SaveLoad : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        bool result = GameManager.Inst.SaveShipDeployData(GameManager.Inst.UserPlayer);
        if (result)
        {
            Debug.Log("저장 성공");
        }
        else
        {
            Debug.Log("저장 실패");
        }
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        bool result = GameManager.Inst.LoadShipDeployData(GameManager.Inst.UserPlayer);
        if (result)
        {
            Debug.Log("로딩 성공 ");
        }
        else
        {
            Debug.Log("로딩 실패");
        }
    }
}
