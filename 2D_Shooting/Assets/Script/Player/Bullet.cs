using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : PooledObject
{  
    public float speed = 9.0f;
    public float lifeTime = 10.0f;

    //delegate void OnEnemyKill(int score); //델리게이트 선언  리턴 타입 void
    //OnEnemyKill onEnemyKill;
    //public Action<int> onEnemyKill; //적을 Kill했을 때 신호를 보내는 delegate


    protected override void OnEnable()
    {
        base.OnEnable();
        ///질문사항   스크립트에서 getChild로 찾는것과  public GameObject로 인스펙터에서 할당하는것이랑 성능적인 측면에서 차이가 있는가
        StopAllCoroutines(); 
        StartCoroutine(LifeOver(lifeTime));
    }
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.right);// Space.Self = 자신을 기준으로 방향이 정해진다.  Space.World 월드를 기준으로 방향이 정해진다.
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Factory.Inst.GetObject(Pool_Object_Type.PLayer_hit, collision.contacts[0].point, UnityEngine.Random.Range(0, 360.0f));
            gameObject.SetActive(false);
        }   
        //hitExplosion.SetActive(true);
        //hitExplosion.transform.position = collision.contacts[0].point; //충돌지점으로 이펙트 위치 옮기기
        //hitExplosion.transform.Rotate(0, 0, UnityEngine.Random.Range(0, 360.0f));

        //EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>(); // 태그가 Enemy 이기때문에 EnemyBase가 null이 아니다.
        // onEnemyKill?.Invoke(enemy.Score); // onEnemyKill에 연결된 함수를 모두 실행하기 (하나도 없으면 실행)
    }
}
