using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Pool_Object_Type
{
    Player_Bullet = 0,
    PLayer_hit,
    Enemy_Boss,
    Enemy_BossBullet,
    Enemy_BossMissile,
    Enemy_Explosion,
    Enemy_Asteroid,
    Enemy_Asteroid_Mini,
    Enemy_Curve,
    Enemy_Fighter,
    Enemy_Strike,
    Monster1,
    Monster2,
    Monster3,
    PowerUp
}

public class Factory : Singleton<Factory>
{
    //public GameObject PlayerBullet;
    //public GameObject BossBullet;
    BulletPool bulletPool;
    BossPool bossPool;
    BossBulletPool bossBulletPool;
    BossMissilePool bossMissilePool;
    PlayerHitPool hitPool;
    ExplosionPool explosionPool;
    EnemyAsteroidPool enemyAsteroidPool;
    EnemyAsteroidMiniPool enemyAsteroidMiniPool;
    EnemyFighterPool enemyFighterPool;
    EnemyCurvePool enemyCurvePool;
    EnemyStrikePool enemyStrikePool;
    Monster1Pool monster1Pool;
    Monster2Pool monster2Pool;
    Monster3Pool monster3Pool;
    PowerUpPool powerUpPool;


    protected override void OnInitialize()
    {
        base.OnInitialize();
        bulletPool = GetComponentInChildren<BulletPool>();
        bossPool = GetComponentInChildren<BossPool>();
        bossBulletPool = GetComponentInChildren<BossBulletPool>();
        bossMissilePool = GetComponentInChildren<BossMissilePool>();
        hitPool = GetComponentInChildren<PlayerHitPool>();
        explosionPool = GetComponentInChildren<ExplosionPool>();
        enemyAsteroidPool = GetComponentInChildren<EnemyAsteroidPool>();
        enemyAsteroidMiniPool = GetComponentInChildren<EnemyAsteroidMiniPool>();
        enemyFighterPool = GetComponentInChildren<EnemyFighterPool>();
        enemyCurvePool = GetComponentInChildren<EnemyCurvePool>();
        enemyStrikePool = GetComponentInChildren<EnemyStrikePool>();
        monster1Pool = GetComponentInChildren<Monster1Pool>();
        monster2Pool = GetComponentInChildren<Monster2Pool>();
        monster3Pool = GetComponentInChildren<Monster3Pool>();
        powerUpPool = GetComponentInChildren<PowerUpPool>();

        bulletPool?.Initialize();
        bossPool?.Initialize();
        bossBulletPool?.Initialize();
        bossMissilePool?.Initialize();
        hitPool?.Initialize();
        explosionPool?.Initialize();
        enemyAsteroidPool?.Initialize();
        enemyAsteroidMiniPool?.Initialize();
        enemyFighterPool?.Initialize();
        enemyCurvePool?.Initialize();
        enemyStrikePool?.Initialize();
        monster1Pool?.Initialize();
        monster2Pool?.Initialize();
        monster3Pool?.Initialize();
        powerUpPool?.Initialize();


    }
    public GameObject GetObject(Pool_Object_Type type)
    {
        GameObject result;
        switch (type)
        {
            case Pool_Object_Type.Player_Bullet:
                result = bulletPool.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.PLayer_hit :
                result = hitPool?.GetObject()?.gameObject;
                 break;
            case Pool_Object_Type.Enemy_Boss:
                result = bossPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_BossBullet:
                result = bossBulletPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_BossMissile:
                result = bossMissilePool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_Explosion:
                result = explosionPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_Asteroid:
                result = enemyAsteroidPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_Asteroid_Mini:
                result = enemyAsteroidMiniPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_Fighter:
                result = enemyFighterPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_Strike:
                result = enemyStrikePool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Enemy_Curve:
                result = enemyCurvePool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Monster1:
                result = monster1Pool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Monster2:
                result = monster2Pool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Monster3:
                result = monster3Pool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.PowerUp:
                result = powerUpPool?.GetObject()?.gameObject;
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
            case Pool_Object_Type.Enemy_Asteroid_Mini:
                obj.transform.position = position;
                obj.transform.Rotate(angle * Vector3.forward);
                EnemyAsteroidMini mini = obj.GetComponent<EnemyAsteroidMini>();
                mini.Direction = -mini.transform.right;
                break;
            case Pool_Object_Type.Enemy_Curve:
                EnemyCurve curve = obj.GetComponent<EnemyCurve>();
                curve.StartY = position.y;
                break;

        }
        return obj;
    }

    public EnemyAsteroidMini GetAsteroidMini(Vector3 pos, float angle = 0.0f)
    {
        EnemyAsteroidMini mini = enemyAsteroidMiniPool.GetObject();
        mini.transform.position = pos;
        mini.transform.Rotate(angle * Vector3.forward);
        mini.Direction = -mini.transform.right;
        return mini;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos">������ǥ���� �����Ǵ� ��ġ</param>
    /// <returns>������ Ŀ�� ��</returns>
    public EnemyCurve GetEnemyCurve(Vector3 pos)
    {
        EnemyCurve curve = enemyCurvePool.GetObject();
        curve.transform.position = pos;
        curve.StartY = pos.y;
        return curve;
    }
}
