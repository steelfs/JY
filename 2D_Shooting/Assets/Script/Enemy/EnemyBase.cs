
using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : PooledObject
{
    [Header("Base data")]
    public float speed = 3.0f;
    public float waitTimeX = 1.0f;  

    //[SerializeField]
    public int score = 10; //���� �ִ� ����
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
        OnMoveUpdate(); //�� Ŭ������ �̵� ������Ʈ �Լ� ����
    }

    protected virtual void OnMoveUpdate() // ������Ʈ���� ����Ǵ� �̵�ó�� �Լ�
    {
        transform.Translate(Time.deltaTime * speed * -transform.right); // �׳� �������� �̵��ϱ�
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
///CallStack �����â���� �ึ�����κ��� ���� ����� ���̴�.
