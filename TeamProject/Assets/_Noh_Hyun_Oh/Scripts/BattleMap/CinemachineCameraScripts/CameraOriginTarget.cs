using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///  시네머신 버츄얼 카메라가 타겟으로 잡을 오브젝트 
///  키입력시 화면 회전할때 사용한다.
/// </summary>
public class CameraOriginTarget : MonoBehaviour
{

    /// <summary>
    /// 회전속도
    /// </summary>
    [SerializeField]
    float rotateSpeed = 100.0f;

    /// <summary>
    /// 키입력 막기용
    /// </summary>
    bool isRotate = false;

    /// <summary>
    ///  테스트용  해당씬에서만 쓸지 아님 다른곳에서도 쓸지 결정한뒤 접근방법을 바꿀예정
    /// </summary>
    [SerializeField]
    Transform target;
    public Transform Target 
    {
        get => target;
        set => target = value;
    }

    /// <summary>
    /// 화면사이즈(픽셀단위)의 중간 위치값 가져오기 
    /// </summary>
    Vector3 screenHalfPosition = Vector3.zero;

    float rotateValue = 0.0f;
    float RotateValue 
    {
        get => rotateValue;
        set 
        {
            if (rotateValue != value) 
            {
                rotateValue = value > 359.0f ? 0.0f : value < 1.0f ? 0.0f : value; //값이 0~ 360 까지만 셋팅된다.
                onCameraRotateValue?.Invoke(rotateValue);
            } 
        }
    }

    /// <summary>
    /// 회전값을 전달할 델리게이트
    /// </summary>
    public Action<float> onCameraRotateValue;

    /// <summary>
    /// 카메라 이동속도
    /// </summary>
    [SerializeField]
    float followSpeed = 3.0f;

    /// <summary>
    /// wasd 로 움직일때 회전방향을 전달해줄 델리게이트
    /// </summary>
    public Action<Quaternion> cameraRotation;

    private void Awake()
    {
        screenHalfPosition.x = Screen.width * 0.5f;
        screenHalfPosition.z = 0.0f;
        screenHalfPosition.y = Screen.height * 0.5f;

    }
    private void Start()
    {
        InputSystemController.Instance.OnCamera_LeftRotate = OnLeftRotate;
        InputSystemController.Instance.OnCamera_RightRotate = OnRightRotate;
    }
    private void LateUpdate()
    {
        ///문제가 있을거같지만 일단 동작은 잘하네..?
        transform.position = Vector3.Lerp(transform.position,target.position, followSpeed * Time.deltaTime); // 시작위치가 항상바뀜으로 시간누적 뺏다.
        //transform.Translate(target.transform.position ,Space.World);
    }

    private void OnLeftRotate()
    {
        if (!isRotate)
        {
            StartCoroutine(RotateCourutine(-90.0f));

        }
    }

    private void OnRightRotate()
    {
        if (!isRotate)
        {
            StartCoroutine(RotateCourutine(90.0f));

        }
    }
    /// <summary>
    /// 회전방향에따라 천천히 회전시키기
    /// </summary>
    /// <param name="rotateValue">회전 방향및 각도(90,-90)</param>
    /// <returns></returns>
    IEnumerator RotateCourutine(float rotateValue) 
    {
        RotateValue += rotateValue;// 순순히 처음회전값이 0도 로 가정한상태로 얼마나돌아갔는지 체크하기위한 변수 
        isRotate = true;//회전끝날때까지 입력들어와도 막는용
        //Debug.Log(transform.rotation.eulerAngles.y);
        float time = transform.rotation.eulerAngles.y; //시작값 셋팅
        rotateValue += time; //도착값 셋팅
        if (rotateValue > time)//-값 + 값  왼쪽 오른쪽 체크
        {
            while (time < rotateValue)//체킹
            {
                time += Time.deltaTime * rotateSpeed;
                transform.rotation = Quaternion.Euler(0, time, 0);
                cameraRotation?.Invoke(transform.rotation);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(0, rotateValue, 0);
            cameraRotation?.Invoke(transform.rotation);
            isRotate = false;//회전끝난것을 체크
        }
        else 
        {
            while (time > rotateValue)//체킹
            {
                time -= Time.deltaTime * rotateSpeed;
                transform.rotation = Quaternion.Euler(0, time, 0);
                cameraRotation?.Invoke(transform.rotation);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(0, rotateValue, 0);
            cameraRotation?.Invoke(transform.rotation);
            isRotate = false;//회전끝난것을 체크

        }


    }

}

