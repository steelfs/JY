using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cho_Test_Map : TestBase
{
    bool isExist = false;
    bool isPropExist = false;

    public MapTest map;

    protected override void Test1(InputAction.CallbackContext context)
    {
        if (!isExist)                   // 타일이 존재하지 않을 경우에만 생성
        {
            map.Test1();
            isExist = true;         // 중복 맵 생성 방지

        }
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        if (isExist && !isPropExist)
        {
            map.Test2();

            isExist = false;
        }
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        if (isExist)
        {
            map.Test3();
            isPropExist = true;
        }
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        if (isPropExist)
        {
            map.Test4();
            isPropExist = false;
        }
    }
}
