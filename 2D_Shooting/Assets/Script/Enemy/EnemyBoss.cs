using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    [Header("Boss Data")]
    public Vector2 areaMin = new Vector2(2, -3); //Ȱ������  min~ max
    public Vector2 areaMax = new Vector2(7, 3);

    public GameObject bulletPrefab; // �Ϲ��Ѿ�
    public GameObject missilePrefab;// �����̻���
    public float bulletInterval = 1.0f;
    Transform firePosition1;
    Transform firePosition2;
    Transform firePosition3;
    Vector3 targetPos;
    Vector3 moveDirection;

    float currentSpeed = 0.0f; //�����̵��ӵ�

    protected override void Awake()
    {
        base.Awake();
        firePosition1 = transform.GetChild(0);
        firePosition2 = transform.GetChild(1);
        firePosition3 = transform.GetChild(2);
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        Vector3 newPos = transform.position;
        newPos.y = 0.0f;
        transform.position = newPos; // y��ġ 0 

        StopAllCoroutines(); //�ڽ��� �ڷ�ƾ ��� ����
        StartCoroutine(AppearProcess());
    }

    IEnumerator AppearProcess()
    {
        currentSpeed = 0.0f; //����ӵ��� 0���� ����� 
       
        float remainDistance = 5; //���� �Ÿ� 5

        while (remainDistance > 0.01f) // �����ִ� �Ÿ��� ���� 0�� �� �� ���� �ݺ�
        {
            // 0���� remainDistance���� ���� �� Time.deltaTime * 0.5f��ŭ ����� ��ġ
            float deltaMove = Mathf.Lerp(0, remainDistance, Time.deltaTime * 1.2f);
            remainDistance -= deltaMove; // deltamove��ŭ �����Ÿ� ���� 
            transform.Translate(deltaMove * (-Vector3.right)); //deltamove��ŭ �������� �̵�
                       
            yield return null;
        }
        currentSpeed = speed; //oldspeed�� ������� �ʴ� ���� :  stop �� ������ �̹� speed�� �̹� 0�̱� ������ oldSpeed�� �ǹ̰� ����
        SetNextTargetPos();
        StartCoroutine(BulletFire());
    }
    protected override void OnMoveUpdate()
    {
        transform.Translate(Time.deltaTime * currentSpeed * moveDirection);
        //if (targetPos == transform.position) //�־��� �ڵ� float �̱� �빮�� ������ �Ǵ��ϱ� ����

        //if ((targetPos - transform.position).sqrMagnitude < 0.0001f) //�Ʒ� if�� �ڵ尡 ���귮�� �ξ� ����
        //{
        //    //Ÿ�������� ���� ���������
        //    SetNextTargetPos();
        //}
        if ((targetPos - transform.position).sqrMagnitude < 0.01f)
        {
            SetNextTargetPos();
            StartCoroutine(MissileFire());
        }
        //if (transform.position.y > areaMax.y) // �� �ڵ�� vector�� ���� ��� ����ؾ������� �Ʒ� if���� Y���� ����� �ϱ� ������ ���귮�� �ξ� ����
        //{
        //    SetNextTargetPos();
        //    StartCoroutine(MissileFire());
        //}
        //else if (transform.position.y < areaMin.y)
        //{
        //    SetNextTargetPos();
        //    StartCoroutine(MissileFire());
        //}
       
    }

    void SetNextTargetPos()//targetPos ����
    {
        float x;
        float y;

        x = Random.Range(areaMin.x, areaMax.x);
        if (transform.position.y >= 0)
        {
            y = areaMin.y;
        }
        else
        {
            y = areaMax.y;
        }
       
        targetPos = new Vector3(x, y);
        moveDirection = (targetPos - transform.position).normalized;
       // �ӵ��� �����ϰ� �ϱ� ���� ���⸸ ���ܳ��� ���̸� 1�� ���ܳ��´�. ����ȭ
    }

    IEnumerator BulletFire()
    {
        while (true)
        {
            //Instantiate(bulletPrefab, firePosition1.position, Quaternion.identity);
            //Instantiate(bulletPrefab, firePosition2.position, Quaternion.identity);
             Factory.Inst.GetObject(Pool_Object_Type.Enemy_BossBullet,firePosition1.transform.position);
             Factory.Inst.GetObject(Pool_Object_Type.Enemy_BossBullet,firePosition2.transform.position);
             yield return new WaitForSeconds(bulletInterval);
        }
       
    }
    IEnumerator MissileFire()
    {
        for (int i = 0; i < 3; i++)
        {
            Factory.Inst.GetObject(Pool_Object_Type.Enemy_BossMissile,firePosition3.position);
 
            yield return new WaitForSeconds(0.2f);
        }
    }
}
