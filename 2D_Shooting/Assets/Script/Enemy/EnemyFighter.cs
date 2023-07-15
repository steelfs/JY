using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : EnemyBase
{
    [Header("Fighter data")]
    public float amplitude = 4.0f;
    public float frequency = 2.0f; //사인그래프가 한번 왕복하는데 걸리는 시간
    public float spawnY;// 시작할때 높이
    float timeElapsed = 0.0f;//시작부터 경과시간
    protected override void OnEnable()
    {
        base.OnEnable();
        spawnY = transform.position.y;
        StopAllCoroutines();
        StartCoroutine(WaitCoroutine());
    }

    // Update is called once per frame
    IEnumerator WaitCoroutine()
    {
        const float waitTime = 3.0f;
        const float waitTime2 = 1.0f;
        yield return new WaitForSeconds(waitTime); //시작하고 3초 기다리기.
        float priviousSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(waitTime2);
        speed = priviousSpeed;

        //yield return null ;  다음프레임까지 기다리기 
    }
    protected override void OnMoveUpdate()
    {

        timeElapsed += Time.deltaTime * frequency; //사인 함수에서 사용할 파라미터 계산

        transform.position = new Vector3(transform.position.x - Time.deltaTime * speed, // x = 현재위치에서 왼쪽

            spawnY + Mathf.Sin(timeElapsed) * amplitude, transform.position.z); //spawnY 를 기준으로 sin함수 이용해서 높이 결정

        //transform.Translate(Time.deltaTime * speed * Vector2.left);
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
       // Hp = 2;
    }
    //protected override void OnCollisionEnter2D(Collision2D collision)
    //{
    //    base.OnCollisionEnter2D(collision);
    //}
}
