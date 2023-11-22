using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 카메라에 박아둘 컴포넌트
/// 카메라 가 특정 타겟을 향해 직선으로 이동하는로직과 
/// 카메라 특정 타겟을 향해 바라보는 로직 만 담아둘 컴포넌트 
/// </summary>
public class CameraBase : MonoBehaviour
{
#if UNITY_EDITOR
    /// <summary>
    /// 에디터상에서 실행도중 디버그하기위한 설정 빌드에 포함안되니 이런식으로 해도괸찮을거같다.
    /// </summary>
    [SerializeField]
    bool DEBUGCHECK = false;
#endif

    /// <summary>
    /// 카메라 이동속도 
    /// </summary>
    [SerializeField]
    float cameraMoveSpeed = 1.0f;

    /// <summary>
    /// 카메라 회전속도
    /// </summary>
    [SerializeField]
    float cameraRotateSpeed = 1.0f;

    /// <summary>
    /// 카메라가 이동할 위치를 적용할 오브젝트 트랜스폼
    /// </summary>
    Transform followTarget;
    public Transform FollowTarget 
    {
        get => followTarget;
        set 
        {
            if (followTarget != value) // 값이 바뀌면 
            {
                followTarget = value; //수정하고  이동로직을 실행한다

                StopCoroutine(cameraMoving); //기존 이동 멈추고

                cameraMoving = StandardMove(); //새롭게 로직담고

                StartCoroutine(cameraMoving); // 재실행

            }
        }
    }

    /// <summary>
    /// 카메라가 바라볼 대상이 있는경우 설정 
    /// </summary>
    Transform lookTarget;
    public Transform LookTarget 
    {
        get => lookTarget;
        set 
        {
            if (lookTarget != value) 
            {
                lookTarget = value;

                StopCoroutine(moveRotation); //기존 회전 멈추고

                moveRotation = MoveRotation(); //새롭게 로직담고

                StartCoroutine(moveRotation); // 재실행
            }
        }
    }

    /// <summary>
    /// 이동 코루틴 담아둘 변수
    /// </summary>
    public IEnumerator cameraMoving;

    /// <summary>
    /// 이동 도중 카메라가 바라보는 타겟이있을때 바라볼수있는 로직을 담아둘 코루틴
    /// </summary>
    public IEnumerator moveRotation;

    /// <summary>
    /// 이동이 없을경우 타겟만 바라보게 할수있는 로직
    /// </summary>
    public IEnumerator idleRotation;


    private void Awake()
    {
        cameraMoving = StandardMove();
        moveRotation = MoveRotation();
    }

    /// <summary>
    /// standardTarget 위치로 이동하는 로직
    /// 이동을할때 타겟이 존재하면 바라봐야한다.
    /// </summary>
    /// <param name="timeElaspad">tlrk</param>
    IEnumerator StandardMove(float timeElaspad = 0.0f)
    {
        if (lookTarget != null) //바라볼 대상이 있으면 
        {
            StopCoroutine(moveRotation); //기존 바라보던거 멈추고 
            moveRotation = MoveRotation(timeElaspad); //새로 바라보는 로직담고
            StartCoroutine(moveRotation); //바라보는 로직 실행 
        }
        while ((followTarget.position - transform.position).magnitude > 0.04) //이동이끝날때까지 
        {
            timeElaspad += cameraMoveSpeed * Time.deltaTime;// 시간누적용 
            transform.position = Vector3.Lerp(transform.position, followTarget.position, timeElaspad); //조금씩 움직인다.
#if UNITY_EDITOR
            if (DEBUGCHECK) 
            {
                Debug.Log($"CameraBase.cs파일의 StandardMove 코루틴이 실행중입니다. 위치 :{transform.position}");
            }
#endif
            yield return null;
        }
    }

    /// <summary>
    /// 이동 도중에 계속 바라보는 로직 
    /// 문제 1. 이동할때 바라보는 타겟이 바뀌면 순간이동 계속 바라본다 맨처음이 문제.
    ///         그렇다고 Slerp 넣기에는 회전속도와 이동속도가 안맞으면 이동이끝나도 바라보는 방향이 정상적이지 않을수가있다.
    /// <param name="timeElaspad"></param>
    /// </summary>
    IEnumerator MoveRotation(float timeElaspad = 0.0f)
    {
        while ((followTarget.position - transform.position).magnitude > 0.04) //이동이 끝날때까지 
        {
            timeElaspad += cameraMoveSpeed + Time.deltaTime;  //이동속도와 동기화  델타타임의 차이만큼 오차가 발생한다.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position), timeElaspad); //계속 바라봐라
#if UNITY_EDITOR
            if (DEBUGCHECK)
            {
                Debug.Log($"CameraBase.cs파일의 MoveRotation 코루틴이 실행중입니다. 회전 :{transform.rotation}");
            }
#endif
            yield return null;
        }
        transform.rotation = Quaternion.LookRotation(lookTarget.position - transform.position); //반복문에서는 정확하게 위치를못잡으니 마지막에 교정시킨다.
    }

    /// <summary>
    /// 이동 도중에 계속 바라보는 로직 
    /// <param name="timeElaspad"></param>
    /// </summary>
    IEnumerator IdleRotation(float timeElaspad = 0.0f)
    {
        while (timeElaspad > 1.0f) //회전이 끝날때까지 돌린다. 
        {
            timeElaspad += Time.deltaTime * cameraRotateSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position),timeElaspad); //계속 바라봐라
#if UNITY_EDITOR
            if (DEBUGCHECK)
            {
                Debug.Log($"CameraBase.cs파일의 MoveRotation 코루틴이 실행중입니다. 회전 :{transform.rotation}");
            }
#endif
            yield return null;
        }
        transform.rotation = Quaternion.LookRotation(lookTarget.position - transform.position); //반복문에서는 정확한처리가안되니 마지믹에 한번더 수정
    }

}
