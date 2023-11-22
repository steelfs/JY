using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 기타 맵이동시에도 공통으로 사용되로오브젝트
/// </summary>
public class EtcObjects : Singleton<EtcObjects>
{
    /// <summary>
    /// 카메라 관리할 큐
    /// </summary>
    Queue<UICamera> cameraQueue;

    /// <summary>
    /// 캐릭터 상시상태쪽에 보일카메라 3개
    /// </summary>
    UICamera[] teamCharcterView; 
    public UICamera TeamCharcterView => cameraQueue.Dequeue(); //카메라 줄땐 큐에서 꺼내서준다.

    
    protected override void Awake()
    {
        base.Awake();
        teamCharcterView = GetComponentsInChildren<UICamera>(true); //EtcObject 밑에는 항시 3개만 존재 위치바껴도찾기위해 걍 이렇게 찾는다.
        cameraQueue = new Queue<UICamera>(teamCharcterView.Length); //찾은 갯수로 큐만들어두고 
    }
    private void Start()
    {
        TeamBorderManager teamBorderManager = WindowList.Instance.GetComponentInChildren<TeamBorderManager>(true);
        RawImage[] rawImages = teamBorderManager.GetComponentsInChildren<RawImage>(true);
        int i = 0;
        foreach (UICamera camera in teamCharcterView) //돌면서
        {
            rawImages[i].texture = camera.FollowCamera.activeTexture; //택스쳐 집어넣기
            camera.resetData = () =>  //리셋될때 
            {
                cameraQueue.Enqueue(camera);//다시 들어갈수있게 연결해준다.
                //Debug.Log(camera.GetHashCode()); //camera 변수명에 들어있는값이 제대로 순번대로 들어간다 확인완료 
            };
            i++;
            camera.gameObject.SetActive(false);
        }
    }
}
