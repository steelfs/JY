using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObject : TestBase
{
    
    protected override void Test1(InputAction.CallbackContext context)
    {
        PlayerPrefs.DeleteAll();
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        PlayerPrefs.SetFloat("HP", 50.5f);
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        //float hp = PlayerPrefs.GetFloat("HP");
        //Debug.Log(hp);

        bool result = PlayerPrefs.HasKey("MP");
        Debug.Log($"MP 존재 여부 = {result}");
    }

}
