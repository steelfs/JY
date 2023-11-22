using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Quest_UI_Colum : MonoBehaviour ,IPointerClickHandler
{
    /// <summary>
    /// 퀘스트에대한 상태 정보
    /// </summary>
    [SerializeField]
    Quest_State quest_State = Quest_State.None;
    public Quest_State Quest_State 
    {
        get => quest_State;
        set 
        {
            if (quest_State != value)
            {
                quest_State = value;
                Set_State_UI(quest_State);
            }
        }
    }
    [SerializeField]
    bool isSelectedCheck = false;
    public bool IsSelectedCheck
    {
        get => isSelectedCheck;
        set 
        {
            isSelectedCheck = value;
            if (isSelectedCheck)
            {
                iconBackGroundImg.color = selected_Colum_Color;
            }
            else 
            {
                Set_State_UI(quest_State);
            }
        }
    }


    /// <summary>
    /// 퀘스트 아이콘 UI 백그라운드 이미지
    /// </summary>
    [SerializeField]
    Image iconBackGroundImg;

    /// <summary>
    /// 퀘스트 UI 백그라운드 이미지
    /// </summary>
    [SerializeField]
    Image backGroundImg;
    
    /// <summary>
    /// 스토리 상태일때 색상 
    /// </summary>
    [SerializeField]
    Color stroyType;
    
    /// <summary>
    /// 토벌 퀘스트 일때의 색상
    /// </summary>
    [SerializeField]
    Color killCountType;
    
    /// <summary>
    /// 수집 퀘스트 일때의 색상
    /// </summary>
    [SerializeField]
    Color gatheringType;



    /// <summary>
    /// 퀘스트 기본 상태일때의 색상값
    /// </summary>
    [SerializeField]
    Color quest_None_Color;

    /// <summary>
    /// 퀘스트 스타트 상태일때의 색상값
    /// </summary>
    [SerializeField]
    Color quest_Start_Color;

    /// <summary>
    /// 퀘스트 진행중일 때의 색상값 
    /// </summary>
    [SerializeField]
    Color quest_Cancel_Color;

    /// <summary>
    /// 퀘스트 클리어 상태의 색상값
    /// </summary>
    [SerializeField]
    Color quest_Complete_Color;

    /// <summary>
    /// 아이템 선택했을때 색상
    /// </summary>
    [SerializeField]
    Color selected_Colum_Color;

    /// <summary>
    /// 해당UI 가 가지고있는 퀘스트 정보  일단 나중에 사용할수도있어서 연결해놨다.
    /// </summary>
    Gyu_QuestBaseData thisQuestData;
    public Gyu_QuestBaseData ThisQuestData 
    {
        get => thisQuestData;
        set 
        {
            if (value != null) 
            {
                thisQuestData = value;
                Quest_State = thisQuestData.Quest_State;
                Set_Type_UI(ThisQuestData.QuestType);
            }
        }
    }

    /// <summary>
    /// 퀘스트 아이콘 
    /// </summary>
    [SerializeField]
    Image iconImg;

    /// <summary>
    /// 퀘스트 이름
    /// </summary>
    [SerializeField]
    TextMeshProUGUI questTitle;

    /// <summary>
    /// 보상 아이템 갯수
    /// </summary>
    [SerializeField]
    TextMeshProUGUI rewardItemCount;
    [SerializeField]
    Image rewardItemIcon;
    /// <summary>
    /// 보상 재화 갯수
    /// </summary>
    [SerializeField]
    TextMeshProUGUI rewardCoinCount;
    [SerializeField]
    Image rewardCoinIcon;

    /// <summary>
    /// 퀘스트 클릭시 이벤트 전송용 델리게이트
    /// </summary>
    public Action<Gyu_QuestBaseData,Quest_UI_Colum> onClick;

    ItemDataManager testDataManager;

    private void Awake()
    {
        testDataManager = FindObjectOfType<ItemDataManager>();  
        backGroundImg = GetComponent<Image>();
        iconBackGroundImg = transform.GetChild(0).GetComponent<Image>();
      
        Transform child = transform.GetChild(0);
        iconImg = child.GetChild(0).GetComponent<Image>();

        child = transform.GetChild(1);    
        questTitle = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        rewardItemCount = child.GetChild(1).GetComponentInChildren<TextMeshProUGUI>(true);
        rewardItemIcon  = child.GetChild(1).GetChild(0).GetComponent<Image>();


        rewardCoinCount = child.GetChild(2).GetComponentInChildren<TextMeshProUGUI>(true);
        rewardCoinIcon  = child.GetChild(2).GetChild(0).GetComponent<Image>();
        ResetData();
    }

    /// <summary>
    /// 데이터 셋팅용 함수 
    /// </summary>
    /// <param name="questData">퀘스트 데이터 </param>
    public void SetData(Gyu_QuestBaseData questData) 
    {
        ThisQuestData = questData;
        iconImg.sprite = questData.IconImage;
        questTitle.text = $"{questData.Title}";
        if (questData.RewardItem.Length > 1) //보상아이템이 한개이상이면  
        {
            rewardItemCount.text = $"외 {questData.RewardItem.Length}개";
        }
        else 
        {
            rewardItemCount.text = $"{questData.ItemCount[0]} 개";
        }
        //rewardItemIcon.sprite = GameManager.Itemdata.itemDatas[(int)questData.RewardItem[0]].itemIcon; //아이템순번이 이넘순번과같으니 그냥 인트로 변경해서 사용 
        rewardItemIcon.sprite = testDataManager.itemDatas[(int)questData.RewardItem[0]].itemIcon; //아이템순번이 이넘순번과같으니 그냥 인트로 변경해서 사용 

        //rewardCoinIcon.sprite = GameManager.Itemdata.itemDatas[0].itemIcon; //0번이 코인이니 코인아이콘 가져온다 
        rewardCoinIcon.sprite = testDataManager.itemDatas[0].itemIcon; //0번이 코인이니 코인아이콘 가져온다 
        rewardCoinCount.text = $"{questData.RewardCoin} G";

        gameObject.SetActive(true); //셋팅 끝낫으면 활성화시켜서 보여준다 
    }

    /// <summary>
    /// 퀘스트 상태에따른 화면에 보여줄 이미지색변경로직
    /// </summary>
    /// <param name="state">퀘스트 상태</param>
    private void Set_State_UI(Quest_State state) 
    {
        switch (state)
        {
            case Quest_State.None:
                iconBackGroundImg.color = quest_None_Color;
                break;
            case Quest_State.Quest_Start:
                iconBackGroundImg.color = quest_Start_Color;
                break;
            //case Quest_State.Quest_Cancel:
            //    iconBackGroundImg.color = quest_Cancel_Color;
                //break;
            case Quest_State.Quest_Complete:
                iconBackGroundImg.color = quest_Complete_Color;
                break;
            default:
                break;
        }
    }

    private void Set_Type_UI(QuestType type) 
    {
        switch (type)
        {
            case QuestType.Killcount:
                backGroundImg.color = killCountType;
                break;
            case QuestType.Gathering:
                backGroundImg.color = gatheringType;
                break;
            case QuestType.Story:
                backGroundImg.color = stroyType;
                break;
        }
    }

    /// <summary>
    /// UI 데이터 리셋용 함수
    /// </summary>
    public void ResetData() 
    {
        thisQuestData = null;
        iconImg.sprite = null;
        questTitle.text = "";
        rewardCoinCount.text = "";
        rewardItemCount.text = "";

        onClick = null;

        gameObject.SetActive(false);
    }


    /// <summary>
    /// 클릭이벤트 연결 
    /// </summary>
    /// <param name="_"></param>
    public void OnPointerClick(PointerEventData _)
    {
        //클릭
        onClick?.Invoke(thisQuestData,this);
        //Debug.Log($"클릭했어 상태는 :{quest_State} , 퀘스트는  :{thisQuestData}");
    }
}
