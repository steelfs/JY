using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum GunType
{
    Revolver,
    Shotgun,
    AssaultRifle
}
public class GunBase : MonoBehaviour
{
    /// <summary>
    /// 총의 사정거리
    /// </summary>
    public float range;

    /// <summary>
    /// 총의 데미지
    /// </summary>
    public float damage;

    /// <summary>
    /// 초당 연사속도
    /// </summary>
    public float fireRate;

    /// <summary>
    /// 현재 발사가 가능한지 여부
    /// </summary>
    protected bool isFireReady = true;

    /// <summary>
    /// 탄 퍼지는 각도
    /// </summary>
    public float spread;

    /// <summary>
    /// 총 반동
    /// </summary>
    public float recoil;

    /// <summary>
    /// 탄창 크기
    /// </summary>
    public int clipSize;

    /// <summary>
    /// 남은 총알 수
    /// </summary>
    protected int bulletCount;
    public int BulletCount
    {
        get => bulletCount;
        set
        {
            bulletCount = value;
            on_BulletCountChange?.Invoke(bulletCount);
        }
    }
    public Action<int> on_BulletCountChange;
    public Action<float> on_FireRecoil;

    protected VisualEffect muzzleEffect;
    int onFireID;

    protected Transform fireTransform;
    public Transform FireTransform => fireTransform;

    GunType gunType;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
        onFireID = Shader.PropertyToID("OnFire");
    }
    void Initialize()
    {
        BulletCount = clipSize;
        isFireReady = true;
    }
    public void Fire(bool isStart = true)
    {
        if(isFireReady && bulletCount > 0 )
        {
            FireProcess(isStart);
        }
    }
    protected void MuzzleEffect()
    {
        muzzleEffect.SendEvent(onFireID);

    }
    IEnumerator FireReady()
    {        
        yield return new WaitForSeconds(1/fireRate);
        isFireReady = true;
    }
        
    public IEnumerator TestFire(int count)
    {
        float startTime = Time.unscaledTime;
        
        while (count > 0)
        {
            if(isFireReady)
            {
                Fire();
                count--;
            }
            yield return null;
        }

        float endTime = Time.unscaledTime;
        Debug.Log($"전체 진행 시간 : {endTime - startTime}");
    }

    protected virtual void FireProcess(bool isFireStart = true)
    {
        muzzleEffect.SendEvent(onFireID);
        BulletCount--;
        isFireReady = false;
        StartCoroutine(FireReady());

    }

    protected void FireRecoil()
    {
        //Time.timeScale = 0.1f;
        on_FireRecoil?.Invoke(recoil);
    }

  
    public void Equip()
    {
        Initialize();
    }

    protected Vector3 GetFireDirection()
    {
        Vector3 result = GameManager.Inst.Player.FireTransform.forward;

        result = Quaternion.AngleAxis(UnityEngine.Random.Range(-spread, spread), GameManager.Inst.Player.FireTransform.right) * result; // 위 아래로 회전        
        result = Quaternion.AngleAxis(UnityEngine.Random.Range(0.0f, 360.0f), GameManager.Inst.Player.FireTransform.forward) * result;  // forward 축을 기준으로 0~360사이로 회전

        //fireDir = result;

        return result;
    }

    //Vector3 fireDir = Vector3.forward;
    void OnDrawGizmos()
    {
        if (fireTransform != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(GameManager.Inst.Player.FireTransform.position, GameManager.Inst.Player.FireTransform.position + GameManager.Inst.Player.FireTransform.forward * range);

            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(fireTransform.position, fireTransform.position + fireDir * range);

        }
    }
}
