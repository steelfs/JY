using EnumList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 추적형 UI 기능 클래스 
/// 상태이상 과 체력 관련 UI 기능 들어있는 컴퍼넌트
/// </summary>
public class TrackingBattleUI : Base_PoolObj
{
#if UNITY_EDITOR
    public bool isDebug = false; //디버그 표시해줄 체크박스 에디터에 추가하자
#endif

    /// <summary>
    /// 추적할 유닛
    /// </summary>
    [SerializeField]
    private Transform followTarget = null;
    public Transform FollowTarget
    {
        get => followTarget;
        set
        {
            followTarget = value;
        }

    }

    /// <summary>
    /// 현재 보여주고있는 카메라 거리측정용으로 사용된다 
    /// </summary>
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    /// <summary>
    /// 카메라와의 거리 최소값 이거리 이하가 되면 안보여줘야된다.
    /// </summary>
    float minDirection = 3.0f;
    [SerializeField]
    /// <summary>
    /// 카메라와의 거리 최대값 이거리 이상이 되면 안보여줘야된다
    /// </summary>
    float maxDirection = 20.0f;

    /// <summary>
    /// 카메라 타겟위치에서 Y 값으로 얼마나 떨어질지에대한 값 screen좌표 이다 x , y 
    /// </summary>
    [SerializeField]
    float uiDir = 30.0f;

    /// <summary>
    /// 카메라와 캐릭터간의 기본거리값 카메라가 타겟팅으로 잡고있는 캐릭터간의 거리 
    /// </summary>
    float defaultDir = 10.0f;

    /// <summary>
    /// 상태이상 한줄에 들어갈 갯수
    /// </summary>
    const int gridWidthLength = 4;

    /// <summary>
    /// 1기준으로 한줄에 들어가는 갯수를 나눈값
    /// 나눗셈을 한번만하기위한 꼼수 
    /// </summary>
    readonly float gridWidthScale = 1.0f / gridWidthLength;


    /// <summary>
    /// 상태이상이 들어갈 그리드레이아웃 그룹
    /// </summary>
    GridLayoutGroup glg;

    /// <summary>
    /// 위에보여줄 상태이상 위치값  
    /// </summary>
    RectTransform rtTop;

    /// <summary>
    /// 아래에 보여줄 체력및 스태미나 보여줄 위치값
    /// </summary>
    RectTransform rtBottom;

    /// <summary>
    /// 상태이상 위치값 담을 백터
    /// </summary>
    Vector2 topDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// 체력및 스태미나 위치값 담을 백터
    /// </summary>
    Vector2 bottomDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// 상태이상 크기조절용 백터
    /// </summary>
    Vector2 topDefaultDeltaSize = Vector2.zero;

    /// <summary>
    /// 체력 및 스태미나 크기조절용 백터
    /// </summary>
    Vector2 bottomDefaultSize = Vector2.zero;

    /// <summary>
    /// 상태이상의 크기변경을 위해 사용되는 백터
    /// </summary>
    Vector2 topGroupCellSize = Vector2.zero;

    /// <summary>
    /// 상태이상이 늘어날경우 그리드위치변경을위해 사용되는 백터
    /// </summary>
    Vector2 gridPos = Vector2.zero;

    /// <summary>
    /// 상태이상 UI 들어갈 그리드그룹 위치값
    /// </summary>
    Transform stateGroup;


    /// <summary>
    /// 지속시간이 지나거나 아이템을 사용해서 상태가 해제된 경우 호출
    /// 지금은 스킬데이터 안씀
    /// </summary>
    public Action<SkillData> releaseStatus;

    /// <summary>
    /// 상태이상 최대갯수 
    /// </summary>
    int stateSize = 4;
    int StateSize {
        get => stateSize;
        set
        {
            if (value > stateSize) //크기가 기본사이즈보다크면 
            {
                stateSize *= 2; //일단 두배로 늘린다
                IStateData[] temp = states; //기존값들을 백업해둔다.
                states = new IStateData[stateSize]; // 늘린사이즈만큼 새로 배열 만든다. 
                for (int i = 0; i < temp.Length; i++)
                {
                     states[i] = temp[i]; //기존값들을 다시 담는다.
                }
            }
        }
        
    }

    /// <summary>
    /// 현재 상태이상 상태 담아둘 배열
    /// </summary>
    private IStateData[] states;
    public IStateData[] States => states;

    private List<SkillData> stateSkillDatas;

    /// <summary>
    /// 체력 게이지 조절할 렉트
    /// </summary>
    private RectTransform hpRect;

    /// <summary>
    /// 스테미나 게이지 조절할 렉트
    /// </summary>
    private RectTransform stmRect;

    private float hp_UI_Value = 1.0f;   // 체력 게이지조절용 변수         현재 상태값
    private float stm_UI_Value = 1.0f;  // 스테미나 게이조절용 변수       현재 상태값

    private float change_HpValue = 0.0f;    //수정될 게이지 조절값
    private float change_StmValue = 0.0f;   //수정될 게이지 조절값

    /// <summary>
    /// 외부에서 호출해서 사용할 델리
    /// </summary>
    public Action<float, float> hpGaugeSetting;
    IEnumerator hpChangeCoroutine;

    /// <summary>
    /// 외부에서 호출해서 사용할 댈리
    /// </summary>
    public Action<float, float> stmGaugeSetting;
    IEnumerator stmChangeCoroutine;
    
    //게이지 조절속도 변수 
    [SerializeField]
    [Range(0.1f,2.0f)]
    float gaugeSpeed = 1.0f;


    CanvasGroup cg;

    /// <summary>
    /// 
    /// </summary>
    WaitForFixedUpdate uiGaugeSpeed = new();

    Slider hpSlider;
    Slider stmSlider;

    /// <summary>
    /// 초기값들을 셋팅해둔다 나중에 거리에따른 사이즈조절에 사용할값
    /// </summary>
    protected override  void Awake()
    {
        base.Awake();
        cg = GetComponent<CanvasGroup>();
        stateGroup = transform.GetChild(0);
        glg = stateGroup.GetComponent<GridLayoutGroup>();
        rtTop = stateGroup.GetComponent<RectTransform>();
        Transform tempChild = transform.GetChild(1);
        rtBottom = tempChild.GetComponent<RectTransform>();

        mainCamera = Camera.main;
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //카메라 찾아서 셋팅한다. - 시네머신으로 카메라 전환시 해당값을 교체하는 로직을 추가가 필요
        //맨처음 맞춰놓은 사이즈(기본)값을 저장해둔다.
        topGroupCellSize = glg.cellSize;
        //상태이상
        topDefaultAnchoredPosition = rtTop.anchoredPosition;
        topDefaultDeltaSize = rtTop.sizeDelta;
        //체력 스태미나
        bottomDefaultAnchoredPosition = rtBottom.anchoredPosition;
        bottomDefaultSize = rtBottom.sizeDelta;



        states = new IStateData[stateSize]; // 상태이상의 배열크기를 잡아둔다.
        stateSkillDatas = new(stateSize);

        stmRect = tempChild.GetChild(1).GetChild(0).GetComponent<RectTransform>(); // stm rect
        hpChangeCoroutine = HP_GaugeSetting();
        hpRect = tempChild.GetChild(2).GetChild(0).GetComponent<RectTransform>(); // hp rect
        stmChangeCoroutine = Stm_GaugeSetting();
        hpGaugeSetting = (now, max) => {
            //턴제라 실시간 처리는 배제하고 제작함. 공격한번에 1번만 수정되도록 호출이 필요
            //연타 도 1번의 데미지로 처리하도록 회복도 마찬가지
            change_HpValue = now/max;
            StopCoroutine(hpChangeCoroutine);
            hpChangeCoroutine = HP_GaugeSetting();
            StartCoroutine(hpChangeCoroutine);

        };
        stmGaugeSetting = (now, max) => {
            change_StmValue = now/max;
            StopCoroutine(stmChangeCoroutine);
            stmChangeCoroutine = Stm_GaugeSetting();
            StartCoroutine(stmChangeCoroutine);
        };




    }

    /// <summary>
    /// 활성화시 셋팅하기
    /// </summary>
    protected override void OnEnable()
    {
        InitTracking();
    }
   
    /// <summary>
    /// 배틀맵 시작시 초기화할 값들 셋팅 
    /// </summary>
    private void InitTracking() 
    {
        //mainCamera = Camera.main; //유니티에서 제공해주는걸 사용해보자 awake에서는 못가져온다. OnEnable 처음호출에서도못가져온다.
        StopAllCoroutines();//기존추적하던게있으면 멈추고 
        StartCoroutine(StartTracking()); //새로추척
    }
    /// <summary>
    /// 추적 시작
    /// </summary>
    IEnumerator StartTracking() 
    {
        while (true)
        {
            SetTrackingUI(); //캐릭터가 움직일때마다 UI 크기및 위치를 변경시켜줘야한다. 
            yield return null;

        }
    }
    /// <summary>
    /// 값이 0~ 1 
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator HP_GaugeSetting()
    {
        Vector2 tempVector = Vector2.zero; //rect transform 값변환시 사용할 변수
        if (change_HpValue > hp_UI_Value) //회복 
        {
            while (hp_UI_Value < change_HpValue) //들어온값보다 작으면 수치계속변경
            {
                hp_UI_Value += Time.deltaTime * gaugeSpeed; //부드럽게~
                RectUISetting(hpRect, hp_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(hpRect, change_HpValue);
        }
        else if (change_HpValue < hp_UI_Value) //데미지  
        {
            while (hp_UI_Value > change_HpValue)
            {
                hp_UI_Value -= Time.deltaTime * gaugeSpeed; //위와 동일한 기능 방향만 반대임
                RectUISetting(hpRect, hp_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(hpRect, change_HpValue);
        }
        else 
        {
            RectUISetting(hpRect, change_HpValue);
        }
    }
    /// <summary>
    /// 스테미나 UI 조절용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Stm_GaugeSetting()
    {
        if (change_StmValue > stm_UI_Value) //회복 
        {
            while (stm_UI_Value < change_StmValue) //들어온값보다 작으면 수치계속변경
            {
                stm_UI_Value += Time.deltaTime * gaugeSpeed; //부드럽게~
                RectUISetting(stmRect, stm_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(stmRect, change_StmValue);
        }
        else if (change_StmValue < stm_UI_Value) //데미지  
        {
            while (stm_UI_Value > change_StmValue)
            {
                stm_UI_Value -= Time.deltaTime * gaugeSpeed;
                RectUISetting(stmRect, stm_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(stmRect, change_StmValue);
        }
        else 
        {
            RectUISetting(stmRect, change_StmValue);
        }
    }
    /// <summary>
    /// Gauge UI 를 조절하기위한 함수
    /// </summary>
    /// <param name="uiRect">조절될 RectTransform</param>
    /// <param name="value">조절될 값</param>
    private void RectUISetting(RectTransform uiRect,float value) 
    {
        Vector2 tempVector = Vector2.zero;          //rect transform 값변환시 사용할 변수
        tempVector = uiRect.anchorMax;              //rect transform Anchors 값의 max 쪽 
        tempVector.x = value;                       // 그중에 x 값을 줄이면 됨
        uiRect.anchorMax = tempVector;              // right 수정용으로 받아오고 
        uiRect.offsetMax = Vector2.zero;            // right 값 0으로 수정해서 이미지 이동시키기
    }
    /// <summary>
    /// 카메라와 플레이어간의 거리를 재서 추적형 UI 크기를 조절시키는 함수
    /// </summary>
    private void SetTrackingUI()
    {
        if (FollowTarget != null ) //플레이어가 있을경우만 실행
        {
            Vector3 playerPosition = mainCamera.WorldToScreenPoint(FollowTarget.position); //플레이어 스크린좌표를 읽어온다.
            playerPosition.y += uiDir; //캐릭터위치정중앙에서 살짝위로 
            transform.position = playerPosition; //주적할 오브젝트의 위치를 쫒아간다.
            
#if UNITY_EDITOR
            if (isDebug) 
            {
                Debug.Log(playerPosition);
            }
#endif
            //UI보이는 거리 구간을 확인
            if (playerPosition.z < maxDirection && playerPosition.z > minDirection) //해당범위안에서만 보여준다. z기준으로만잡고있지만 추가해주면된다.
            {
                //기준으로 가까워지면 배율이 증가하고 멀어지면 배율이 감소하는 연산을 하고싶은데 곱하기로 하려면 if문이 엄청들어간다 
                float scale = defaultDir / playerPosition.z; //배율을 뽑기위해 곱하기연산으로 돌리고싶었지만 방법을 못찾앗다.
                                                             //초기값에서 배율로 곱해서 사이즈정한다.
                                                             ////배율별로 사이즈조절
                rtBottom.anchoredPosition = bottomDefaultAnchoredPosition * scale;
                rtBottom.sizeDelta = bottomDefaultSize * scale;

                ////상태이상
                glg.cellSize = topGroupCellSize * scale; //셀사이즈를 조절하고 
                gridPos = topDefaultAnchoredPosition * scale; //상태이상기본위치를 설정한뒤 
                //상태이상이 한줄에서 두줄 , 두줄에서 세줄 같이 변경될때를 위해 계산을한다.
                gridPos.y += (glg.cellSize.y * //셀사이즈만큼 간격을 추가한다 
                                (int)((transform.GetChild(0).childCount - 1) * gridWidthScale)); //한줄이 넘어가면 

                rtTop.anchoredPosition = gridPos; //수정된 값을 적용       
                
                rtTop.sizeDelta = topDefaultDeltaSize * scale; //상태이상의 크기를 조절

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true); //화면에 보여준다.
                }

            }
            else //벗어나면 
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false); //안보여준다.
                }
            }
        }
    }
   
    /// <summary>
    /// 한턴당 상태이상의 진행도를 감소시키는 함수  (수정하는함수)
    /// 턴메니져에서 일괄실행  - 테스트 진행중
    /// </summary>
    public void TrunActionStateChange() 
    {
        for (int i=0; i<  states.Length; i++) 
        {
            if (states[i] == null) continue; //빈값이면 다음으로 
            states[i].CurrentDuration += states[i].ReducedDuration; //값이 존재하면 상태이상 갱신
            if (states[i].CurrentDuration == states[i].MaxDuration) // 상태이상 지속시간이 끝났으면 
            {
                releaseStatus?.Invoke(states[i].SkillData);//상태해제됬다고 신호를 보낸다.
                stateSkillDatas.Remove(states[i].SkillData);
                states[i].ResetData();//상태 초기화 하고 
                states[i] = null; //배열에서 삭제

            }
        }

    }


    /// <summary>
    /// 현재 진행중인 상태이상 데이터 셋팅
    /// 배열에 추가하고 관리한다.
    /// </summary>
    /// <param name="addData">상태이상의 정보</param>
    public void AddOfStatus(SkillData skillData) 
    {
        if (stateSkillDatas.Contains(skillData)) //추가된 값이있으면 
        {
            foreach (IStateData state in states)
            {
                if (state != null && state.SkillData == skillData)
                {
                    ((StateObject_PoolObj)state).InitData(skillData);
                    return;
                }
            }
        }
        else 
        {
            for(int i=0; i< StateSize; i++)//전체검색해보고 
            {
                if (states[i] == null) //빈곳이있는경우 
                {
                    IStateData stateData = SettingStateUI(skillData);//풀에서 객체가져와서 UI셋팅
                    stateData.SkillData = skillData;
                    states[i] = stateData;//추가하고
                    stateSkillDatas.Add(skillData);
                    return;//빠져나간다.
                }
            }
        }
        //StateSize++; //상태리스트 꽉차있으면 배열 사이즈늘리고 
        //AddStateArray(addData); // 함수를 다시호출해서 배열에 추가시킨다.
    }

    /// <summary>
    /// 상태이상이 발동시 호출될 함수 
    /// 상태이상별로 UI처리관련 내용을 추가할예정
    /// </summary>
    /// <param name="skillData">스킬 정보</param>
    /// <returns>상태이상의 정보를 생성해서 반환</returns>
    private IStateData SettingStateUI(SkillData skillData)
    {
        StateObject_PoolObj poolObj = (StateObject_PoolObj)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.STATE_POOL); //풀에서 꺼내고
        poolObj.transform.SetParent(stateGroup);// 부모 셋팅하고 
        poolObj.InitData(skillData);    //스킬 데이터 넘겨서 초기화 함수실행
        return poolObj;

    }

    /// <summary>
    /// 초기화 함수
    /// 셋팅값 초기화및 풀로 돌리고걸려있는 상태이상 도 전부 초기화 
    /// 최종적으로 큐에 추가하고 풀로 돌려버린다.
    /// </summary>
    public  void ResetData() 
    {
      
        //델리게이트 초기화 
        releaseStatus = null;
        //거리재기위한 카메라와 기준점이될 플레이어 참조를 해제 
        //mainCamera = null;
        FollowTarget = null;
        for (int i = 0; i < StateSize; i++) //상태이상 내용을 전부
        {
            if (states[i] != null) //값이 들어 있는것들 찾아서
            {
                states[i].ResetData(); //내부값들 초기화및 풀로 돌리기작업 하고
                states[i] = null; // 빈값으로 셋팅
            }
        }
        RectUISetting(hpRect, 1.0f);
        RectUISetting(stmRect, 1.0f);
        transform.SetParent(poolTransform);//풀로 돌린다
        gameObject.SetActive(false); //큐로 돌리고 
        //초기화 마지막에 풀로 돌린다.

        
    }

    public void SetVisibleUI() 
    {
        cg.alpha = 1.0f;
    }

    public void SetInVisibleUI() 
    {
        cg.alpha = 0.0f;

    }


}
