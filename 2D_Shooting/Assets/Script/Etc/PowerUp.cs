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
           
            StopAllCoroutines(); //일단 정지
        
            if (dirChangeCount > 0 && gameObject.activeSelf) // 횟수가 남아있으면 코루틴 실행(인터벌 이후 방향전환)
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
        // 40% 확률로 플레이어 반대방향으로 이동하게 만들기 
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
        dir.Normalize(); // 방향벡터를 유닛벡터로 변경해서 방향만 남기기         
        DirChangeCount--; //카운트 감소시키면서 코루틴 재호출
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
