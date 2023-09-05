using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestChat : TestBase
{
    //protected override void Test1(InputAction.CallbackContext _)
    //{
    //    NetworkObject obj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
    //    GameManager.Inst.Log(obj.gameObject.name);
    //}
    //protected override void Test2(InputAction.CallbackContext _)
    //{
    //    string input = "/setname AAAbb가";
    //    int space = input.IndexOf(' ');
    //    string commandToken = input.Substring(0, space);
    //    commandToken = commandToken.ToLower();
    //    string parameterToken = input.Substring(space + 1);

    //    GameManager.Inst.SetUserName(parameterToken);
    //}
    //protected override void Test3(InputAction.CallbackContext _)
    //{
    //    string commandLine = "/setcolor 1.5f, 0.0f, 0";
    //    int space = commandLine.IndexOf(" ");
    //    string commandToken = commandLine.Substring(0, space);
    //    string paramToken = commandLine.Substring(space + 1);
    //    commandToken = commandToken.ToLower();

    //    string[] splitNumbers = paramToken.Split(',', ' ');

    //    float[] colorValue = new float[4] { 0, 0, 0, 0};
    //    int count = 0;
    //    foreach (string number in splitNumbers)
    //    {
    //        if (number.Length == 0)
    //            continue;

    //        if (count > 3)
    //        {
    //            break;
    //        }
    //        if( float.TryParse(number, out colorValue[count]))//number 를 float 으로 변환
    //        {
    //            colorValue[count] = 0;
    //        }
    //        count++;
    //    }
    //    for (int i = 0; i < colorValue.Length; i++)
    //    {
    //        colorValue[i] = Mathf.Clamp01(colorValue[i]);
    //    }
    //    Color color = new Color(colorValue[0], colorValue[1], colorValue[2], colorValue[3]);
    //}
    NetPlayer player;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        player = GameManager.Inst.Player;
        player.IsEffectOn = true;
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        player.IsEffectOn = false;
    }
}
