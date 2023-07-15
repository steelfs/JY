using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ObjectPool : TestBase
{
    public Pool_Object_Type object_Type;

    List<GameObject> objects = new List<GameObject>();

    protected override void Test1(InputAction.CallbackContext context)
    {
        GameObject test = Factory.Inst.GetObject(object_Type);
        test.transform.position = transform.position;
        objects.Add(test);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        while(objects.Count > 0)
        {
            GameObject del = objects[0];
            objects.RemoveAt(0);
            del.gameObject.SetActive(false);
        }
        objects.Clear();
    }
}
