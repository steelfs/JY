using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : PooledObject
{  
    public float speed = 9.0f;
    public float lifeTime = 10.0f;

    //delegate void OnEnemyKill(int score); //��������Ʈ ����  ���� Ÿ�� void
    //OnEnemyKill onEnemyKill;
    //public Action<int> onEnemyKill; //���� Kill���� �� ��ȣ�� ������ delegate


    protected override void OnEnable()
    {
        base.OnEnable();
        ///��������   ��ũ��Ʈ���� getChild�� ã�°Ͱ�  public GameObject�� �ν����Ϳ��� �Ҵ��ϴ°��̶� �������� ���鿡�� ���̰� �ִ°�
        StopAllCoroutines(); 
        StartCoroutine(LifeOver(lifeTime));
    }
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.right);// Space.Self = �ڽ��� �������� ������ ��������.  Space.World ���带 �������� ������ ��������.
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Factory.Inst.GetObject(Pool_Object_Type.PLayer_hit, collision.contacts[0].point, UnityEngine.Random.Range(0, 360.0f));
            gameObject.SetActive(false);
        }   
        //hitExplosion.SetActive(true);
        //hitExplosion.transform.position = collision.contacts[0].point; //�浹�������� ����Ʈ ��ġ �ű��
        //hitExplosion.transform.Rotate(0, 0, UnityEngine.Random.Range(0, 360.0f));

        //EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>(); // �±װ� Enemy �̱⶧���� EnemyBase�� null�� �ƴϴ�.
        // onEnemyKill?.Invoke(enemy.Score); // onEnemyKill�� ����� �Լ��� ��� �����ϱ� (�ϳ��� ������ ����)
    }
}
