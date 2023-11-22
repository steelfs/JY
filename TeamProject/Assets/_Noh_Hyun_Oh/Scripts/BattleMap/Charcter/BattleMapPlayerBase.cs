using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleMapPlayerBase : Base_PoolObj, ICharcterBase
{
    //public static BattleMapPlayerBase instance;
    /// <summary>
    /// 현재 캐릭이 컨트롤할수있는상태인지 체크
    /// </summary>
    bool isControll = false;
    public bool IsControll
    {
        get => isControll;
        set => isControll = value;
    }

    /// <summary>
    /// 캐릭터 데이터 연동용 변수
    /// </summary>
    Player_ charcterData;
    public Player_ CharcterData => charcterData;

    /// <summary>
    /// 캐릭터 모델링의 목덜미 를 가리키고 있는 오브젝트의 위치 
    /// 카메라 의 타겟및 UI 위치 설정할때 사용한다.
    /// </summary>
    Transform cameraTarget;

    /// <summary>
    /// 이동버그가 존재해서 체크하는 변수
    /// </summary>
    bool isMoveCheck = false;
    public bool IsMoveCheck => isMoveCheck;

    /// <summary>
    /// 추적형 UI 
    /// </summary>
    private TrackingBattleUI battleUI = null;
    public TrackingBattleUI BattleUI
    {
        get => battleUI;
        set => battleUI = value;

    }
    /// <summary>
    /// 현재 내위치에있는 타일
    /// </summary>
    public Tile currentTile;
    public Tile CurrentTile
    {
        get
        {
            if (currentTile == null)
            {
                currentTile = GetCurrentTile?.Invoke();
            }
            return currentTile;
        }
    }

    public Func<Tile> GetCurrentTile { get; set; }
    
    /// <summary>
    /// 추적형 UI 가 있는 캔버스 위치
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;


    /// <summary>
    /// 행동력 혹은 이동 거리
    /// </summary>
    [SerializeField]
    float moveSize = 5.0f;
    
    public float MoveSize
    {
        get => moveSize;
        set => moveSize = value;
    }

    /// <summary>
    /// 이동용 애니메이션 
    /// </summary>
    [SerializeField]
    Animator unitAnimator;

    /// <summary>
    /// 애니메이션 이름값 미리 캐싱
    /// </summary>
    int isWalkingHash = Animator.StringToHash("IsWalking");

    /// <summary>
    /// 이동 속도 
    /// </summary>
    [Range(0.0f,10.0f)]
    [SerializeField]
    float moveSpeed = 3.0f;


    /// <summary>
    /// 좌측상단에있는 캐릭터 상태창
    /// </summary>
    UICamera viewPlayerCamera;

    public Action<Tile,float> onMoveRangeClear;

    protected override void Awake()
    {
        base.Awake();
        charcterData = GetComponentInChildren<Player_>();
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
        cameraTarget = transform.GetChild(transform.childCount-1); //마지막 위치에 있어야함
    }

    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI 담을 캔버스위치
        TeamBorderStateUI uiComp = WindowList.Instance.TeamBorderManager.TeamStateUIs[0];
        Base_Status playerData = GameManager.PlayerStatus.Base_Status;

        playerData.on_MaxExp_Change += (maxExpValue) => {
            uiComp.SetExpGaugeAndText(0.0f, maxExpValue);

        };

        playerData.on_ExpChange += (expValue) => {
            uiComp.SetExpGaugeAndText(expValue, playerData.ExpMax);

        };
        uiComp.SetExpGaugeAndText(playerData.Exp, playerData.ExpMax);

        playerData.on_CurrentStamina_Change += (stmValue) =>
        {
            //Debug.Log(stmValue);
            uiComp.SetStmGaugeAndText(stmValue, playerData.Base_MaxStamina);
            float currentMoveSize = stmValue > moveSize ? moveSize : stmValue;
            //moveSize = stmValue;
            if (TurnManager.Instance.CurrentTurn != null) 
            {
                TurnManager.Instance.CurrentTurn.TurnActionValue = stmValue;
            }
            if (battleUI != null)
            {
                BattleUI.stmGaugeSetting(stmValue, playerData.Base_MaxStamina); //소모된 행동력 표시
            }
            onMoveRangeClear?.Invoke(currentTile, currentMoveSize);
            if (TurnManager.Instance.TurnIndex > 0 &&  stmValue < 1.0f) //최소행동값? 보다 낮으면 
            {
                TurnManager.Instance.CurrentTurn.TurnEndAction();//턴종료 
            }
        };
      
        playerData.on_CurrentHP_Change += (hpValue) =>
        {
            uiComp.SetHpGaugeAndText(hpValue, playerData.Base_MaxHP);
            if (battleUI != null)
            {
                BattleUI.hpGaugeSetting(hpValue, playerData.Base_MaxHP); //소모된 행동력 표시
            }
        };

        charcterData.on_Buff_Start += (buffValue) =>
        {
            battleUI.AddOfStatus(buffValue);
        };
        StartCoroutine(rateDisable());
    }

    /// <summary>
    /// 초기화 후 큐로 돌린다
    /// </summary>
    /// <returns></returns>
    IEnumerator rateDisable() 
    {
        yield return null;
        //start 로직이끝난뒤 업데이트 첫번째에 비활성화 시켜서 다시 큐에 넣어버린다.
        ResetData();
    }

    protected override void OnEnable()
    {
        if (battleUICanvas != null)  //캔버스 위치를 찾아놨으면
        {
            InitUI();//초기화
        }
    }
   
    /// <summary>
    /// 추적형 UI 초기화 함수 셋팅
    /// </summary>
    public void InitUI()
    {
        if (battleUI != null) //값이 있으면
        {
            battleUI.gameObject.SetActive(true); //활성화만 시킨다
        }
        else //추적형 UI가 셋팅안되있으면 셋팅한다
        {
            battleUI = (TrackingBattleUI)Multiple_Factory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // 제일처음 초기화할때 배틀 UI 셋팅하고 
            battleUI.gameObject.name = $"{name} _ Tracking"; //이름확인용
            battleUI.transform.SetParent(battleUICanvas);//풀은 캔버스 밑에없기때문에 배틀맵UI만 관리할 캔버스 위치 밑으로 이동시킨다.
            battleUI.gameObject.SetActive(true); //활성화 시킨다.
            battleUI.FollowTarget = cameraTarget;     //UI 는 유닛과 1:1 매치가 되있어야 됨으로 담아둔다.
            battleUI.releaseStatus += (_) => { Debug.Log("버프해제"); charcterData.DeBuff(); }; //버프해제 등록
        }
        if (viewPlayerCamera == null)  //카메라 셋팅안되있으면 
        {
            viewPlayerCamera = EtcObjects.Instance.TeamCharcterView;// EtcObject 에 미리 만들어둔 게임오브젝트 가져오기 큐로 관리중이다 
            viewPlayerCamera.TargetObject = cameraTarget; //캐릭터안에 맨밑에서두번째 오브젝트를 카메라 타겟을 만들어둬야쫒아다닌다.
            viewPlayerCamera.gameObject.SetActive(true); //셋팅끝낫으면 활성화시키기
        }
    }

    /// <summary>
    /// 셋팅전의 값으로 돌리기
    /// 값을 초기화 시키고 풀로 돌리고 큐로 돌린다.
    /// </summary>
    public void ResetData()
    {
        if (BattleUI != null) //배틀 UI가 셋팅되있으면 
        {
            BattleUI.ResetData();// 추적형 UI 초기화 
            BattleUI = null; // 비우기
        }
        if (viewPlayerCamera != null)
        {
            viewPlayerCamera.TargetObject = null; //타겟 지우고
            viewPlayerCamera.gameObject.SetActive(false); // 비활성화 시키고 내부적으로 큐로 돌린다.
            viewPlayerCamera = null; //참조 지우기
        }
        if (currentTile != null) 
        {
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentTile); //이동범위  리셋시키고 
            currentTile.ExistType = Tile.TileExistType.None; // 속성 돌리고 
            currentTile = null; //타일 참조해제
        }
        //턴 오브젝트 초기화
        transform.SetParent(poolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
    }

   
    public void SetTile(Tile currentTile) 
    {
        this.currentTile = currentTile;
    } 
  

    /// <summary>
    /// 승근씨가 짜둔 길찾기 가져오기
    /// 
    /// 이동버그 존재함 
    /// - 어떠한 상황에서 발생하는지는 파악이안되나 타일의 값이 charcter 로 셋팅이안되는 상황이 발생 
    ///   이동시 해당로직에서 데이터를 바꾸고있기때문에 여기인거같은데 정확하게 파악을 못하고있음. 
    ///  해결  : 이동범위표시할때 초기화 하는로직에서 꼬였었음 
    /// </summary>
    /// <param name="path">A스타 최단거리 타일리스트</param>
    /// <returns></returns>
    public IEnumerator CharcterMove(Tile moveTile)
    {
        isMoveCheck = true; //이동 중인지 체크하기 

        List<Tile> path = Cho_BattleMap_AStar.PathFind(
                                                            SpaceSurvival_GameManager.Instance.BattleMap,
                                                            SpaceSurvival_GameManager.Instance.MapSizeX,
                                                            SpaceSurvival_GameManager.Instance.MapSizeY,
                                                            currentTile,
                                                            moveTile
                                                            );

        Vector3 targetPos = currentTile.transform.position; //길이없는경우 현재 타일위치 고정
        unitAnimator.SetBool(isWalkingHash, true); //이동애니메이션 재생 시작
        foreach (Tile tile in path)  // 길이있는경우 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //새로운 위치잡고 
            transform.GetChild(0).transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //해당방향 바라보고 
            currentTile.ExistType = Tile.TileExistType.Move;// 기존위치 이동가능하게 바꾸고  
            //Debug.Log($"{this.currentTile.Index}타일 오브젝트 이동중에 타일 데이터일단 move로변경");
            currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}타일 이 데이터가 변경되야된다 charcter 로 ");
            tile.ExistType = Tile.TileExistType.Charcter; //이동한위치 못가게 바꾼다.
            //Debug.Log($"{this.currentTile.Index}타일 이 데이터가 charcter 변경되었다.");
            
            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //이동시작
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, false);

        GameManager.PlayerStatus.Base_Status.Current_Stamina -= this.currentTile.MoveCheckG; //최종이동한 거리만큼 스태미나를 깍는다.

        isMoveCheck = false; //이동끝낫는지 체크
    }

    /// <summary>
    /// 공격하는 코루틴에 사용할 반복자
    /// </summary>
    /// <param name="selectedTile">공격할 위치</param>
    /// <returns></returns>
    public IEnumerator CharcterAttack(Tile selectedTile)
    {
        yield return null;
    }

    /// <summary>
    /// 공격범위내에 있는지 체크용 함수 
    /// 
    /// </summary>
    /// <returns>공격가능한범위면 true  불가능하면 false</returns>
    public bool IsAttackRange()
    {
        return false;
    }
}
