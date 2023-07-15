using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum Pool_Object_Type
{
    Slime = 0,

}

public class Factory : Singleton<Factory>
{
    SlimePool slimePool;
    protected override void OnInitialize()
    {
        base.OnInitialize();

        slimePool = GetComponentInChildren<SlimePool>();

        slimePool?.Initialize();
    }
    public GameObject GetObject(Pool_Object_Type type, Transform spawn = null)
    {
        GameObject result = null;
        switch (type)
        {
            case Pool_Object_Type.Slime:
                result = slimePool?.GetObject(spawn)?.gameObject;
                break;
        
       
            default:
                result = new GameObject();
                break;
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">������Ʈ ����</param>
    /// <param name="position">��������ġ (������ǥ)</param>
    /// <param name="angle">z�� ȸ�� ����</param>
    /// <returns></returns>
    public GameObject GetObject(Pool_Object_Type type,Vector3 position, float angle = 0.0f) //�����ε� �Լ� �������鼭 ��ġ�� ������ �����ϴ� �Լ�
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;
        obj.transform.Rotate(angle * Vector3.forward);

        switch (type)
        {
            default:
                break;
        }
        return obj;
    }
    public Slime GetSlime()
    {
        GameObject obj = GetObject(Pool_Object_Type.Slime);
        Slime slime = obj.GetComponent<Slime>();
        return slime;
    }
}
