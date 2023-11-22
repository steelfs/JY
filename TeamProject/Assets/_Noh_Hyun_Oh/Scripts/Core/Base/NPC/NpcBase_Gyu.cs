using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NpcBase_Gyu : MonoBehaviour
{
    
    /// <summary>
    /// 엔피씨의 관리할 인덱스
    /// </summary>
    int npcId;
    public int Npcid => npcId;

   
    /// <summary>
    /// 엔피씨의 현재 대화종류
    /// </summary>
    protected TalkType talkType;
    public TalkType TalkType => talkType;
    
    /// <summary>
    /// 대화 진행도 
    /// </summary>
    protected int talkDepth = 0;
    public int TalkDepth => talkDepth;
   
    /// <summary>
    /// 이동로직 찾아두기 연결용 .
    /// </summary>
    [SerializeField]
    NPCMove moveProccess;
    public NPCMove MoveProccess => moveProccess;


    /// <summary>
    /// 근처에와서 대화가능하도록 실행시키는 델리게이트
    /// </summary>
    public Action<int> onTalkEnableButton;

    /// <summary>
    /// 주변을 벗어나면 신호보낼 델리게이트
    /// </summary>
    public Action onTalkDisableButton;

    [SerializeField]
    /// <summary>
    /// NPC 얼굴을 찍을 카메라 
    /// </summary>
    Camera npcCharcterCamera;

    /// <summary>
    /// UI rawImage 에 연결할 렌더러 텍스쳐 
    /// </summary>
    public RenderTexture GetTexture => npcCharcterCamera.targetTexture;
    protected virtual void Awake()
    {
        moveProccess = transform.parent.GetComponentInChildren<NPCMove>(true);
        npcCharcterCamera = transform.GetComponentInChildren<Camera>();
        npcCharcterCamera.targetTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
        npcCharcterCamera.targetTexture.name = $"{name}_의 텍스쳐";

        ///카메라 룩 위치 찾기 
        Transform lookTarget = FindObjectOfType<Cho_PlayerMove>(true).transform;

        moveProccess.getTarget = () => {
          
            return lookTarget;
        };
    }

    /// <summary>
    /// 엔피씨 초기화용 함수 
    /// </summary>
    /// <param name="npcIndex">엔피씨 번호부여할 값</param>
    public virtual void InitData(int npcIndex) 
    {
        npcId = npcIndex;
    }

    // 퀘스트 리스트중에 어떤값을 가져올지 가져오는 기능도 필요 
    // 특정 진행상황에서만 할수있는 퀘스트 라던가 
    // 완료된 퀘스트 따로 저장한다던가 
    // 리스트에는 있는데 더이상 진행할수없는 퀘스트 라던가  등등 생각날때마다 적자


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("들어와");
            onTalkEnableButton?.Invoke(Npcid);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("나가");
            onTalkDisableButton?.Invoke();
        }
    }
}
