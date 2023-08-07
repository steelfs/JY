using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    CapsuleCollider bladeCollider;
    ParticleSystem ps;
    public GameObject hitEffect;

    Player player;


    private void Awake()
    {
        bladeCollider = GetComponent<CapsuleCollider>();
        ps = GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        player = GameManager.Inst.Player;
    }
    public void BladeColliderEnable(bool enable)//콜라이더를  타이밍에 맞춰 켜고 끄는 함수 
    {
        bladeCollider.enabled = enable;
    }
    public void EffectEnable(bool enable)
    {
        if (enable)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))//트리거에 들어온 것이 적이고 
        {
            IBattle target = other.GetComponent<IBattle>();//전투가 가능한 적이면 
            if (target != null)
            {
                player.Attack(target);//공격 

                Vector3 inpactPoint = transform.position + transform.up * 0.8f;//칼날부분
                Vector3 effectPoint = other.ClosestPoint(inpactPoint);// 충돌한 콜라이더와 칼날부분의 가장 가까운 위치

                Instantiate(hitEffect, effectPoint, Quaternion.identity);

                //Time.timeScale = 0;
            }
        }
    }
}
