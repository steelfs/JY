using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBullet : EnemyBase
{
    public GameObject Hit_Explosion;
    public float lifeTime = 10.0f;
    protected override void OnEnable()
    {
        base.OnEnable();
        LifeOver(lifeTime);
    }
  
   
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject hitExplosion = Factory.Inst.GetObject(Pool_Object_Type.Enemy_Explosion);
            hitExplosion.transform.SetParent(null); //����Ʈ�� �θ� ����
            hitExplosion.transform.position = collision.contacts[0].point; //�浹�������� ����Ʈ ��ġ �ű��
            hitExplosion.transform.Rotate(0, 0, UnityEngine.Random.Range(0, 360.0f));
            hitExplosion.SetActive(true);

            gameObject.SetActive(false);
            //  Hit_Explosion.SetActive(false);
        }
    }
}
