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

    public void BladeColliderEnable(bool enable)//�ݶ��̴���  Ÿ�ֿ̹� ���� �Ѱ� ���� �Լ� 
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
