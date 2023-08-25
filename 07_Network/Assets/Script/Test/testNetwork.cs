using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class testNetwork : TestBase
{
    public Logger logger;
    public string testText;

    public Vector3 newPos = new Vector3();
    protected override void Test3(InputAction.CallbackContext context)
    {
        NetPlayer player = FindObjectOfType<NetPlayer>();
        player.position.Value = newPos;
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("11111");
        Debug.Log(sb.ToString());
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        logger.Log(testText);
    }
}
