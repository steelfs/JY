using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR //만약 에디터에서 실행중이라면 이걸 쓰겠다. 즉 빌드시에는 포함시키지 않겠다.//#define
using UnityEditor;
#endif
public class TurretChase : TurretBase
{
    //플레이어의 참조방법 transform(참조타입)? or vector3(값타입 플레이어의 위치가 바뀌때마다 다시 처리해줘야함)
    Transform target;//플레이어
    SphereCollider sightTrigger;

    public float sightRange = 10.0f;
    public float fireAngle = 10.0f;
    public float turnSpeed = 2.0f;
    bool IsFiring = false;


    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>(); //시야 감지용 트리거
    }
    private void Start()
    {
        sightTrigger.radius = sightRange;
    }
    private void Update()
    {
        LookTargetAndAttack();//대상이 있는지 확인해서 있으면 타겟으로 회전후 공격
    }
    void LookTargetAndAttack() //대상을 바라보고 범위안에 들어오면 공격하는 함수
    {
        if (target != null)//타겟이 있을때만 
        {
            Vector3 dir = target.position - barrelBodyTransform.position;
            dir.y = 0;
           
            if (IsVisibleTarget(dir)) //타겟이 보일때만
            {
                barrelBodyTransform.rotation = Quaternion.Slerp(barrelBodyTransform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
                //Quaternion.LookRotation(dir);// 특정 (방향)을 바라보게하는 함수

                //barrelBodyTransform.LookAt(target);//트렌스폼이 특정(지점)을 바라보게 만드는 함수

                //각도 구하기 Angle, 파라미터 = Vector3 from, to  사이각 중 작은 것을 리턴해주는 함수
                //SignedAngle// 두 벡터의 사이각 중 축이되는 벡터를 기준으로 계산
                float angle = Vector3.Angle(barrelBodyTransform.forward, dir);
                if (angle < fireAngle && IsVisibleTarget(dir)) // 타겟 사이에 장애물이 없어 Ray로 스캔이 되면
                {
                    StartFire(); //발사각 안이면서  보이면
                }
                else
                {
                    StopFire();
                }
            }
            else
            {
                StopFire();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform; //플레이어가 들어오면 타겟으로 지정
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;// 플레이어가 나가면 null;
            StopFire();
        }
    }

    bool IsVisibleTarget(Vector3 lookDir)
    {//ray 
        bool result = false;
        if (target != null)
        {
            Ray ray = new(barrelBodyTransform.position, lookDir);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, sightRange))
            {
                if (hitInfo.transform == target)
                {
                    result = true;
                }
                // 기즈모 거리 입력 스팟 sightRange수정
                //sightRange를 가장먼저 Ray가 스캔된 위치로 설정해야함
                sightRange = (barrelBodyTransform.position - hitInfo.transform.position).sqrMagnitude;
            } 
        }
        return result;
    }

    void StartFire()
    {
        if (IsFiring)
        {
            StartCoroutine(fireCoroutine);
            IsFiring = false;
        }
    }
    void StopFire()
    {
        if (!IsFiring)
        {
            StopCoroutine(fireCoroutine);        
            IsFiring = true;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, sightRange,2);//빌드할때 빼줘야함

        if (barrelBodyTransform == null)
        {
            barrelBodyTransform = transform.GetChild(4);
        }
        Gizmos.DrawLine(barrelBodyTransform.position, barrelBodyTransform.position + barrelBodyTransform.forward * sightRange);

        Vector3 from = barrelBodyTransform.position;
        Vector3 to = barrelBodyTransform.position + barrelBodyTransform.forward * sightRange;
        Gizmos.color = IsFiring? Color.red : Color.green; // true면 왼쪽 
        Gizmos.DrawLine(from, to);

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, barrelBodyTransform.up) * barrelBodyTransform.forward;
        //drawLine = 2D에서 vector3를 하드코딩으로 넣어준 것과 형태만 다를 뿐이다. 굳이 어렵게 생각할 필요는 없다.
        //발사각
        Gizmos.color = Color.white;
        to = barrelBodyTransform.position + dir1 * sightRange;
        Gizmos.DrawLine(from, to);

        to = barrelBodyTransform.position + dir2 * sightRange;
        Gizmos.DrawLine(from, to);
    }
#endif

    // Line 위치 2개
    // Ray  위치, 방향
}
