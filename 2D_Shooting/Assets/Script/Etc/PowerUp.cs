using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PooledObject
{
    public float moveSpeed = 3.0f;

    public float dirChangeInterval = 1.0f;

    Vector2 dir;

    Transform playerTransform = null;
    int dirChangeCount;
    public int dirchangeMaxCount = 5;
    Animator anim;
    public int DirChangeCount
    {
        get => dirChangeCount;
        set 
        {
            dirChangeCount = value;
            anim.SetInteger("SetInt", dirChangeCount);
           
            StopAllCoroutines(); //�ϴ� ����
        
            if (dirChangeCount > 0 && gameObject.activeSelf) // Ƚ���� ���������� �ڷ�ƾ ����(���͹� ���� ������ȯ)
            {
                StartCoroutine(DirChange());
            }
        }
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        StopAllCoroutines();
        DirChangeCount = dirchangeMaxCount;
        playerTransform = GameManager.Inst.Player.transform;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);
    }
    IEnumerator DirChange()
    {
        // 40% Ȯ���� �÷��̾� �ݴ�������� �̵��ϰ� ����� 
        float normalCase = 0.4f;
        


        yield return new WaitForSeconds(dirChangeInterval);
        if (Random.value < normalCase)
        {
            Vector2 playertoPowerUp = transform.position - playerTransform.position;
            dir = Quaternion.Euler(0,0, Random.Range(-90.0f, 90.0f)) * playertoPowerUp;
        }
        else
        {
            dir = Random.insideUnitCircle;
        }
        dir.Normalize(); // ���⺤�͸� ���ֺ��ͷ� �����ؼ� ���⸸ �����         
        DirChangeCount--; //ī��Ʈ ���ҽ�Ű�鼭 �ڷ�ƾ ��ȣ��
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (dirChangeCount > 0 && collision.gameObject.CompareTag("Boarder"))
        {
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);
            DirChangeCount--;
        }
    }
}
