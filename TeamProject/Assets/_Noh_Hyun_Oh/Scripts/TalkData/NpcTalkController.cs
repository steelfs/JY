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
    /// ���Ǿ� ��ȭ��� �����ϴ� Ŭ���� 
    /// </summary>
    TalkData_Gyu talkData;
    public TalkData_Gyu TalkData => talkData;



    /// <summary>
    /// ��ȭ �ڷ�ƾ �ߺ�üũ�� 
    /// </summary>
    bool isTalking = false;
    public bool IsTalking 
    {
        get => isTalking;
        set => isTalking = value;
    } 
    /// <summary>
    /// ���� ��ȭ���� 
    /// </summary>
    int currentTalkIndex = 0;




    /// <summary>
    /// ���Ǿ� �̹���
    /// </summary>
    RawImage npcImg;

    /// <summary>
    /// ���Ǿ� �̸�
    /// </summary>
    TextMeshProUGUI nameBox;

    /// <summary>
    /// ���Ǿ� ��ȭ���
    /// </summary>
    TextMeshProUGUI talkBox;

    /// <summary>
    /// ��ȭ���� ��ư
    /// </summary>
    Button talkEndButton;
    public Button TalkEndButton => talkEndButton;

    
    /// <summary>
    /// ���� ���� �ѱ�� ��ư
    /// </summary>
    Button nextButton;

    /// <summary>
    /// �α� ��� ��ư 
    /// </summary>
    Button logButton;





    public Action openTalkWindow;
    public Action closeTalkWindow;


    /// <summary>
    /// Func�� �����Ϳ���Ÿ�̹� ���߱⿡�� �������� �⺻������ null üũ�� �ؾߵǰ� �Լ�ȣ��� �о���°��̶�
    /// �������ۿ�����.
    /// </summary>
    public Func<NpcBase_Gyu> onTalkClick;

    /// <summary>
    /// ��ȭ ������ �����´�.
    /// </summary>
    public Func<int, string[]> getTalkDataArray;





    /// <summary>
    /// �α� �������� ������Ʈ
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
    /// Npc ��縦 ���
    /// </summary>
    public void Talk(int talkIndex)
    {
        if (!isTalking)
        {
            cg.alpha = 1.0f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
            openTalkWindow?.Invoke();
            //Debug.Log($"���� ������ �ȵǴµ� ? : {openTalkWindow}");
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
    /// Ÿ���� ȿ�����ִ� �ؽ�Ʈ ��� �ϱ�
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
