using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcTalkController : MonoBehaviour
{

    /// <summary>
    /// 엔피씨 대화목록 관리하는 클래스 
    /// </summary>
    TalkData_Gyu talkData;
    public TalkData_Gyu TalkData => talkData;



    /// <summary>
    /// 대화 코루틴 중복체크용 
    /// </summary>
    bool isTalking = false;
    public bool IsTalking 
    {
        get => isTalking;
        set => isTalking = value;
    } 
    /// <summary>
    /// 현재 대화순번 
    /// </summary>
    int currentTalkIndex = 0;




    /// <summary>
    /// 엔피씨 이미지
    /// </summary>
    RawImage npcImg;

    /// <summary>
    /// 엔피씨 이름
    /// </summary>
    TextMeshProUGUI nameBox;

    /// <summary>
    /// 엔피씨 대화목록
    /// </summary>
    TextMeshProUGUI talkBox;

    /// <summary>
    /// 대화종료 버튼
    /// </summary>
    Button talkEndButton;
    public Button TalkEndButton => talkEndButton;

    
    /// <summary>
    /// 다음 대사로 넘기는 버튼
    /// </summary>
    Button nextButton;

    /// <summary>
    /// 로그 토글 버튼 
    /// </summary>
    Button logButton;





    public Action openTalkWindow;
    public Action closeTalkWindow;


    /// <summary>
    /// Func은 데이터연결타이밍 맞추기에는 편하지만 기본적으로 null 체크를 해야되고 함수호출로 읽어오는것이라
    /// 느릴수밖에없다.
    /// </summary>
    public Func<NpcBase_Gyu> onTalkClick;

    /// <summary>
    /// 대화 내용을 가져온다.
    /// </summary>
    public Func<int, string[]> getTalkDataArray;





    /// <summary>
    /// 로그 관리해줄 컴포넌트
    /// </summary>
    LogManager logManager;
    public LogManager LogManager => logManager;

    CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();   
        cg.alpha = 1.0f;

        logManager = FindObjectOfType<LogManager>(true);
        talkData = FindObjectOfType<TalkData_Gyu>();

        npcImg = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        nameBox = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        talkBox = transform.GetChild(2).GetComponent<TextMeshProUGUI>();



        talkEndButton = transform.GetChild(3).GetComponent<Button>();
        talkEndButton.onClick.AddListener(ResetData);


        nextButton = transform.GetChild(4).GetComponent<Button>();
        nextButton.onClick.AddListener(() => Talk(currentTalkIndex + 1));


        logButton = transform.GetChild(5).GetComponent<Button>();
        logButton.onClick.AddListener(() => {
            logManager.LogBoxSetting(currentTalkIndex);
        });


       
    }
    private void Start()
    {
        ResetData();
    }
    public void ReTalking() 
    {
        isTalking = false;
        currentTalkIndex = 0;
        StopAllCoroutines();
        Talk(currentTalkIndex);
    }
    /// <summary>
    /// Npc 대사를 출력
    /// </summary>
    public void Talk(int talkIndex)
    {
        if (!isTalking)
        {
            cg.alpha = 1.0f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
            openTalkWindow?.Invoke();
            //Debug.Log($"값이 없으면 안되는데 ? : {openTalkWindow}");
            NpcBase_Gyu npc = onTalkClick?.Invoke();
            npcImg.texture = npc.GetTexture;
            if (npc != null)
            {
                nameBox.text = npc.name;
                string[] talkString = getTalkDataArray?.Invoke(talkIndex);
                if (talkString != null && talkString.Length > 0)
                {

                    StartCoroutine(Typing(talkString, talkIndex));
                }
            }

            //OpenWindow(npc.);
        }
    }

    /// <summary>
    /// 타이핑 효과가있는 텍스트 출력 하기
    /// </summary>
    /// <param name="textArray"></param>
    /// <returns></returns>
    IEnumerator Typing(string[] textArray, int talkIndex)
    {
        isTalking = true;
        talkBox.text = "";
        foreach (string text in textArray)
        {
            foreach (char letter in text.ToCharArray())
            {
                talkBox.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
            talkBox.text += "\r\n";
        }
        currentTalkIndex = talkIndex;
        isTalking = false;
    }

    public void ResetData() 
    {
        closeTalkWindow?.Invoke();
        currentTalkIndex = 0;
        npcImg.texture = null;
        nameBox.text = "";
        talkBox.text = "";
        logManager.ResetData();
        cg.alpha = 0.0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
        isTalking = false;
    }
}
