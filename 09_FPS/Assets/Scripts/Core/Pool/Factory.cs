using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum Pool_Object_Type
{
    bulletHole = 0,

}

public class Factory : Singleton<Factory>
{
    BulletHole_Pool bulletHolePool;
    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletHolePool = GetComponentInChildren<BulletHole_Pool>();

        bulletHolePool?.Initialize();
    }
    public GameObject GetObject(Pool_Object_Type type, Transform spawn = null)
    {
        GameObject result = null;
        switch (type)
        {
            case Pool_Object_Type.bulletHole:
                result = bulletHolePool?.GetObject(spawn)?.gameObject;
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
    public BulletHole GetbulletHole()
    {
        GameObject obj = GetObject(Pool_Object_Type.bulletHole);
        BulletHole bulletHole = obj.GetComponent<BulletHole>();
        return bulletHole;
    }
    public BulletHole GetbulletHole(Vector3 position, Vector3 normal)
    {
        GameObject obj = GetObject(Pool_Object_Type.bulletHole);
        BulletHole bulletHole = obj.GetComponent<BulletHole>();
        bulletHole.Initialize(position, normal);
        return bulletHole;
    }
}
