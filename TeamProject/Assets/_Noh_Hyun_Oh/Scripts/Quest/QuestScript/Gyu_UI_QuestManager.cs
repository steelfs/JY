using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum Quest_State
{
    None = 0,               // ����Ʈ ����������������
    Quest_Start,            // ����Ʈ ��������
    //Quest_Cancel,           // ����Ʈ �����ϰ� ����� ����
    Quest_Complete,         // ����Ʈ �Ϸ��� ����
}
public class Gyu_UI_QuestManager : MonoBehaviour, IPopupSortWindow
{
   
    /// <summary>
    /// ����Ʈâ ����
    /// </summary>
    TextMeshProUGUI titleBox;

    /// <summary>
    /// ����Ʈâ ����
    /// </summary>
    TextMeshProUGUI descriptionBox;

    /// <summary>
    /// ����Ʈâ ���� ����
    /// </summary>
    TextMeshProUGUI clearBox;

    /// <summary>
    /// ����Ʈâ ����
    /// </summary>
    Button questAcceptButton;
    public Button QuestAcceptButton => questAcceptButton;

    /// <summary>
    /// ����Ʈâ ��� 
    /// </summary>
    Button questCancelButton;
    public Button QuestCancelButton => questCancelButton;

    /// <summary>
    /// ����Ʈâ Ȯ��
    /// </summary>
    Button questSuccessButton;
    public Button QuestSuccessButton => questSuccessButton;

    /// <summary>
    /// ����Ʈ �ڽ��� Ʈ������
    /// </summary>
    Transform questBoxPanel;



    /// <summary>
    /// ����Ʈ Ȯ�� ��ư
    /// </summary>
    Button questConfirm;
    public Button QuestConfirm => questConfirm;





    /// <summary>
    /// �������� ����Ʈ ���� 
    /// </summary>
    TextMeshProUGUI myQuestBox;

    /// <summary>
    /// �������� �ؽ�Ʈ�ڽ��� Ʈ������ 
    /// </summary>
    Transform myQuestBoxPanel;




    /// <summary>
    /// ����Ʈ ����Ʈ ������ â�� Ʈ������
    /// </summary>
    Transform questListPanel;

    /// <summary>
    /// ����Ʈ ����Ʈ �� ���� ������ �� Ʈ������
    /// </summary>
    Transform questListContentPanel;


    /// <summary>
    /// ����Ʈ ����Ʈ �� ������ UI ��� 
    /// </summary>
    Quest_UI_Colum[] quest_UI_Array;

    /// <summary>
    /// ���õ� ����Ʈ UI ������Ʈ
    /// </summary>
    Quest_UI_Colum selectedColum;
    Quest_UI_Colum SelectedColum 
    {
        get => selectedColum;
        set 
        {
            if (selectedColum != value) 
            {
                if (selectedColum != null) 
                {

                    selectedColum.IsSelectedCheck = false;
                }
                selectedColum = value;
            }
        }
    }

    public Action<IPopupSortWindow> PopupSorting { get ; set ; }

  

    //----------------- ����Ʈ �޴���(���� �����Ͱ����ϴ�) �� ������ Action��
    /// <summary>
    /// questData ��������� �������� 
    /// ��ɺи��ϱ����� ���λ��� �߰���
    /// </summary>
    Gyu_QuestManager questManager;

  



    /// <summary>
    /// ����Ʈ �����̉�ٰ� �˷��ִ� ��������Ʈ
    /// </summary>
    public Action   onAcceptQuest;
    
    /// <summary>
    /// ����Ʈ �Ϸᰡ ��ٰ� �˷��ִ� ��������Ʈ 
    /// </summary>
    public Action   onSucessQuest;

    /// <summary>
    /// ����Ʈ�� ��� ��ٰ� �˷��ִ� ��������Ʈ
    /// </summary>
    public Action onCancelQuest;

    /// <summary>
    /// ����Ʈ ����Ʈ���� ����Ʈ�� ���É�ٰ� �˸��� ��������Ʈ
    /// </summary>
    public Action<Gyu_QuestBaseData> onSelectedQuest;


    /// <summary>
    /// ����Ʈ ����Ʈ�� �������� ����Ʈ �����ִ� ��ư
    /// </summary>
    Button questList_CurrentViewBtn;
    /// <summary>
    /// ����Ʈ ����Ʈ�� �Ϸ�� ����Ʈ �����ִ� ��ư
    /// </summary>
    Button questList_ClearViewBtn;


    /// <summary>
    /// ��ȭ �������� ������Ʈ
    /// </summary>
    NpcTalkController npcTalkController;

    /// <summary>
    /// 
    /// </summary>
    CanvasGroup cg;

    /// <summary>
    /// ���� ����Ʈ �Ϸ�� ����Ŭ���� ��ٰ� �˷��ִ� ��������Ʈ 
    /// </summary>
    public Action OnGameClear;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;


        npcTalkController = FindObjectOfType<NpcTalkController>();
        
        npcTalkController.openTalkWindow += () => { };
        npcTalkController.closeTalkWindow += initialize;
        
        questManager = GetComponent<Gyu_QuestManager>(); 

        questBoxPanel = transform.GetChild(0);
        titleBox = questBoxPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionBox = questBoxPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        clearBox = questBoxPanel.GetChild(2).GetComponent<TextMeshProUGUI>();

        questAcceptButton = questBoxPanel.GetChild(3).GetComponent<Button>();
        questAcceptButton.onClick.AddListener(AcceptButton);
        
        questCancelButton = questBoxPanel.GetChild(4).GetComponent<Button>();
        questCancelButton.onClick.AddListener(CancelButton);

        questSuccessButton = questBoxPanel.GetChild(5).GetComponent<Button>();
        questSuccessButton.onClick.AddListener(SucessButton);



        questConfirm = transform.GetChild(1).GetComponent<Button>();
        questConfirm.onClick.AddListener(ToNpcQuestListPanelToggle);



        myQuestBoxPanel = transform.GetChild(2); 
        myQuestBox = myQuestBoxPanel.GetChild(1).GetComponent<TextMeshProUGUI>();



        questListPanel = transform.GetChild(3);
        questList_CurrentViewBtn = questListPanel.GetChild(2).GetComponent<Button>();
        questList_CurrentViewBtn.onClick.AddListener(() => {
            ToPlayerCurrentQuestListPanelOpen();
        });
        questList_ClearViewBtn = questListPanel.GetChild(3).GetComponent<Button>();
        questList_ClearViewBtn.onClick.AddListener(() => {
            ToPlayerCompleteQuestListPanelOpen();
        });

        questListContentPanel = questListPanel.GetComponentInChildren<VerticalLayoutGroup>(true).transform;
        quest_UI_Array = questListContentPanel.GetComponentsInChildren<Quest_UI_Colum>(true);



        npcTalkController.ResetData();
    }

    private void Start()
    {
        OnGameClear = WindowList.Instance.EndingCutImageFunc.EndingCutScene;

        InputSystemController.InputSystem.Options.Quest.performed += (_) => {
            //������������ ����Ʈ�� ��� �������� ���ѵڿ� �������� ���� �߰��ؾߵ� 
            //Gyu_QuestBaseData questData  =  questManager.Player.CurrentQuests[0];
            //MyQuestButton(questData);
            //����Ʈ�� ����ش�.
            if (!questListPanel.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ToPlayerCurrentQuestListPanelOpen();
                OpenWindow();
            }
            else 
            {
                QuestListPanelClose();
            }
        };
        InputSystemController.InputSystem.Common.Esc.performed += (_) => {
            initialize();
        };
    }


    public void OnQuestNpc() 
    {
        questConfirm.gameObject.SetActive(true);
    }

   /// <summary>
    /// ����Ʈ ����Ʈ �ݱ� �Լ�
    /// </summary>
    private void QuestListPanelClose() 
    {
        questListPanel.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// ĳ���Ͱ� ������ ����Ʈ ����Ʈ�� �����ִ� �Լ�
    /// </summary>
    public void ToPlayerCurrentQuestListPanelOpen()
    {
        questBoxPanel.gameObject.SetActive(false); //����Ʈ �� ������ ���� 
        questListPanel.gameObject.SetActive(true);
        QuestListDataReset();   //���� �����͸��� 
        ToPlayerQuestListWindowOpenAndDataSetting(questManager.Player.CurrentQuests); //������ ������ ���� 
    }

    /// <summary>
    /// ĳ���Ͱ� �Ϸ��� ����Ʈ ����Ʈ�� �����ִ� �Լ�
    /// </summary>
    public void ToPlayerCompleteQuestListPanelOpen()
    {
        questBoxPanel.gameObject.SetActive(false); //����Ʈ �� ������ ���� 
        questListPanel.gameObject.SetActive(true);
        QuestListDataReset();   //���� �����͸��� 
        ToPlayerQuestListWindowOpenAndDataSetting(questManager.Player.ClearQuestList); //������ ������ ���� 
    }

    /// <summary>
    /// ����Ʈ ����Ʈ ������ ������ Ȱ��ȭ ��Ű�� �Լ� 
    /// </summary>
    /// <param name="questDataList">������ ����Ʈ ����Ʈ</param>
    private void ToPlayerQuestListWindowOpenAndDataSetting(List<Gyu_QuestBaseData> questDataList)
    {
        int questIndexCount = 0;
        int checkCount = quest_UI_Array.Length - 1;   //ui���� �̻� ������ ���������ʰ� üũ�� ������Ƶΰ�
        foreach (Gyu_QuestBaseData quest_UI in questDataList)   //����Ʈ ������ �ٱܾ�ͼ� 
        {
            quest_UI_Array[questIndexCount].SetData(quest_UI);          // ������ �����ϰ� 
            quest_UI_Array[questIndexCount].onClick = ToPlayerQuestItemClick;   // �׼� �����ϰ� 
            //UI�� �������� ���������ϰ��־ üũ�ϴ� ���� �߰� 
            questIndexCount++;
            if (questIndexCount > checkCount) break;
        }
    }

    /// <summary>
    /// �÷��̾��� ����Ʈ����Ʈ���� 
    /// ����Ʈ����Ʈ���� ���ϴ� ����Ʈ�� Ŭ�������� ������ ���� 
    /// </summary>
    /// <param name="questData"> Ŭ���� ����Ʈ ������</param>
    /// <param name="selectColum"> Ŭ���� ����Ʈ UI ������Ʈ</param>
    private void ToPlayerQuestItemClick(Gyu_QuestBaseData questData, Quest_UI_Colum selectColum)
    {
        selectColum.IsSelectedCheck = true;
        MyQuestButton(questData);

        SelectedColum = selectColum;
 
    }

    /// <summary>
    /// NPC �� ���� ������ Quest List �� ������ִ� â ���� ������ �����ϱ�
    /// </summary>
    public void ToNpcQuestListPanelToggle()
    {
        myQuestBoxPanel.gameObject.SetActive(false);
        questListPanel.gameObject.SetActive(true);
        QuestListDataReset();   //���� �����͸��� 
        QuestNPC currentNpc = (QuestNPC)questManager.Array_NPC[questManager.CurrentNpcIndex]; //���� ��ȭ���� ���Ǿ������� ��������
        ToNpcQuestListWindowOpenAndDataSetting(currentNpc.OwnQuestList); //������ ������ ���� 
    }

    /// <summary>
    /// ����Ʈ ����Ʈ ������ ������ Ȱ��ȭ ��Ű�� �Լ� 
    /// </summary>
    /// <param name="questDataList">������ ����Ʈ ����Ʈ</param>
    private void ToNpcQuestListWindowOpenAndDataSetting(List<Gyu_QuestBaseData> questDataList) 
    {
        int questIndexCount = 0;
        int checkCount = quest_UI_Array.Length-1;   //ui���� �̻� ������ ���������ʰ� üũ�� ������Ƶΰ�
        foreach (Gyu_QuestBaseData quest_UI in questDataList)   //����Ʈ ������ �ٱܾ�ͼ� 
        {
            //Debug.Log($"����Ʈ �ε�����: {quest_UI.QuestId} , ����Ʈ ������: {quest_UI.Title}");
            quest_UI_Array[questIndexCount].SetData(quest_UI);          // ������ �����ϰ� 
            quest_UI_Array[questIndexCount].onClick = QuestItemClick;   // �׼� �����ϰ� 
            //UI�� �������� ���������ϰ��־ üũ�ϴ� ���� �߰� 
            questIndexCount++;
            if (questIndexCount > checkCount) break;
        }
    }

    /// <summary>
    /// ����Ʈ ����Ʈ UI ������ �ʱ�ȭ ��Ű���Լ�
    /// </summary>
    private void QuestListDataReset() 
    {
        foreach (Quest_UI_Colum quest_UI in quest_UI_Array)
        {
            quest_UI.gameObject.SetActive(true);
            quest_UI.ResetData();
        }
    }

    /// <summary>
    /// ����Ʈ����Ʈ���� ���ϴ� ����Ʈ�� Ŭ�������� ������ ���� 
    /// </summary>
    /// <param name="questData"> Ŭ���� ����Ʈ ������</param>
    /// <param name="selectColum"> Ŭ���� ����Ʈ UI ������Ʈ</param>
    private void QuestItemClick(Gyu_QuestBaseData questData, Quest_UI_Colum selectColum) 
    {
        selectColum.IsSelectedCheck = true;
        NpcQuest(questData);
        onSelectedQuest?.Invoke(questData);
        SelectedColum = selectColum;
        npcTalkController.ReTalking();
    }


    // Npc ��ȣ�ۿ� - ����Ʈ ����, ����, �Ϸ�
    public void NpcQuest(Gyu_QuestBaseData questData)
    {
        questBoxPanel.gameObject.SetActive(true);

        if (questData.Quest_State == Quest_State.None)  //����Ʈ ���۾ȉ����� 
        {
            ForQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if (questData.Quest_State != Quest_State.Quest_Complete && questData.IsSucess())    //����Ʈ �Ϸ� ���� ����
        {
            SucessQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if (questData.Quest_State == Quest_State.Quest_Start) // ����Ʈ �������λ��� 
        {
            ProgressQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
        else if(questData.Quest_State == Quest_State.Quest_Complete) //�Ϸ�� ����Ʈ 
        {
            ProgressQuest();
            titleBox.text = questData.Title;
            descriptionBox.text = questData.Description;
            clearBox.text = questData.ClearObjectives;
            return;
        }
    }

    // ����Ʈ ���� ��ư
    public void AcceptButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onAcceptQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.Quest_Start;
        }
    }

    // ����Ʈ ���� ��ư
    public void CancelButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onCancelQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.None;
        }
    }

    // ����Ʈ�� �Ϸ� ��ư(Ŭ����)
    public void SucessButton()
    {
        if (questBoxPanel.gameObject.activeSelf)
        {
            questBoxPanel.gameObject.SetActive(false);
            onSucessQuest?.Invoke();
            selectedColum.Quest_State = Quest_State.Quest_Complete;
            if (selectedColum.ThisQuestData.QuestType == QuestType.Story) 
            {
                OnGameClear?.Invoke();
            }
        }
    }

    // ���� �������� ����Ʈ ����
    public void MyQuestButton(Gyu_QuestBaseData questData)
    {
        myQuestBoxPanel.gameObject.SetActive(true);
        if (questData != null)  //�������� ����Ʈ�� �ִ°�� 
        {
            myQuestBox.text = questData.Title;
            myQuestBox.text += "\r\n";
            int forSize = questData.CurrentCount.Length;
            Array enumArray;
            if (questData.QuestMosters != null && questData.QuestMosters.Length > 0)
            {
                enumArray = questData.QuestMosters;
            }
            else 
            {
                enumArray = questData.RequestItem;
            }
            for (int i = 0; i < forSize; i++)
            {
                    myQuestBox.text += $"\r\n {enumArray.GetValue(i)} : {questData.CurrentCount[i]} / {questData.RequiredCount[i]}";
            }
        }
        else //���°�� 
        {
            myQuestBox.text = "�������� ������ �����ϴ�."; 
            //�̰��� ���� ���־��ϴµ� �ϴ� �� �̷��� �ص���
        }
    }

    /// <summary>
    /// ���� ������ ����Ʈ
    /// </summary>
    void ForQuest()
    {
        questAcceptButton.gameObject.SetActive(true);
        questCancelButton.gameObject.SetActive(true);
        questSuccessButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// �Ϸ� ������ ����Ʈ
    /// </summary>
    void SucessQuest()
    {
        questAcceptButton.gameObject.SetActive(false);
        questCancelButton.gameObject.SetActive(false);
        questSuccessButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// ����Ʈ�� �������϶�
    /// </summary>
    void ProgressQuest()
    {
        questAcceptButton.gameObject.SetActive(false);
        questCancelButton.gameObject.SetActive(false);
        questSuccessButton.gameObject.SetActive(false);
    }


    /// <summary>
    /// ����Ʈ UI �ʱ�ȭ ��Ű�� �Լ�
    /// </summary>
    public void initialize()
    {
        onSelectedQuest?.Invoke(null);
        titleBox.text = "";
        descriptionBox.text = "";
        clearBox.text = "";
       

        myQuestBoxPanel.gameObject.SetActive(false);
        questBoxPanel.gameObject.SetActive(false);
        questConfirm.gameObject.SetActive(false);
        questListPanel.gameObject.SetActive(false);

        if (questManager.ActionUI != null && questManager.IsActionActive) 
        {
            questManager.ActionUI.visibleUI();
        }
        //quests = null;
        //array_NPC = null;
    }



    public void OpenWindow()
    {
        PopupSorting(this);
    }

    public void CloseWindow()
    {
        npcTalkController.ResetData();
    }
}
