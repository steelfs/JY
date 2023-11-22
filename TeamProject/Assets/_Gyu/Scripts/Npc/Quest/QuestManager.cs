using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public TalkData talkData;   // 대사 모음
    private int ItemNumber;     // 퀘스트 보상 아이템&골드

    // UI상태 체크
    private bool isTalk;
    private bool isNpcQuest;
    private bool isMyQuest;

    // 대화창 UI
    public GameObject[] Buttons;
    public GameObject TalkPanel;
    public TextMeshProUGUI NameBox;
    public TextMeshProUGUI TalkBox;

    // Npc 퀘스트창 UI
    public GameObject QuestPanel;
    public TextMeshProUGUI TitleBox;
    public TextMeshProUGUI DescriptionBox;
    public TextMeshProUGUI ClearBox;

    // Player 퀘스트창 UI
    public GameObject MyQuestPanel;
    public TextMeshProUGUI MyQeustBox;

    // Npc퀘스트
    public List<Quest> quests;

    // 캔버스 위치
    GameObject CanvasLocation;

    NpcBase NpcMe;

    private void Awake()
    {
        talkData = GameObject.FindAnyObjectByType<TalkData>();
        Buttons = new GameObject[3];

        CanvasLocation = GameObject.Find("Canvas");
        TalkPanel = CanvasLocation.transform.GetChild(0).gameObject;
        QuestPanel = CanvasLocation.transform.GetChild(1).gameObject;
        MyQuestPanel=CanvasLocation.transform.GetChild(2).gameObject;
        NameBox = TalkPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TalkBox = TalkPanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        DescriptionBox = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TitleBox = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        ClearBox = QuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[2];
        MyQeustBox = MyQuestPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        for(int i = 0; i < 3; i++)
            Buttons[i] = QuestPanel.GetComponentsInChildren<Button>()[i].gameObject;
        NpcMe = GetComponentInChildren<NpcBase>();

        if (instance == null)
            instance = this;

        initialize();
        MyQuestPanel.SetActive(false);
    }

    // 화면 중앙 대화하기 버튼
    public void Action(int npc)
    {
        //ItemNumber = (int)QuestManager.instance.quests[CharBase.instance.questChapter].questReward.RewardItem;

        Talk(npc);
    }

    /// <summary>
    /// Npc 대사를 출력
    /// </summary>
    /// <param name="id">QuestManager의 Npcid와 TalkData의 Npcid를 대칭해서 대사를 출력</param>
    public void Talk(int id)
    {
        if (!isTalk)
        {
            if (PlayerQuest.instance.myquest.questClear.clearType == QuestClear.ClearType.Comunication)
            {
                PlayerQuest.instance.myquest.isSucess = true;
            }

            NameBox.text = transform.name;
            isTalk = true;
            TalkPanel.SetActive(isTalk);
            string talkString = talkData.GetTalk(id);
            StartCoroutine(Typing(talkString));
        }
        else
        {
            initialize();
            TalkPanel.SetActive(isTalk);
        }
    }

    // Npc 상호작용 - 퀘스트 수락, 거절, 완료
    public void NpcQuest()
    {
        if (!isNpcQuest)
        {
            // 퀘스트 클리어X, 진행X
            if (quests[PlayerQuest.instance.questCount].isSucess == false && quests[PlayerQuest.instance.questCount].isProgress == false)
            {
                ForQuest();
                isNpcQuest = true;
                TitleBox.text = quests[PlayerQuest.instance.questCount].Title;
                DescriptionBox.text = quests[PlayerQuest.instance.questCount].Description;
                ClearBox.text = quests[PlayerQuest.instance.questCount].Clear;
            }
            // 퀘스트 클리어X, 진행O
            else if (quests[PlayerQuest.instance.questCount].isProgress == true && quests[PlayerQuest.instance.questCount].isSucess == false)
            {
                ProgressQuest();
                isNpcQuest = true;
                TitleBox.text = PlayerQuest.instance.myquest.Title;
                DescriptionBox.text = PlayerQuest.instance.myquest.Description;
                ClearBox.text = PlayerQuest.instance.myquest.Clear;
            }
            // 퀘스트 클리어O, 진행O
            else if (quests[PlayerQuest.instance.questCount].isProgress == true && quests[PlayerQuest.instance.questCount].isSucess == true)
            {
                SucessQuest();
                isNpcQuest = true;
                TitleBox.text = PlayerQuest.instance.myquest.Title;
                DescriptionBox.text = PlayerQuest.instance.myquest.Description;
                ClearBox.text = PlayerQuest.instance.myquest.Clear;
            }
        }
        //퀘스트 초기화
        else
        {
            ResetNpcQuest();
        }

        QuestPanel.SetActive(isNpcQuest);
    }

    // 퀘스트 수락 버튼
    public void AcceptButton()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (QuestManager.instance.quests[i].isSucess == false && QuestManager.instance.quests[i].isProgress == false)
            {
                PlayerQuest.instance.myquest = QuestManager.instance.quests[i];
                QuestManager.instance.quests[i].isProgress = true;
                ResetNpcQuest();
                break;
            }
        }

        QuestPanel.SetActive(isNpcQuest);
    }

    // 퀘스트 거절 버튼
    public void CancelButton()
    {
        ResetNpcQuest();
        QuestPanel.SetActive(isNpcQuest);
    }

    // 퀘스트를 완료 버튼(클리어)
    public void SucessButton()
    {
        ResetNpcQuest();
        PlayerQuest.instance.myquest.Description = "";

        // 퀘스트 클리어 보상 수령
        //var itemC = (ItemCode)ItemNumber;
        //Debug.Log(itemC);
        //GameManager.SlotManager.AddItem(itemC);

        QuestPanel.SetActive(isNpcQuest);
    }

    // 현재 진행중인 퀘스트 보기
    public void MyQuestButton()
    {
        if (!isMyQuest)
        {
            isMyQuest = true;
            MyQeustBox.text = PlayerQuest.instance.myquest.Description;
        }
        else
            isMyQuest = false;

        MyQuestPanel.SetActive(isMyQuest);
    }

    /// <summary>
    /// 수락 가능한 퀘스트
    /// </summary>
    void ForQuest()
    {
        Buttons[0].SetActive(true);  // 수락
        Buttons[1].SetActive(true);  // 거절
        Buttons[2].SetActive(false); // 완료
    }

    /// <summary>
    /// 완료 가능한 퀘스트
    /// </summary>
    void SucessQuest()
    {
        ResetNpcQuest();
        Buttons[0].SetActive(false); // 수락
        Buttons[1].SetActive(false); // 거절
        Buttons[2].SetActive(true);  // 완료

        PlayerQuest.instance.questCount += 1;
    }

    /// <summary>
    /// 퀘스트가 진행중일때
    /// </summary>
    void ProgressQuest()
    {
        Buttons[0].SetActive(false); // 수락
        Buttons[1].SetActive(false); // 거절
        Buttons[2].SetActive(false); // 완료
    }

    // 퀘스트.Panel 초기화
    void ResetNpcQuest()
    {
        isNpcQuest = false;
        TitleBox.text = null;
        DescriptionBox.text = null;
        ClearBox.text = null;
    }

    // 모든.Panel 초기화
    public void initialize()
    {
        isTalk = false;
        TalkPanel.SetActive(false);
        NameBox.text = null;
        TalkBox.text = null;

        isNpcQuest = false;
        QuestPanel.SetActive(false);
        TitleBox.text = null;
        DescriptionBox.text = null;
        ClearBox.text = null;
    }

    // 타이핑 효과
    IEnumerator Typing(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            TalkBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
