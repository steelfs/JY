using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    public int bulletRemain;

    VisualEffect muzzleEffect;
    int onFireID;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
        onFireID = Shader.PropertyToID("OnFire");
    }

    public void Fire()
    {
        if( bulletRemain > 0 )
        {
            muzzleEffect.SendEvent(onFireID);
            bulletRemain--;

            FireProcess();
        }
    }

    protected virtual void FireProcess()
    {
    }

    protected Transform fireTransform;
    public void Equip()
    {
        fireTransform = GameManager.Inst.Player.transform.GetChild(0);
    }

    Vector3 randomPos;
    protected Vector3 GetFireDir()
    {
        Vector3 result = fireTransform.forward;

        randomPos = new Vector3(Random.Range(-spread * 0.01f, spread * 0.01f), Random.Range(-spread * 0.01f, spread * 0.01f), 0);//랜덤한 총알 탄착군 형성
        result = randomPos;
        fireDir = result;
        return result;
    }

    Vector3 fireDir = Vector3.forward;
    private void OnDrawGizmos()
    {
        if (fireDir != null && fireTransform != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(fireTransform.position, fireTransform.position + fireTransform.forward * range);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(fireTransform.position, fireTransform.position + fireDir * range);
        }
       
    }
}
