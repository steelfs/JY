using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 타입 해당 순서 바꾸면 퀘스트 인덱스 값도 같이 바뀌니 조심하자
/// 자세한건 QuestScriptableGenerate 스크립트 생성 부분참조
/// </summary>
public enum QuestType
{
    Story = 0,                   //  시나리오 퀘스트
    Killcount ,              //  토벌퀘스트
    Gathering ,              //  수집퀘스트
}

/// <summary>
/// 대화 타입
/// </summary>
public enum TalkType
{
    Comunication = 0,           //  일반대화
    Story,                      //  시나리오 퀘스트
    KillCount,                  //  토벌퀘스트
    Gathering,                  //  수집퀘스트
}

/// <summary>
/// 임시로 잡아둔 몬스터 타입
/// </summary>
public enum Monster_Type 
{
    Base = 0,
    Size_S ,
    Size_M ,
    Size_L ,
    Boss ,
}

/// <summary>
/// 유아이는 메니저를 캔버스로 이동시켜서 한곳에서 관리하도록한다.
/// </summary>
public class Gyu_QuestManager : MonoBehaviour
{
    /// <summary>
    /// 플레이어 데이터 
    /// </summary>
    [SerializeField]
    PlayerQuest_Gyu player;
    public PlayerQuest_Gyu Player => player;

    /// <summary>
    /// 엔피씨가 대화시 바라볼방향
    /// </summary>
    [SerializeField]
    PlayerLookTarget looktarget;


    /// <summary>
    /// UI Action 연결용으로 가져오기 
    /// </summary>
    Gyu_UI_QuestManager questUIManager;
    public Gyu_UI_QuestManager QuestUIManager => questUIManager; 

    /// <summary>
    /// 선택된 퀘스트 담아둘 변수
    /// </summary>
    Gyu_QuestBaseData selectQuest;
    Gyu_QuestBaseData SelectQuest 
    {
        get => selectQuest;
        set 
        {
            if (selectQuest != value)
            {
                selectQuest = value;
                onChangeQuest?.Invoke(selectQuest);

            }
        }
    }

    /// <summary>
    /// 맵에있는 NPC 들
    /// </summary>
    [SerializeField]
    QuestNPC[] array_NPC;
    public QuestNPC[] Array_NPC => array_NPC;

    /// <summary>
    /// 마지막에 창을 열고있던 NPC 인덱스
    /// </summary>
    int currentNpcIndex = -1;
    public int CurrentNpcIndex => currentNpcIndex;


    public Action<Gyu_QuestBaseData> onChangeQuest;

    /// <summary>
    /// 퀘스트 원본이있는곳 싱글톤으로 나중에빼야한다 지금은 테스트라 이대로 테스트
    /// </summary>
    QuestScriptableGenerate questScriptableGenerate;
    [SerializeField]
    NpcTalkController talkController;
    public NpcTalkController TalkController => talkController;
    InteractionUI actionUI;
    public InteractionUI ActionUI => actionUI;

    /// <summary>
    /// 기능 활성화여부 
    /// </summary>
    bool isActionActive = false;
    public bool IsActionActive => isActionActive;
    private void Awake()
    {
        player = FindObjectOfType<PlayerQuest_Gyu>();

        talkController = FindObjectOfType<NpcTalkController>();

        questUIManager = GetComponent<Gyu_UI_QuestManager>();   //기능분리를 위해 스크립트를 따로뺏다.
    }

    private void Start()
    {
        questUIManager.onSelectedQuest = (quest) =>
        {
            //퀘스트 선택
            SelectQuest = quest;
        };

        questUIManager.onAcceptQuest = () =>
        {
            //퀘스트 추가
            player.AppendQuest(selectQuest);
        };

        questUIManager.onSucessQuest = () =>
        {
            //퀘스트 완료 
            player.ClearQuest(selectQuest);
        };

        questUIManager.onCancelQuest = () =>
        {
            //퀘스트 취소 
            player.CancelQuest(selectQuest);
        };

       

        questScriptableGenerate = DataFactory.Instance.QuestScriptableGenerate;

        //F 키눌렀을때의 액션 연결
        InputSystemController.InputSystem.Player.Action.performed += (_) => {
            if (isActionActive)
            {
                talkController.ResetData();
                talkController.Talk(0);
                actionUI.invisibleUI?.Invoke();
            }
        };

    }
    public void InitDataSetting()
    {
        actionUI = FindObjectOfType<InteractionUI>(true);
        // 팩토리로 할시 엔피씨 위치를 몇개 후보지역두고 랜덤으로 변경시키는게 더간단할거같다.
        // 초기화 하는것은 여기말고 다른곳으로 빼서 해야될거같다 .. 팩토리 로 생성시킨뒤에 껏다켯다하면 될거같긴한데.. 
        array_NPC = FindObjectsOfType<QuestNPC>(true);   //씬에있는 엔피씨 찾아서 담아두고 ( 찾는 순서가 바뀔수도있으니 다른방법을 찾아보자.)
        looktarget = FindObjectOfType<PlayerLookTarget>(true);
        for (int i = 0; i < array_NPC.Length; i++)
        {
            //위치와 모양을 변경시키면 될거같기도한데.. 일단 고민좀해보자..
            array_NPC[i].InitData(i); //npc 를 초기화 시킨다.
            array_NPC[i].onTalkDisableButton += () => 
            {
                talkController.ResetData();
                talkController.openTalkWindow = null;
                talkController.closeTalkWindow = null;
                talkController.onTalkClick = null;
                talkController.getTalkDataArray = null;
                talkController.LogManager.getLogTalkDataArray = null;
                actionUI.invisibleUI?.Invoke();
                talkController.IsTalking = true;
                isActionActive = false;
            }; 
            array_NPC[i].onTalkEnableButton += (npcId) =>
            {
                talkController.ResetData();
                talkController.openTalkWindow = () => questUIManager.OnQuestNpc();
                talkController.closeTalkWindow = () => questUIManager.initialize();
                currentNpcIndex = npcId;
                talkController.onTalkClick = () => array_NPC[currentNpcIndex];

                talkController.getTalkDataArray = (talkIndex) =>
                {
                    return talkController.TalkData.GetTalk(array_NPC[currentNpcIndex].TalkType, talkIndex);

                };
                talkController.LogManager.getLogTalkDataArray = (talkIndex) => {
                    return talkController.TalkData.GetLog(array_NPC[currentNpcIndex].TalkType, talkIndex);
                };
                actionUI.visibleUI?.Invoke();
                talkController.IsTalking = false;
                isActionActive = true;
            };

            array_NPC[i].InitQuestData(questScriptableGenerate.MainStoryQuestArray,
                                    questScriptableGenerate.KillcountQuestArray,
                                    questScriptableGenerate.GatheringQuestArray); //퀘스트 데이터 처리

        }
    }
}
