using EnumList;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 1. 카메라 사용할 오브젝트들의 이넘만들기 (변수설정만완료 내용추가 필요)
/// 2. 이넘을기준으로 각이넘별 행동연결 ??
///  2-1. 카메라는  
///       - 줌 확대 기능,  v
///       - 일정거리유지하며 따라가기기능 ,  v
///       - 오브젝트 회전방향에맞춰 회전해주는기능 v
///      
/// UI 에서 사용될 캐릭터 따라다닐 카메라 v
/// Texture 적용할 카메라 v
/// </summary>
public class CameraManager : TestBase 
{
    /// <summary>
    /// 컨트롤할 카메라
    /// </summary>
    public Camera actionCam = null;

    CameraBase camBase = null;

    [Header("카메라가 보여줄 타입")]
    /// <summary>
    /// 카메라 를 사용할 오브젝트 종류
    /// </summary>
    [SerializeField]
    protected EnumList.CameraFollowType cameraFollowType = EnumList.CameraFollowType.Custom;

    /// <summary>
    /// 카메라 타입이 바뀔경우 내부 기본값 셋팅 하기위한 프로퍼티
    /// </summary>
    public virtual CameraFollowType CameraFollowType 
    {
        get => cameraFollowType;
        protected set 
        {
            ResetData();
            switch (value)
            {
                case CameraFollowType.Custom:
                    cameraMoveCoroutine = CustomCamera();
                    break;
                case CameraFollowType.MiniMap:
                    CustomCameraPos.y = 50.0f;
                    CustomCameraRotate = miniMapRotate;
                    actionCam.targetTexture = cameraTexture;
                    textureImage.texture = cameraTexture;
                    cameraMoveCoroutine = MiniMapCamera();
                    break;
                case CameraFollowType.UITexture:
                    CustomCameraPos.z = 2.0f;
                    actionCam.targetTexture = cameraTexture;
                    textureImage.texture = cameraTexture;
                    cameraMoveCoroutine = UITextureView();
                    break;
                case CameraFollowType.QuarterView:
                    waitForSeconds = new WaitForSeconds(followSpeed);
                    CustomCameraPos = quarterViewPos;
                    cameraMoveCoroutine = QuarterView();
                    break;
                default:
                    break;
            }
            cameraFollowType = value;   
        }
    }

    public Camera FollowCamera => actionCam;
    [Header("카메라가 추적할 객체")]

    [SerializeField]
    /// <summary>
    /// 추적당할 오브젝트 의 트랜스폼
    /// </summary>
    protected Transform target = null;

    [SerializeField]
    /// <summary>
    /// 추적당할 오브젝트가 공격할 객체위치
    /// </summary>
    protected Transform attackTarget = null;
   


    /// <summary>
    /// 여러상황을 하나의 코드로 실행시키기위해 따로뺏다.
    /// </summary>
    protected IEnumerator cameraMoveCoroutine;


    [Header("카메라의 추적 속도")]

    /// <summary>
    /// 카메라가 캐릭터를 따라다니는 속도
    /// </summary>
    [SerializeField]
    [Range(0.0f, 5.0f)]
    float followSpeed = 1.0f;

    /// <summary>
    /// 코루틴에 사용될 기다릴 시간 
    /// </summary>
    protected WaitForSeconds waitForSeconds;

    /// <summary>
    /// 물리연산 끝난뒤 처리하기 위해 필요한 객체 
    /// </summary>
    protected WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
    

    [Header("사용자가 직접입력해서 수정할때 사용할값")]
    /// <summary>
    /// 입력값으로 카메라 회전 수정할때 사용
    /// </summary>
    [SerializeField]
    protected Quaternion CustomCameraRotate = Quaternion.identity;


    /// <summary>
    /// 입력값으로 카메라 위치 수정할때 사용
    /// </summary>
    [SerializeField]
    protected Vector3 CustomCameraPos = Vector3.zero;

    /// <summary>
    /// UI 에 사용할 카메라와 연결될 텍스쳐 
    /// </summary>
    [SerializeField]
    RenderTexture cameraTexture;

    /// <summary>
    /// 텍스쳐가 보일 UI RawImage 
    /// </summary>
    [SerializeField]
    RawImage textureImage;

    /// <summary>
    /// 90도 아래를 바라보는 방향 저장해두기 미니맵용
    /// </summary>
    readonly Quaternion miniMapRotate = Quaternion.AngleAxis(90.0f, Vector3.right);

    /// <summary>
    /// 쿼터뷰 만들 위치값 
    /// </summary>
    readonly Vector3 quarterViewPos =  new Vector3(0.0f, 10.0f, -5.0f);

    /// <summary>
    /// 캐릭터 의 foward 기준  줌인 값 설정
    /// </summary>
    [SerializeField]
    Vector3 zoomInPos = new Vector3(0.0f, 0.0f, -3.0f);

    protected override void Awake()
    {
        base.Awake();
        //gameObject.layer = LayerMask.NameToLayer("UI");///설정한 레이어 검색해서 번호 가져오기
        //gameObject.tag = "Respawn"; //태그 변경시키기
        if (actionCam == null)
        {
            actionCam = GetComponent<Camera>();
            camBase = actionCam.GetComponent<CameraBase>();
        }
        else 
        {
            camBase = actionCam.GetComponent<CameraBase>();
        }

        cameraTexture = new(256 ,256,16,RenderTextureFormat.ARGB32); //텍스쳐 기본값 으로 만들기 
        cameraTexture.name = $"{gameObject.name}_Texture";           //이름이없어서 햇갈려서 일단 넣어놨다.
        cameraMoveCoroutine = CustomCamera();
    }

    /// <summary>
    /// 설정한 맴버변수 초기화 하는함수 
    /// </summary>
    protected virtual void ResetData() 
    {
        actionCam.targetTexture = null;
        CustomCameraPos = Vector3.zero;
        CustomCameraRotate = Quaternion.identity;
        cameraMoveCoroutine = null;
        if(textureImage != null) textureImage.texture = null;
    }

    /// <summary>
    /// 카메라가 타겟의 포지션과 회전을 영향 받으며 따라가는 로직 
    /// </summary>
    /// <returns></returns>
    IEnumerator CustomCamera()
    {
        if (target != null) //타겟이 있을때만 실행
        {
            while (cameraFollowType == CameraFollowType.Custom)
            {
                actionCam.transform.position = target.position + CustomCameraPos;
                actionCam.transform.rotation = target.rotation * CustomCameraRotate;

                yield return fixedWait;
            }
        }

    }

    /// <summary>
    /// 정해진 회전방향을 유지하고 정해진 거리를 유지한채 타겟을 쫒아다니는 로직 
    /// 미니맵용
    /// </summary>
    IEnumerator MiniMapCamera()
    {
        if (target != null)
        {
            while (cameraFollowType == CameraFollowType.MiniMap)
            {
                actionCam.transform.position = CustomCameraPos + target.position;
                actionCam.transform.rotation = CustomCameraRotate;

                yield return fixedWait;
            }
        }


    }



    /// <summary>
    /// UI용 카메라 조절
    /// </summary>
    IEnumerator UITextureView()
    {
        if (target != null)
        {

            while (cameraFollowType == CameraFollowType.UITexture)
            {
                actionCam.transform.position = target.position // 목표 지점에서 
                                    - (target.forward * CustomCameraPos.z); // 바라보는 방향 반대쪽으로 일정거리 떨어진값을 카메라위치로한다.

                actionCam.transform.rotation = Quaternion.LookRotation(target.position - actionCam.transform.position, target.up); //방향백터로 바라보게 만들기 y축기준으로 돌린다.
                yield return fixedWait;
            }
        }
    }

    /// <summary>
    ///  쿼터뷰 형식의 카메라 위치셋팅과 타겟 바라보기
    /// </summary>
    IEnumerator QuarterView()
    {
        if (target != null)
        {

            while (cameraFollowType == CameraFollowType.QuarterView)
            {
                //타겟의 Y 축의 회전값을 가져온다 .
                //기본적으로  타겟이 y축 회전만한다고 보면  y 값은 0으로 고정될것이다.  
                //Quaternion.Euler 함수는 x y z  축 을 기준으로 어느정도 회전했는지에대한 결과값을 반환하는 함수이다.
                //target.eulerAngles 의 값을 사용하면 해당 target 이 어느축으로 어느정도 회전했는지 결과값을 구할수있다 이값은 바로 rotation 값으로 적용이 가능하다
                Quaternion rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);  // y 축으로 타겟이 어느정도 회전했는지에대한 값을 받아온다 
                actionCam.transform.position = target.position + (rotation * CustomCameraPos); //회전방향에 거리를 곱해서 쿼터뷰 처럼보이게 위치를 잡는다

                // 타겟의 방향을 바라보도록 카메라 회전
                actionCam.transform.rotation = Quaternion.LookRotation(target.position - actionCam.transform.position);
                yield return null; // 픽시드 업데이트에서 발동하도록 걸어준다.
            }
        }

    }
    /// <summary>
    /// 줌인 할때 타겟을 정중앙이 아닌 살짝 빗겨나가게 만들기위한 거리값
    /// </summary>
    [SerializeField]
    Vector3 zoomFocusPos = Vector3.zero;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 tempPos = Vector3.zero;

   

   

   

    /// <summary>
    /// 카메라를 특정위치로 이동시킬때 셋팅하는 함수
    /// </summary>
    /// <param name="pos">카메라가 이동될 위치값</param>
    public virtual void SetRotation(Vector3 pos) 
    {
        CustomCameraPos = pos;
    }
    /// <summary>
    /// 카메라를 특정 방향으로 회전시킬때 사용하는 함수 
    /// </summary>
    /// <param name="rotate">회전적용할값</param>
    public virtual void SetPosition(Quaternion rotate) 
    {
        CustomCameraRotate = rotate;
    }

    /// <summary>
    /// 셋팅한로직 실행용 
    /// </summary>
    public virtual void CameraMoveStart() 
    {
        StartCoroutine(cameraMoveCoroutine);
    }

    /// <summary>
    /// 실행중인 기능 멈추기
    /// </summary>
    public virtual void CameraMoveEnd() 
    {
        StopCoroutine(cameraMoveCoroutine);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        CameraFollowType = cameraFollowType;
        StartCoroutine(cameraMoveCoroutine);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        StopCoroutine(cameraMoveCoroutine);
        StartCoroutine(StaticCameraController.ZoomIn(target,actionCam,attackTarget.transform,followSpeed));
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        StopCoroutine (cameraMoveCoroutine);
        StartCoroutine(StaticCameraController.ZoomOut(target, actionCam, quarterViewPos, followSpeed));
    }
    protected override void Test4(InputAction.CallbackContext context)
    {
        camBase.FollowTarget = target;
        camBase.LookTarget  = attackTarget;
    }
}
