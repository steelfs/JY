using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    shell = 0,       // 총알 구멍
}

public class Factory : Singleton<Factory>
{    
    BulletPool bulletPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletPool = GetComponentInChildren<BulletPool>();

        bulletPool?.Initialize();
    }

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오는 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetObject(PoolObjectType type, Transform spawn = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.shell:
                result = bulletPool?.GetObject(spawn)?.gameObject;                
                break;
            default:
                result = new GameObject();
                break;
        }

        return result;
    }

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오면서 위치와 각도를 설정하는 함수
    /// </summary>
    /// <param name="type">생성할 오브젝트의 종류</param>
    /// <param name="position">생성할 위치(월드좌표)</param>
    /// <param name="angle">z축 회전 정도</param>
    /// <returns>생성한 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        switch (type)
        {        
            default:                
                break;
        }
        
        return obj;
    }

    /// <summary>
    /// 총알 구멍을 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public Shell GetBullet()
    {
        GameObject obj = GetObject(PoolObjectType.shell);
        Shell hole = obj.GetComponent<Shell>();
        return hole;
    }

  
}
