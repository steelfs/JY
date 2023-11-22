using EnumList;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// UI 에서 사용될 캐릭터 따라다닐 카메라 
/// Texture 적용할 카메라
/// </summary>
public class UICamera : MonoBehaviour ,ICameraBase
{

    /// <summary>
    /// 딱히안쓴다
    /// </summary>
    EnumList.CameraFollowType cameraTarget = EnumList.CameraFollowType.Custom;
    public CameraFollowType TargetType 
    {
        get => cameraTarget; 
        set => cameraTarget =value; 
    }
    /// <summary>
    /// 여기선 안쓴다.
    /// </summary>
    public Vector3 Distance { get; set; }

    /// <summary>
    /// 여기선 안써
    /// </summary>
    public float FollowSpeed { get; set; }




    /// <summary>
    /// 캐릭터 추적 카메라
    /// </summary>
    Camera actionCam = null;
    public Camera FollowCamera => actionCam;
   

    [SerializeField]
    /// <summary>
    /// 추적당할 오브젝트
    /// </summary>
    private Transform target = null;
    public Transform TargetObject 
    {
        get => target; 
        set => target = value; 
    }

    /// <summary>
    /// 카메라와 목표간의 간격
    ///  - 1 캐릭터 얼굴보기  1 캐릭터 뒤통수 보기
    /// </summary>
    [SerializeField]
    [Range(-1.0f,1.0f)]
    float distance = 1.0f;

    /// <summary>
    /// 이카메라는 EtcObjects 최상위 게임오브젝트안에서 큐로 관리함으로 큐로 돌릴 델리게이트 
    /// </summary>
    public Action resetData;
   
    private void Awake()
    {
        //gameObject.layer = LayerMask.NameToLayer("UI");///설정한 레이어 검색해서 번호 가져오기
        //gameObject.tag = "Respawn"; //태그 변경시키기
        actionCam = GetComponent<Camera>();
        actionCam.targetTexture = new RenderTexture(512,512,16,RenderTextureFormat.ARGB32);
    }


    IEnumerator MoveCamera()
    {
        while (true)
        {
            TrackingValueSetting();
            yield return null;

        }
    }
    
    private void OnEnable()
    {
        if (target != null) 
        {
            StartCoroutine(MoveCamera());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        resetData?.Invoke(); //큐로돌린다.

    }

    /// <summary>
    /// 카메라가 특정 목표를 같은 x,y 선상의 z 값기준으로 바라보게만든다.
    /// </summary>
    private void TrackingValueSetting() 
    {
        transform.position = target.transform.position  // 목표 지점에서 
                            -(target.transform.forward * distance); // 목표의 정면방향에  distance 를 더한뒤  - 를 해서 바라보는방향 반대쪽으로 위치 시킨다.

        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up); //방향백터로 바라보게 만들기 y축기준으로 돌린다.
    }

}
