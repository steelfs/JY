using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : TestBase
{
    //플레이어를 천천히 따라가는 느낌의 카메라
    // Slerp이용 플레이어를 따라가기
    //따라다니는 대상 필요
    //카메라의 최종위치는 처음시작할때 플레이어와의 거리를 유지해야한다.
    //앞뒤 먼저 구현 후  회전
    float length;
    public Transform target;
    public float speed = 3.0f;
    Vector3 offset;
    private void Start()
    {
        if (target == null)
        {
            Player player = FindObjectOfType<Player>();     //찾는방법 Find Tag, Type(딱 한개만 있다는 전재 하에 Type)
            target = player.transform;
        }
        offset = transform.position - target.position;// 타겟 위치에서 카메라 이 오브젝트로 오는 방향벡터;
        length= offset.magnitude;// 거리 구하기
    }
    private void FixedUpdate()
    {//offset만큼 움직인 위치로 설정
        transform.position = Vector3.Slerp(transform.position, target.position + Quaternion.LookRotation(target.forward) * offset, Time.fixedDeltaTime * speed);
        transform.LookAt(target);// 회전 빈오브젝트 생성후 인스펙터에서 할당 조정


        // 타겟(플레이어)에서 카메라로 가는 레이
        Ray ray = new Ray(target.position, transform.position - target.position);// 레이슬 계속 쏴서 플레이어와 카메라 사이 뭔가 들어오면 레이가 충돌한 지점으로 카메라 위치를 변경하기
        if (Physics.Raycast(ray, out RaycastHit hitInfo, length))
        {
            transform.position = hitInfo.point;
        }
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
       
    }
}
