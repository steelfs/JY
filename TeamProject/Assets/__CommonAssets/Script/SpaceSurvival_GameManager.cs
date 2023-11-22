using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


/// <summary>
/// 포탈 체크용 이넘
/// </summary>
[Flags]
public enum StageList :byte
{
    None = 0,
    stage1 = 1, 
    stage2 = 2, 
    stage3 = 4,
    All = stage1 | stage2 | stage3  
}
/// <summary>
/// 게임에서 필요한 데이터 및 공통된 기능을 담을 메니저 클래스 
/// </summary>
public class SpaceSurvival_GameManager : ChildComponentSingeton<SpaceSurvival_GameManager>
{
    /// <summary>
    /// 보스 전투인지 체크할변수 
    /// </summary>
    public bool IsBoss = false;

    /// <summary>
    /// 함선에서 저장시 위치잡기용 캐싱할 객체
    /// </summary>
    Transform playerPos ;
    public Transform PlayerStartPos
    {
        get => playerPos;
        set => playerPos = value;
    }

    /// <summary>
    /// 배틀맵에서 이동시 함선에서의 위치 잡기용 
    /// </summary>
    Vector3 shipStartPos = Vector3.zero;
    public Vector3 ShipStartPos 
    {
        get => shipStartPos;
        set => shipStartPos = value;
    }


    /// <summary>
    /// 플레이어 퀘스트 정보 담아두기
    /// </summary>
    PlayerQuest_Gyu playerQuest;
    public PlayerQuest_Gyu PlayerQuest 
    {
        get 
        {
            if (playerQuest == null) 
            {
                playerQuest = getPlayerQuest?.Invoke();
            }
            return playerQuest;
        }
    }
    public Func<PlayerQuest_Gyu> getPlayerQuest;

    /// <summary>
    /// 배틀맵 시작시 셋팅할 맵의 타일 변수 
    /// </summary>
    [SerializeField]
    Tile[] battleMap;
    public Tile[] BattleMap
    {
        get
        {
            if (battleMap == null || battleMap.Length == 0) //배틀맵의 값이없으면 
            {
                battleMap = GetBattleMapTilesData?.Invoke(); //델리게이트 요청해 값을 받아오도록한다.
                //배틀맵이 아닌경우 델리게이트가 값을 셋팅못하니 null 이 셋팅 될수도있다.
            }
            //battleMap ??= GetBattleMapTilesData?.Invoke(); //위의 주석과 같은 내용이라고 한다 . (복합형)
            return battleMap;

        }
    }
    public Func<Tile[]> GetBattleMapTilesData;


    /// <summary>
    /// 배틀맵 시작시 셋팅할 맵의 타일 가로갯수 
    /// </summary>
    [SerializeField]
    int mapSizeX = -1;
    public int MapSizeX
    {
        get
        {
            if (mapSizeX < 0 && GetMapTileX != null) //초기값이면 
            {
                mapSizeX = GetMapTileX();
            }
            return mapSizeX;
        }
    }
    public Func<int> GetMapTileX;

    /// <summary>
    /// 배틀맵 시작시 셋팅할 맵의 타일 세로갯수 
    /// </summary>
    [SerializeField]
    int mapSizeY = -1;
    public int MapSizeY
    {
        get
        {
            if (mapSizeY < 0 && GetMapTileY != null) //초기값이면 
            {
                mapSizeY = GetMapTileY();
            }
            return mapSizeY;
        }
    }
    public Func<int> GetMapTileY;

    /// <summary>
    /// 플레이어의 팀원 목록을 저장해둔다.
    /// </summary>
    BattleMapPlayerBase[] playerTeam;
    public BattleMapPlayerBase[] PlayerTeam
    {
        get
        {
            //if (playerTeam == null) //팀목록이 없으면 
            //{
            //    playerTeam = GetPlayerTeam?.Invoke(); // 델리를 요청해서 받아온다
            //}
            playerTeam ??= GetPlayerTeam?.Invoke(); // 위의 주석 내용과 같음(복합형)
            return playerTeam;
        }
    }
    public Func<BattleMapPlayerBase[]> GetPlayerTeam;

    /// <summary>
    /// 적군 실시간 목록 가져온다.
    /// </summary>
    public Func<IEnumerable<BattleMapEnemyBase>> GetEnemeyTeam;

    /// <summary>
    /// 이동범위 표시하는 컴포넌트 가져온다.
    /// </summary>
    MoveRange moveRange;
    public MoveRange MoveRange
    {
        get
        {
            if (moveRange == null)
            {
                moveRange = GetMoveRangeComp?.Invoke();
            }
            return moveRange;
        }

    }
    /// <summary>
    /// 이동 범위표시하는 로직 받아오기위한 델리게이트
    /// </summary>
    public Func<MoveRange> GetMoveRangeComp;


    /// <summary>
    /// 공격 범위 표시하는 컴포넌트 가져온다.
    /// </summary>
    AttackRange attackRange;
    public AttackRange AttackRange
    {
        get
        {
            if (attackRange == null)
            {
                attackRange = GetAttackRangeComp?.Invoke();
            }
            return attackRange;
        }

    }
    /// <summary>
    /// 공격 범위표시하는 로직 받아오기위한 델리게이트
    /// </summary>
    public Func<AttackRange> GetAttackRangeComp;

    /// <summary>
    /// 배틀맵의 초기화 함수는 싱글톤형식으로 들고다니지 않기때문에 
    /// 전역으로 관리할 변수에 항시들어가지않는다 그래서 Get 할때 체크 필요 
    /// </summary>
    InitCharcterSetting battleMapInitClass;
    public InitCharcterSetting BattleMapInitClass
    {
        get
        {
            if (battleMapInitClass == null)
            {
                battleMapInitClass = GetBattleMapInit?.Invoke();
            }
            return battleMapInitClass;
        }
    }
    public Func<InitCharcterSetting> GetBattleMapInit;

    /// <summary>
    /// 스테이지 클리어 여부 
    /// </summary>
    [SerializeField]
    StageList stageClear = StageList.None;
    public StageList StageClear
    {
        get => stageClear;
        set
        {
            stageClear = value;
        }
    }

    /// <summary>
    /// 현재 진행중인 스테이지 저장용 
    /// </summary>
    [SerializeField]
    StageList currentClear = StageList.None;
    public StageList CurrentStage
    {
        get => currentClear;
        set
        {
            currentClear = value;
        }
    }
    /// <summary>
    /// 배틀맵의 클리어 여부
    /// </summary>
    bool isBattleMapClear = false;
    public bool IsBattleMapClear 
    {
        get => isBattleMapClear;
        set => isBattleMapClear = value;
    }

    /// <summary>
    /// 아이템이 존재하는지 체크할 타일 리스트
    /// </summary>
    List<Tile> itemTileList = new List<Tile>(); 
    public List<Tile> ItemTileList => itemTileList;

    /// <summary>
    /// 공격범위를 취소하고 이동범위를 다시표시하는 함수 중복으로 쓰이는곳이있어서 따로뺏다.
    /// </summary>
    public void To_AttackRange_From_MoveRange()
    {
        AttackRange.ClearLineRenderer(); //공격범위 초기화한다.
        AttackRange.isAttacRange = false;
        AttackRange.isSkillAndAttack = false;
        //다시 이동범위 표시한다.
        BattleMapPlayerBase player = (BattleMapPlayerBase)TurnManager.Instance.CurrentTurn.CurrentUnit;
        float moveSize = player.CharcterData.Player_Status.Stamina > player.MoveSize ? player.MoveSize : player.CharcterData.Player_Status.Stamina; //이동거리구하고
        MoveRange.MoveSizeView(player.CurrentTile, moveSize);//이동범위표시해주기 
    }

    public void ResetData(bool isLoadedBattleMap = false)
    {
        if (!isLoadedBattleMap)
        {
            battleMap = null;
            mapSizeX = -1;
            mapSizeY = -1;
            moveRange = null;
            attackRange = null;
            GetMoveRangeComp = null;
            GetAttackRangeComp = null;
            battleMapInitClass = null;
            GetBattleMapInit = null;
        }
        playerTeam = null;
        GetPlayerTeam = null;
        GetEnemeyTeam = null;
        IsBoss = false;
        playerPos = null;
        playerQuest = null;
        itemTileList.Clear();
    }

    /// <summary>
    /// 나중에 최적화 지금은 작동만 되게 
    /// </summary>
    /// <param name="currentTile">길찾기할 유닛의 현재위치</param>
    /// <param name="TargetTile"> 길찾기할 유닛의 도착할 위치</param>
    /// <param name="moveSize">찾은길의 이동가능거리</param>
    /// <returns>이동가능거리만큼의 길찾기 타일배열</returns>
    public Tile[] GetEnemyAstarTiles(Tile currentTile , Tile TargetTile , float moveSize)
    {
        List<Tile> tempList = Cho_BattleMap_Enemy_AStar.PathFind(battleMap, mapSizeX, mapSizeY, currentTile, TargetTile);
        Tile[] resultPath  = new Tile[(int)moveSize];
        for (int i = 0; i < resultPath.Length; i++)
        {
            resultPath[i] = tempList[i];
        }
        return resultPath;
    }

    /// <summary>
    /// 인벤토리 및 옵션창 컨트롤 활성화
    /// </summary>
    public void OnUIControll()
    {
        InputSystemController.InputSystem.UI_Inven.Enable();
        InputSystemController.InputSystem.Options.Enable();

    }

    /// <summary>
    /// 인벤토리 및 옵션창 컨트롤 비활성화 
    /// 기존 열려있는 창 모두닫기 
    /// </summary>
    public void OffUIControll()
    {
        InputSystemController.InputSystem.UI_Inven.Disable();
        InputSystemController.InputSystem.Options.Disable();
        WindowList.Instance.PopupSortManager.CloseAllWindow(); //열려있는 UI 창 모두닫기   
    }
}
