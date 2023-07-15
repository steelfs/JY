using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyAsteroid : EnemyBase
{
    //��� �����Ǹ� destination ������ �����ǰ� �� �������� �̵��Ѵ�.
    // ��� �׻� �ݽð�������� ȸ���Ѵ�.(ȸ�� �ӵ��� ����)
    //������ ���� ������ �׸���

    //������
    [Header("Asteroid data")]
    Vector3? destination = null; 
    Vector3 direction;
    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 4.0f;

    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;

    public float rotateSpeed;

    [Range(0f, 1f)] // �ν�����â���� ���� �� ���� ����
    public float criticalRate = 0.95f; //Ȯ���� ���������� �̴Ͽ�� ������

    public float minLifeTime = 4.0f;// ��� ����
    public float maxLifeTime = 7.0f;

    
   //������ ���� ������Ƽ
    public Vector3? Destination
    {
        get => destination;
        set
        {
            if (destination == null) //destination null �� ���� ���õȴ�. (�ѹ��� ���õȴ�.)
            {
                destination = value;
                direction = (destination.Value - transform.position).normalized; //����ũ�⸦ 1�� �������ش�.
  
            }
        }
    }

    protected override void OnMoveUpdate()
    {
        if (destination != null)
        {
            //���� = ũ��� �������� �̷���� �ִ�. ������ ũ�⿡ 1�� �����ָ� ������͸� ���´�
            //�Ʒ��ڵ�� ������Ʈ���� ��� ����Ǳ� ������ �����Ӹ��� ��� ����� ������ϱ� ������ ���� ���� �ڵ��̴�.
            transform.Translate(Time.deltaTime * speed * direction, Space.World);
        }
        transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.forward);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();

        speed = Random.Range(minMoveSpeed,maxMoveSpeed);
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed) ;
        StartCoroutine(SelfCrush());
       // Hp = 3;
    }
    //protected override void OnCollisionEnter2D(Collision2D collision)
    //{
    //    base.OnCollisionEnter2D(collision);
    //}
    //�Ѿ��� �°� �״±���߰�.


    protected override void Die()
    {
        //���� �� ������ ȸ����, ������ ���ư��� �̴� ������ ���� 95%Ȯ����(3�� ~7��) 5%Ȯ���� 20��  ����
        //�̴Ͽ�� �׻� ���� ���̰��� ������.
        //���ư��� ������ ����

        // 1. ��� �����Ѵ�.  2. ������ ������ �������� �ٲ۴�.  3. �����̼��� ����� ȸ������ �ٲ� ��������� �ٲ۴�. 4. ȸ���� / ������ Ƚ��   �� ������ ������ ���̰��� ���Ѵ�.
            
       
        int count;

        if (Random.value < criticalRate) //ũ��Ƽ���� ������ 20�� ����
        {
            count = 20;
        }
        else
        {
            count = Random.Range(3, 8);//�Ϲ��� ��Ȳ 
         
        }


        float angle = 360.0f / count;  //���̰� ���ϱ�
        float startAngle = Random.Range(0, 360f); //���۰� ���ϱ�
        for (int i = 0; i < count; i++)
        {
            
            Factory.Inst.GetAsteroidMini(transform.position,startAngle + angle * i);
            //GameObject obj = Factory.Inst.GetObject(Pool_Object_Type.Enemy_Asteroid_Mini);
            //obj.transform.position = transform.position; // ��ġ �ű��
            //obj.transform.Rotate((startAngle * angle * i) * Vector3.forward);
        }       
        base.Die();
    }

    IEnumerator SelfCrush()
    {
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        yield return new WaitForSeconds(lifeTime);
        Die();
    }
    public void Test_Die()
    {
        Die();
    }
}
