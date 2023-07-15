using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WayPointUser
{
    Transform bladeMesh;
    public float rotateSpeed = 720.0f;
    protected override Transform Target
    {
        //get => base.Target;
        set
        {
            base.Target = value;
            transform.LookAt(Target);
        }
    }

    private void Awake()
    {
        bladeMesh = transform.GetChild(0);
    }
    private void Update()
    {
        bladeMesh.Rotate(Time.deltaTime * rotateSpeed * Vector3.right);
    }
    //1.이동방향을 바라봐야한다.
    //날이 회전해야한다.
    //플레이어 피격시 사망
    //protected override void OnArrived()
    //{
    //    base.OnArrived();
    //    //targetWayPoints.currentWayPoint 를 vector3로 바꿔서 LookRotation에 파라미터로 넘겨야하는데 방법이 떠오르질 않는다
    //    //targetWayPoints.currentWayPoint.position 으로 대입을 했는데 회전 각도가 맞지 않는다. 
    //    //주의할 점은 LookRotation의 파라미터로는 타겟의 포지션을 넘기는게 아니라 타겟을향한"방향"을 넘겨줘야한다.

    //    //transform,rotation 에는 쿼터니언 타입을 넣어줘야하기 때문에 위에 쿼터니언 타입의 변수를 새로 만들어 줘야한다.
    //    Vector3 angle = targetWayPoints.currentWayPoint.position - transform.position;
    //    Quaternion rotation = Quaternion.LookRotation(angle, Vector3.up);
    //    transform.rotation = rotation;//생각의 순서는 여기부터 순차적으로 위로 올라간다
    //}
    private void OnCollisionEnter(Collision collision)//불필요한 코드실행을 방지하기 위해 추가 레이어설정 예) 트랩과 그라운드 충돌시 에도 이코드가 실행됨
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Die();
        }

    }
}
