using System;
using UnityEngine;
using UnityEngine.UI;

public class StateObject_PoolObj : Base_PoolObj, IStateData
{
    /// <summary>
    /// 상태이상의 종류
    /// </summary>
    //StateType stateType;
    //public StateType Type 
    //{
    //    get => stateType;
    //    set 
    //    {
    //        stateType = value;
    //    }
    //}

    SkillData skillData;
    public SkillData SkillData { get=> skillData; set => skillData = value; }
   
    [SerializeField]
    [Range(0.0f,1.0f)]
    /// <summary>
    /// 한턴당 감소되는 수치 
    /// </summary>
    private float reducedDuration = 0.5f;
    public float ReducedDuration { get => reducedDuration; set => reducedDuration = value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// 상태이상의 최대 지속시간
    /// </summary>
    private float maxDuration = 1.0f;
    public float MaxDuration { get => maxDuration; set => maxDuration =value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// 남은 지속시간
    /// </summary>
    private float currentDuration = 0.0f;
    /// <summary>
    /// 0에서 부터 증가하면서 MaxDuration 값보다 크게 되면 해제된다.
    /// </summary>
    public float CurrentDuration { 
        get => currentDuration;

        set 
        {
            if (currentDuration != value) //값이 변경됫으면
            {
                if (value < 0.0f) 
                {
                    currentDuration = 0.0f; 

                }
                else if (value > MaxDuration)
                {
                    currentDuration = maxDuration;
                }
                else 
                { 
                    currentDuration = value; //값변경하고 
                }
                FillAmoutSetting(currentDuration); //게이지 갱신
            }
            
        }
    }
    /// <summary>
    /// 게이지 보여줄 이미지
    /// </summary>
    Image gaugeImg;

    /// <summary>
    /// 아이콘 이미지
    /// </summary>
    Image iconImg;

    TeamBorderStateUI stateBoard;
    

    /// <summary>
    /// 나누기한번만하기위해 배율 미리계산할 변수
    /// </summary>
    float computationalScale = -1.0f;
    protected  override void Awake()
    {
        base.Awake();
        gaugeImg = GetComponent<Image>();
        iconImg =  transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// 초기화 함수 
    /// </summary>
    /// <param name="skillData">스킬에대한 정보</param>
    public void InitData(SkillData skillData) 
    {
        if (skillData is Skill_Blessing blessing)
        {
            reducedDuration = 1.0f; //한턴당 1씩감소
            maxDuration = blessing.TurnBuffCount;
            iconImg.sprite = blessing.skill_sprite;
            currentDuration = 0.0f;
            gaugeImg.fillAmount = 0.0f;
            computationalScale = (1 / MaxDuration); //배율 미리구해두기
            this.skillData = skillData;
            stateBoard = WindowList.Instance.TeamBorderManager.TeamStateUIs[0]; //귀찮아 하나만할거니 걍 0번연결
            stateBoard.AddState(skillData);
            return;
        }
        Debug.Log($" 스킬데이터 : {skillData} 는 상태정보가 아닙니다.");
    }
    /// <summary>
    /// UI 조절할 함수 
    /// </summary>
    /// <param name="value">조절될 값</param>
    private void FillAmoutSetting(float value) 
    {
        gaugeImg.fillAmount = computationalScale * value; //미리구해둔 배율로 셋팅 
        stateBoard.TrunActionValueSetting(gaugeImg.fillAmount);
    }
    /// <summary>
    /// 초기화 작업
    /// </summary>
    public void ResetData() 
    {
        //상태값 초기화하고 
        //Type = StateType.None; //상태이상 종류 초기화
        skillData = null;
        reducedDuration = -1.0f; //한턴당 진행될 값초기화 
        maxDuration = -1.0f; // 최대치 초기화
        currentDuration = -1.0f; //현재 진행값 초기화
        computationalScale = -1.0f;//배율 초기화
        gaugeImg.fillAmount = 1.0f;// 게이지 초기화 
        iconImg.sprite = null;     //이미지 초기화 
        stateBoard.RemoveState();
        transform.SetParent(poolTransform); //풀로 돌린다.
        gameObject.SetActive(false); //큐에 다시 넣기위해 비활성화 
        /*
         transform.SetParent 함수는 객체가 활성화 비활성화 교체되는시점에서 사용될경우 오류가난다.
         그래서 활성화체크를하여 SetParent 를 실행한다.
         에러확인 : 오브젝트가 활성화 상태일때 gameobject.SetActive(false)실행후 바로  transform.SetParent(부모) 를 실행하니 터진다.
         */
    }

   
}
