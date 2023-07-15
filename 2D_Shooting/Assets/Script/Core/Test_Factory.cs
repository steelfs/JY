using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Factory : TestBase
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
            Destroy(del);
        }
        objects.Clear();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        EnemyCurve curve = Factory.Inst.GetEnemyCurve(transform.position);
        objects.Add(curve.gameObject);
    }
}
