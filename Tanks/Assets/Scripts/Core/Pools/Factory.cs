using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum PoolObjectType
{
    Shell = 0,       // 총알 구멍
    Explosion
}

public class Factory : Singleton<Factory>
{    
    ShellPool ShellPool;
    ExplosionPool ExplosionPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        ShellPool = GetComponentInChildren<ShellPool>();
        ExplosionPool = GetComponentInChildren<ExplosionPool>();

        ShellPool?.Initialize();
        ExplosionPool?.Initialize();
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
            case PoolObjectType.Shell:
                result = ShellPool?.GetObject(spawn)?.gameObject;                
                break;
            case PoolObjectType.Explosion:
                result = ExplosionPool?.GetObject(spawn)?.gameObject;
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
    public GameObject GetObject(PoolObjectType type, Vector3 position, float angle = 0.0f)
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
    public Explosion GetExplosion(Vector3 position, Vector3 normal)
    {
        GameObject obj = GetObject(PoolObjectType.Explosion, position);
        Explosion explosion = obj.GetComponent<Explosion>();
        explosion.Initialize(position, normal);
        return explosion;
        
    }
    public Shell GetShell(Transform parent = null)
    {
        GameObject obj = GetObject(PoolObjectType.Shell);
        Shell shell = obj.GetComponent<Shell>();
        return shell;
    }
    /// <summary>
    /// 총알 구멍을 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    //public BulletHole GetBulletHole()
    //{
    //    GameObject obj = GetObject(PoolObjectType.BulletHole);
    //    BulletHole hole = obj.GetComponent<BulletHole>();
    //    return hole;
    //}

    //public BulletHole GetBulletHole(Vector3 position, Vector3 normal, Vector3 reflect)
    //{
    //    GameObject obj = GetObject(PoolObjectType.BulletHole);
    //    BulletHole hole = obj.GetComponent<BulletHole>();
    //    hole.Initialize(position, normal, reflect);
    //    return hole;
    //}
}
