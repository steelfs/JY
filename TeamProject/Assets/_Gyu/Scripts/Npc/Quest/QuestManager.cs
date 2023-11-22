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
    public TalkData talkData;   // ��� ����
    private int ItemNumber;     // ����Ʈ ���� ������&���

    // UI���� üũ
    private bool isTalk;
    private bool isNpcQuest;
    private bool isMyQuest;

    // ��ȭâ UI
    public GameObject[] Buttons;
    public GameObject TalkPanel;
    public TextMeshProUGUI NameBox;
    public TextMeshProUGUI TalkBox;

    // Npc ����Ʈâ UI
    public GameObject QuestPanel;
    public TextMeshProUGUI TitleBox;
    public TextMeshProUGUI DescriptionBox;
    public TextMeshProUGUI ClearBox;

    // Player ����Ʈâ UI
    public GameObject MyQuestPanel;
    public TextMeshProUGUI MyQeustBox;

    // Npc����Ʈ
    public List<Quest> quests;

    // ĵ���� ��ġ
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

    // ȭ�� �߾� ��ȭ�ϱ� ��ư
    public void Action(int npc)
    {
        //ItemNumber = (int)QuestManager.instance.quests[CharBase.instance.questChapter].questReward.RewardItem;

        Talk(npc);
    }

    /// <summary>
    /// Npc ��縦 ���
    /// </summary>
    /// <param name="id">QuestManager�� Npcid�� TalkData�� Npcid�� ��Ī�ؼ� ��縦 ���</param>
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

    // Npc ��ȣ�ۿ� - ����Ʈ ����, ����, �Ϸ�
    public void NpcQuest()
    {
        if (!isNpcQuest)
        {
            // ����Ʈ Ŭ����X, ����X
            if (quests[PlayerQuest.instance.questCount].isSucess == false && quests[PlayerQuest.instance.questCount].isProgress == false)
            {
                ForQuest();
                isNpcQuest = true;
                TitleBox.text = quests[PlayerQuest.instance.questCount].Title;
                DescriptionBox.text = quests[PlayerQuest.instance.questCount].Description;
                ClearBox.text = quests[PlayerQuest.instance.questCount].Clear;
            }
            // ����Ʈ Ŭ����X, ����O
            else if (quests[PlayerQuest.instance.questCount].isProgress == true && quests[PlayerQuest.instance.questCount].isSucess == false)
            {
                ProgressQuest();
                isNpcQuest = true;
                TitleBox.text = PlayerQuest.instance.myquest.Title;
                DescriptionBox.text = PlayerQuest.instance.myquest.Description;
                ClearBox.text = PlayerQuest.instance.myquest.Clear;
            }
            // ����Ʈ Ŭ����O, ����O
            else if (quests[PlayerQuest.instance.questCount].isProgress == true && quests[PlayerQuest.instance.questCount].isSucess == true)
            {
                SucessQuest();
                isNpcQuest = true;
                TitleBox.text = PlayerQuest.instance.myquest.Title;
                DescriptionBox.text = PlayerQuest.instance.myquest.Description;
                ClearBox.text = PlayerQuest.instance.myquest.Clear;
            }
        }
        //����Ʈ �ʱ�ȭ
        else
        {
            ResetNpcQuest();
        }

        QuestPanel.SetActive(isNpcQuest);
    }

    // ����Ʈ ���� ��ư
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

    // ����Ʈ ���� ��ư
    public void CancelButton()
    {
        ResetNpcQuest();
        QuestPanel.SetActive(isNpcQuest);
    }

    // ����Ʈ�� �Ϸ� ��ư(Ŭ����)
    public void SucessButton()
    {
        ResetNpcQuest();
        PlayerQuest.instance.myquest.Description = "";

        // ����Ʈ Ŭ���� ���� ����
        //var itemC = (ItemCode)ItemNumber;
        //Debug.Log(itemC);
        //GameManager.SlotManager.AddItem(itemC);

        QuestPanel.SetActive(isNpcQuest);
    }

    // ���� �������� ����Ʈ ����
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
    /// ���� ������ ����Ʈ
    /// </summary>
    void ForQuest()
    {
        Buttons[0].SetActive(true);  // ����
        Buttons[1].SetActive(true);  // ����
        Buttons[2].SetActive(false); // �Ϸ�
    }

    /// <summary>
    /// �Ϸ� ������ ����Ʈ
    /// </summary>
    void SucessQuest()
    {
        ResetNpcQuest();
        Buttons[0].SetActive(false); // ����
        Buttons[1].SetActive(false); // ����
        Buttons[2].SetActive(true);  // �Ϸ�

        PlayerQuest.instance.questCount += 1;
    }

    /// <summary>
    /// ����Ʈ�� �������϶�
    /// </summary>
    void ProgressQuest()
    {
        Buttons[0].SetActive(false); // ����
        Buttons[1].SetActive(false); // ����
        Buttons[2].SetActive(false); // �Ϸ�
    }

    // ����Ʈ.Panel �ʱ�ȭ
    void ResetNpcQuest()
    {
        isNpcQuest = false;
        TitleBox.text = null;
        DescriptionBox.text = null;
        ClearBox.text = null;
    }

    // ���.Panel �ʱ�ȭ
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

    // Ÿ���� ȿ��
    IEnumerator Typing(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            TalkBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
