using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    CapsuleCollider bladeCollider;
    ParticleSystem ps;

    private void Awake()
    {
        bladeCollider = GetComponent<CapsuleCollider>();
        ps = GetComponent<ParticleSystem>();
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
}
