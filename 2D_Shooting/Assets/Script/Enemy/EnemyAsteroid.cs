using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyAsteroid : EnemyBase
{
    //운석은 생성되면 destination 방향이 지정되고 그 방향으로 이동한다.
    // 운석은 항상 반시계방향으로 회전한다.(회전 속도는 랜덤)
    //목적지 스팟 기즈모로 그리기

    //목적지
    [Header("Asteroid data")]
    Vector3? destination = null; 
    Vector3 direction;
    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 4.0f;

    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;

    public float rotateSpeed;

    [Range(0f, 1f)] // 인스펙터창에서 범위 내 설정 가능
    public float criticalRate = 0.95f; //확률로 폭발적으로 미니운석이 생성됨

    public float minLifeTime = 4.0f;// 운석의 수명
    public float maxLifeTime = 7.0f;

    
   //목적지 설정 프로퍼티
    public Vector3? Destination
    {
        get => destination;
        set
        {
            if (destination == null) //destination null 일 때만 세팅된다. (한번만 세팅된다.)
            {
                destination = value;
                direction = (destination.Value - transform.position).normalized; //백터크기를 1로 세팅해준다.
  
            }
        }
    }

    protected override void OnMoveUpdate()
    {
        if (destination != null)
        {
            //백터 = 크기와 방향으로 이루어져 있다. 백터의 크기에 1을 곱해주면 방향백터만 남는다
            //아래코드는 업데이트에서 계속 실행되기 때문에 프레임마다 계속 계산을 해줘야하기 때문에 좋지 않은 코드이다.
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
    //총알을 맞고 죽는기능추가.


    protected override void Die()
    {
        //죽을 때 랜덤한 회전과, 각도로 날아가는 미니 랜덤한 갯수 95%확률로(3개 ~7개) 5%확률로 20개  생성
        //미니운석은 항상 같은 사이각을 가진다.
        //날아가는 방향은 랜덤

        // 1. 운석을 생성한다.  2. 생성될 갯수를 랜덤으로 바꾼다.  3. 로테이션을 사용해 회전각을 바꿔 진행방향을 바꾼다. 4. 회전각 / 생성된 횟수   를 나눠서 일정한 사이각을 정한다.
            
       
        int count;

        if (Random.value < criticalRate) //크리티컬이 터지면 20개 생성
        {
            count = 20;
        }
        else
        {
            count = Random.Range(3, 8);//일반적 상황 
         
        }


        float angle = 360.0f / count;  //사이각 구하기
        float startAngle = Random.Range(0, 360f); //시작각 구하기
        for (int i = 0; i < count; i++)
        {
            
            Factory.Inst.GetAsteroidMini(transform.position,startAngle + angle * i);
            //GameObject obj = Factory.Inst.GetObject(Pool_Object_Type.Enemy_Asteroid_Mini);
            //obj.transform.position = transform.position; // 위치 옮기기
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
