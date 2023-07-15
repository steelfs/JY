using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : EnemyBase
{
    [Header("Fighter data")]
    public float amplitude = 4.0f;
    public float frequency = 2.0f; //���α׷����� �ѹ� �պ��ϴµ� �ɸ��� �ð�
    public float spawnY;// �����Ҷ� ����
    float timeElapsed = 0.0f;//���ۺ��� ����ð�
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
        yield return new WaitForSeconds(waitTime); //�����ϰ� 3�� ��ٸ���.
        float priviousSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(waitTime2);
        speed = priviousSpeed;

        //yield return null ;  ���������ӱ��� ��ٸ��� 
    }
    protected override void OnMoveUpdate()
    {

        timeElapsed += Time.deltaTime * frequency; //���� �Լ����� ����� �Ķ���� ���

        transform.position = new Vector3(transform.position.x - Time.deltaTime * speed, // x = ������ġ���� ����

            spawnY + Mathf.Sin(timeElapsed) * amplitude, transform.position.z); //spawnY �� �������� sin�Լ� �̿��ؼ� ���� ����

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
