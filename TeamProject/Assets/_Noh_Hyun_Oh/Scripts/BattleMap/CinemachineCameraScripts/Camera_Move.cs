using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// 마우스가 화면 가장자리의 어디를 향하는지에대한 설정값
/// </summary>
public enum Screen_Side_Mouse_Direction : byte
{
    None = 0,
    Left = 1,
    Right = 2,
    Top = 4,
    Bottom = 8,
}

/// <summary>
///  배틀맵  시네머신 버츄얼 카메라 이동 클래스
/// </summary>
public class Camera_Move : MonoBehaviour
{
    /// <summary>
    /// 캐릭터 따라다니는 기본카메라
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera originFollowCameraObject;
    /// <summary>
    /// 화면 밖을 마우스 올렸을때 이동할 카메라
    /// </summary>
    [SerializeField]
    CinemachineVirtualCamera moveCam;
    /// <summary>
    /// 화면밖으로 벗어날때 위치값을 잡아줄 브레인 카메라
    /// </summary>
    [SerializeField]
    CinemachineBrain brain;
    public CinemachineBrain Brain 
    {
        get 
        {
            if (brain == null) //호출했을경우 없을땐 
            {
                brain = GetCineBrainCam?.Invoke(); //한번 찾는다 .
            }
            return brain;
        }
    }
    /// <summary>
    /// 브레인 카메라 찾아오는 델리게이트 
    /// 초기화 컴퍼넌트 InitCharcterSetting 에서 연결중
    /// </summary>
    public Func<CinemachineBrain> GetCineBrainCam;
    /// <summary>
    ///  화면이동할때 우선순위 가져오기위해 높은값을 지정
    /// </summary>
    readonly int viewIndex = 1000;
    /// <summary>
    /// 화면 이동끝났을때 우선순위 반환할 낮은값 지정
    /// </summary>
    readonly int closeIndex = 0;

    /// <summary>
    /// 카메라 이동속도 
    /// </summary>
    [SerializeField]
    float moveSpeed = 10.0f;

    /// <summary>
    /// 카메라 이동감지 받아올 델리게이트
    /// </summary>
    public Action<Screen_Side_Mouse_Direction> moveCamera;

    [SerializeField]
    /// <summary>
    /// 이동값 방향 담을 백터
    /// </summary>
    Vector3 tempMoveDir = Vector3.zero;

    /// <summary>
    /// 카메라 회전관리할 오브젝트
    /// </summary>
    Transform cameraOriginObject;

    private void Awake()
    {
        //위에 카메라 가져오는거 여기서 가져오기 
        //나중에 카메라 픽스되면 여기에 찾는로직추가
        moveCamera += OnMove;

        //Follow 한이유는 awake 에서는 Vcam 이 제대로된 회전값이 셋팅이 안되기때문이다

    }
    private void OnEnable()
    {
        OnInitPos();
    }
    public void OnInitPos() 
    {
        moveCam.transform.position = originFollowCameraObject.transform.position; //일단 위치 기본카메라로
        moveCam.transform.rotation = originFollowCameraObject.transform.rotation; //카메라이동시 이상하지않게 시작점잡기
        cameraOriginObject = originFollowCameraObject.Follow;
    }
    /// <summary>
    /// 카메라 이동 관련 값셋팅
    /// </summary>
    /// <param name="dir">마우스가 위치한 스크린 방향</param>
    private void OnMove(Screen_Side_Mouse_Direction dir)
    {
        // 회전기준이되는 카메라의 회전값을 가져온다
        switch (dir)
        {
            case Screen_Side_Mouse_Direction.None:
                tempMoveDir = Vector3.zero;
                moveCam.Priority = closeIndex; //메인카메라로 우선순위넘기기
                break;

            case Screen_Side_Mouse_Direction.Left | Screen_Side_Mouse_Direction.Top:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = -cameraOriginObject.transform.right;
                tempMoveDir += cameraOriginObject.transform.forward;
                break;
            case Screen_Side_Mouse_Direction.Right | Screen_Side_Mouse_Direction.Top:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = cameraOriginObject.transform.right;
                tempMoveDir += cameraOriginObject.transform.forward;
                break;

            case Screen_Side_Mouse_Direction.Left | Screen_Side_Mouse_Direction.Bottom:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = -cameraOriginObject.transform.right;
                tempMoveDir -= cameraOriginObject.transform.forward;
                break;
            case Screen_Side_Mouse_Direction.Right | Screen_Side_Mouse_Direction.Bottom:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = cameraOriginObject.transform.right;
                tempMoveDir -= cameraOriginObject.transform.forward;
                break;

            case Screen_Side_Mouse_Direction.Left:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = -cameraOriginObject.transform.right; 
                break;
            case Screen_Side_Mouse_Direction.Right:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = cameraOriginObject.transform.right;
                break;
            case Screen_Side_Mouse_Direction.Top:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = cameraOriginObject.transform.forward;
                break;
            case Screen_Side_Mouse_Direction.Bottom:
                moveCam.Priority = viewIndex; //우선순위 가져오기
                tempMoveDir = -cameraOriginObject.transform.forward;
                break;
            default:
                break;
        }
        
        moveCam.transform.position = Brain.transform.position;
        //기본적으로 같이 움직이기때문에 화면밖으로 벗어나는것을 방지하기위해 브레인을 따라다니게 셋팅한다.

    }
    //이동및 회전 셋팅
    private void Update()
    {
        moveCam.transform.rotation = originFollowCameraObject.transform.rotation;
        moveCam.transform.position += Time.deltaTime * moveSpeed * tempMoveDir;
    }

}
