
using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : PooledObject
{
    [Header("Base data")]
    public float speed = 3.0f;
    public float waitTimeX = 1.0f;  

    //[SerializeField]
    public int score = 10; //적이 주는 점수
    public int Score => score;

    public int maxHp = 1;
    public int hp = 1;

    Player targetPlayer = null;

    public int Hp
    {
        get => hp;
        protected set
        {
            if(hp != value)
            {
                hp = value;
                if (hp <= 0)
                {
                    Die();
                }
            }
        }
    }

    public Action<int> onDie;
    protected virtual void Awake()
    {
  
        
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();
        hp = maxHp;
    }

    protected override void OnDisable()
    {         
        if (targetPlayer != null)
        {
            onDie -= targetPlayer.AddScore;
        }
        base.OnDisable();
    }
    void Update()
    {
        OnMoveUpdate(); //각 클래스별 이동 업데이트 함수 실행
    }

    protected virtual void OnMoveUpdate() // 업데이트에서 실행되는 이동처리 함수
    {
        transform.Translate(Time.deltaTime * speed * -transform.right); // 그냥 왼쪽으로 이동하기
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Hp--;            
        }
    }
    protected virtual void OnInitialize()
    {
        if (targetPlayer == null)
        {
            targetPlayer = GameManager.Inst.Player;
        }
        if (targetPlayer != null)
        {
            onDie += targetPlayer.AddScore;
        }
       
    }
    protected virtual void Die()
    {
        GameObject explosionEffect = Factory.Inst.GetObject(Pool_Object_Type.Enemy_Explosion, transform.position, UnityEngine.Random.Range(0.0f, 360.0f));
        //explosion.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360));
       // explosionEffect.SetActive(true);
        onDie?.Invoke(score);
       
        gameObject.SetActive(false);
    }
}
///CallStack 디버그창에서 멘마지막부분이 먼저 실행된 것이다.
