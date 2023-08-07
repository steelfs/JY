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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))//Ʈ���ſ� ���� ���� ���̰� 
        {
            IBattle target = other.GetComponent<IBattle>();//������ ������ ���̸� 
            if (target != null)
            {
                player.Attack(target);//���� 

                Vector3 inpactPoint = transform.position + transform.up * 0.8f;//Į���κ�
                Vector3 effectPoint = other.ClosestPoint(inpactPoint);// �浹�� �ݶ��̴��� Į���κ��� ���� ����� ��ġ

                Instantiate(hitEffect, effectPoint, Quaternion.identity);

                //Time.timeScale = 0;
            }
        }
    }
}
