using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleMapEnemyBase : Base_PoolObj ,ICharcterBase 
{
    /// <summary>
    /// 몬스터는 컨트롤할수없으니 형식만 맞춰두자
    /// </summary>
    public bool IsControll { get; set; }

    /// <summary>
    /// 몬스터 정보 받아오기
    /// </summary>
    Enemy_ enemyData;
    public Enemy_ EnemyData => enemyData;

    public virtual bool IsMoveCheck { get; }

    /// <summary>
    /// UI 가 따라다닐 위치
    /// </summary>
    Transform cameraTarget;
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
    /// 추적형 UI 가 있는 캔버스 위치
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;

    /// <summary>
    /// 현재 자신의 위치의 타일
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


    public Func<Tile> GetCurrentTile { get; set ; }
    public Action<BattleMapEnemyBase> onDie;

    /// <summary>
    /// 행동력 혹은 이동가능 거리
    /// </summary>
    [SerializeField]
    protected float moveSize = 4.0f;
    public float MoveSize
    {
        get => moveSize;
        set => moveSize = value;
    }

    /// <summary>
    /// 이동 속도
    /// </summary>
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float moveSpeed = 3.0f;

    /// <summary>
    /// 이동후 대기시간
    /// </summary>
    [SerializeField]
    WaitForSeconds waitTime = new WaitForSeconds(0.5f);
    /// <summary>
    /// 행동끝났으면 신호보낼 델리게이트
    /// </summary>
    public Action onActionEndCheck;

    /// <summary>
    /// 락온할 카메라 셋팅하기
    /// </summary>
    public Action onCameraTarget;

    protected override void Awake()
    {
        base.Awake();
        cameraTarget = transform.GetChild(transform.childCount - 1);
        enemyData = GetComponentInChildren<Enemy_>();
        enemyData.on_Enemy_Stamina_Change += (stmValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.stmGaugeSetting(stmValue, enemyData.MaxStamina); //소모된 행동력 표시
            }
        };
        enemyData.on_Enemy_HP_Change += (hpValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.hpGaugeSetting(hpValue, enemyData.MaxHp); //소모된 행동력 표시
            }
            if (enemyData.HP < 0)
            {
                Die();
                ResetData();
            }
        };
     
    }

    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI 담을 캔버스위치
        InitUI();
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
        currentTile.ExistType = Tile.TileExistType.None; // 속성 돌리고 
        
        if(enemyData.GrapPosition.transform.childCount > 0)
        {
            GameObject temp = enemyData.GrapPosition.GetChild(0).gameObject;
            Destroy(temp);
        }

        currentTile = null; //타일 참조해제
        //턴 오브젝트 초기화
        transform.SetParent(poolTransform); //풀로 돌린다
        gameObject.SetActive(false); // 큐를 돌린다.
    }

   

    public void Defence(float damage, bool isCritical = false)
    {
        enemyData.onHit();
        float finalDamage = Mathf.Max(0, damage - enemyData.DefencePower);
        GameManager.EffectPool.GetObject(finalDamage, transform, isCritical);
        enemyData.HP -= finalDamage;
    }


    /// <summary>
    /// 캐릭터 이동용 반복자
    /// </summary>
    /// <param name="playerTile">플레이어 위치가있는 타일</param>
    /// <returns></returns>
    public IEnumerator CharcterMove(Tile playerTile)
    {
        List<Tile> path = Cho_BattleMap_Enemy_AStar.PathFind(
                                                         SpaceSurvival_GameManager.Instance.BattleMap,
                                                         SpaceSurvival_GameManager.Instance.MapSizeX,
                                                         SpaceSurvival_GameManager.Instance.MapSizeY,
                                                         this.currentTile,
                                                         playerTile,
                                                         moveSize
                                                         );

        Vector3 targetPos = currentTile.transform.position; //길이없는경우 현재 타일위치 고정
                                                            //unitAnimator.SetBool(isWalkingHash, true); //이동애니메이션 재생 시작
        //onCameraTarget?.Invoke();   //이동할때 카메라 가져오기
        yield return waitTime;
        
        //foreach (Tile tile in path) //몬스터 중복 방지 용으로 타일값 미리셋팅해서 체크하자 
        //{
        //    tile.ExistType = Tile.TileExistType.Monster;//이것이 실행됬으면 밑에로직은 무조건 실행되야 정상동작된다.
        //}

        foreach (Tile tile in path)  // 길이있는경우 
        {
            float timeElaspad = 0.0f;
            enemyData.Move();
            targetPos = tile.transform.position; //새로운 위치잡고 
            transform.GetChild(0).transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //해당방향 바라보고 
            this.currentTile.ExistType = Tile.TileExistType.None;
            //Debug.Log($"{this.currentTile.Index}타일 오브젝트 이동중에 타일 데이터일단 move로변경");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}타일 이 데이터가 변경되야된다 charcter 로 ");
            tile.ExistType = Tile.TileExistType.Monster;//이것이 실행됬으면 밑에로직은 무조건 실행되야 정상동작된다.

            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //이동시작
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
        }

        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        //unitAnimator.SetBool(isWalkingHash, false);

        enemyData.Stop();

        if (IsAttackRange()) //공격할수있으면
        {
            yield return CharcterAttack(playerTile);// 공격 
        }

        onActionEndCheck?.Invoke(); //행동끝났으면 신호보내기
    }

    void Die()
    {
        GameManager.PlayerStatus.GetExp((uint)enemyData.EnemyExp);
        GameManager.Item_Spawner.SpawnItem(this);
        onDie?.Invoke(this);
    }

    /// <summary>
    /// 공격 하는 반복자 
    /// </summary>
    public IEnumerator CharcterAttack(Tile attackTile)
    {
        //Debug.Log($"{enemyData.name} - {enemyData.wType} - {enemyData.mType} - {enemyData.AttackPower}");
        transform.GetChild(0).transform.rotation = Quaternion.LookRotation(attackTile.transform.position - transform.position);
        if(enemyData.wType == Enemy_.WeaponType.Riffle && enemyData.mType != Monster_Type.Size_L)
            GameManager.EffectPool.GetObject(SkillType.Penetrate, attackTile.transform.position);
        enemyData.Attack_Enemy(SpaceSurvival_GameManager.Instance.PlayerTeam[0].CharcterData);
        yield return waitTime; //공격 애니메이션 끝날때까지 기다려주는것도 좋을거같다.
    }
    
    /// <summary>
    /// 공격범위 내에 있는지 체크하는 함수
    /// </summary>
    /// <returns>공격가능 true 불가능 false</returns>
    public bool IsAttackRange()
    {
        return Cho_BattleMap_Enemy_AStar.SetEnemyAttackSize(currentTile, enemyData.AttackRange); //공격범위 체크 
    }

}
