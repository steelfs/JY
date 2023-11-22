using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


/// <summary>
/// ��Ż üũ�� �̳�
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
/// ���ӿ��� �ʿ��� ������ �� ����� ����� ���� �޴��� Ŭ���� 
/// </summary>
public class SpaceSurvival_GameManager : ChildComponentSingeton<SpaceSurvival_GameManager>
{
    /// <summary>
    /// ���� �������� üũ�Һ��� 
    /// </summary>
    public bool IsBoss = false;

    /// <summary>
    /// �Լ����� ����� ��ġ���� ĳ���� ��ü
    /// </summary>
    Transform playerPos ;
    public Transform PlayerStartPos
    {
        get => playerPos;
        set => playerPos = value;
    }

    /// <summary>
    /// ��Ʋ�ʿ��� �̵��� �Լ������� ��ġ ���� 
    /// </summary>
    Vector3 shipStartPos = Vector3.zero;
    public Vector3 ShipStartPos 
    {
        get => shipStartPos;
        set => shipStartPos = value;
    }


    /// <summary>
    /// �÷��̾� ����Ʈ ���� ��Ƶα�
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
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���� 
    /// </summary>
    [SerializeField]
    Tile[] battleMap;
    public Tile[] BattleMap
    {
        get
        {
            if (battleMap == null || battleMap.Length == 0) //��Ʋ���� ���̾����� 
            {
                battleMap = GetBattleMapTilesData?.Invoke(); //��������Ʈ ��û�� ���� �޾ƿ������Ѵ�.
                //��Ʋ���� �ƴѰ�� ��������Ʈ�� ���� ���ø��ϴ� null �� ���� �ɼ����ִ�.
            }
            //battleMap ??= GetBattleMapTilesData?.Invoke(); //���� �ּ��� ���� �����̶�� �Ѵ� . (������)
            return battleMap;

        }
    }
    public Func<Tile[]> GetBattleMapTilesData;


    /// <summary>
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���ΰ��� 
    /// </summary>
    [SerializeField]
    int mapSizeX = -1;
    public int MapSizeX
    {
        get
        {
            if (mapSizeX < 0 && GetMapTileX != null) //�ʱⰪ�̸� 
            {
                mapSizeX = GetMapTileX();
            }
            return mapSizeX;
        }
    }
    public Func<int> GetMapTileX;

    /// <summary>
    /// ��Ʋ�� ���۽� ������ ���� Ÿ�� ���ΰ��� 
    /// </summary>
    [SerializeField]
    int mapSizeY = -1;
    public int MapSizeY
    {
        get
        {
            if (mapSizeY < 0 && GetMapTileY != null) //�ʱⰪ�̸� 
            {
                mapSizeY = GetMapTileY();
            }
            return mapSizeY;
        }
    }
    public Func<int> GetMapTileY;

    /// <summary>
    /// �÷��̾��� ���� ����� �����صд�.
    /// </summary>
    BattleMapPlayerBase[] playerTeam;
    public BattleMapPlayerBase[] PlayerTeam
    {
        get
        {
            //if (playerTeam == null) //������� ������ 
            //{
            //    playerTeam = GetPlayerTeam?.Invoke(); // ������ ��û�ؼ� �޾ƿ´�
            //}
            playerTeam ??= GetPlayerTeam?.Invoke(); // ���� �ּ� ����� ����(������)
            return playerTeam;
        }
    }
    public Func<BattleMapPlayerBase[]> GetPlayerTeam;

    /// <summary>
    /// ���� �ǽð� ��� �����´�.
    /// </summary>
    public Func<IEnumerable<BattleMapEnemyBase>> GetEnemeyTeam;

    /// <summary>
    /// �̵����� ǥ���ϴ� ������Ʈ �����´�.
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
    /// �̵� ����ǥ���ϴ� ���� �޾ƿ������� ��������Ʈ
    /// </summary>
    public Func<MoveRange> GetMoveRangeComp;


    /// <summary>
    /// ���� ���� ǥ���ϴ� ������Ʈ �����´�.
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
    /// ���� ����ǥ���ϴ� ���� �޾ƿ������� ��������Ʈ
    /// </summary>
    public Func<AttackRange> GetAttackRangeComp;

    /// <summary>
    /// ��Ʋ���� �ʱ�ȭ �Լ��� �̱����������� ���ٴ��� �ʱ⶧���� 
    /// �������� ������ ������ �׽õ����ʴ´� �׷��� Get �Ҷ� üũ �ʿ� 
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
    /// �������� Ŭ���� ���� 
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
    /// ���� �������� �������� ����� 
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
    /// ��Ʋ���� Ŭ���� ����
    /// </summary>
    bool isBattleMapClear = false;
    public bool IsBattleMapClear 
    {
        get => isBattleMapClear;
        set => isBattleMapClear = value;
    }

    /// <summary>
    /// �������� �����ϴ��� üũ�� Ÿ�� ����Ʈ
    /// </summary>
    List<Tile> itemTileList = new List<Tile>(); 
    public List<Tile> ItemTileList => itemTileList;

    /// <summary>
    /// ���ݹ����� ����ϰ� �̵������� �ٽ�ǥ���ϴ� �Լ� �ߺ����� ���̴°����־ ���λ���.
    /// </summary>
    public void To_AttackRange_From_MoveRange()
    {
        AttackRange.ClearLineRenderer(); //���ݹ��� �ʱ�ȭ�Ѵ�.
        AttackRange.isAttacRange = false;
        AttackRange.isSkillAndAttack = false;
        //�ٽ� �̵����� ǥ���Ѵ�.
        BattleMapPlayerBase player = (BattleMapPlayerBase)TurnManager.Instance.CurrentTurn.CurrentUnit;
        float moveSize = player.CharcterData.Player_Status.Stamina > player.MoveSize ? player.MoveSize : player.CharcterData.Player_Status.Stamina; //�̵��Ÿ����ϰ�
        MoveRange.MoveSizeView(player.CurrentTile, moveSize);//�̵�����ǥ�����ֱ� 
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
    /// ���߿� ����ȭ ������ �۵��� �ǰ� 
    /// </summary>
    /// <param name="currentTile">��ã���� ������ ������ġ</param>
    /// <param name="TargetTile"> ��ã���� ������ ������ ��ġ</param>
    /// <param name="moveSize">ã������ �̵����ɰŸ�</param>
    /// <returns>�̵����ɰŸ���ŭ�� ��ã�� Ÿ�Ϲ迭</returns>
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
    /// �κ��丮 �� �ɼ�â ��Ʈ�� Ȱ��ȭ
    /// </summary>
    public void OnUIControll()
    {
        InputSystemController.InputSystem.UI_Inven.Enable();
        InputSystemController.InputSystem.Options.Enable();

    }

    /// <summary>
    /// �κ��丮 �� �ɼ�â ��Ʈ�� ��Ȱ��ȭ 
    /// ���� �����ִ� â ��δݱ� 
    /// </summary>
    public void OffUIControll()
    {
        InputSystemController.InputSystem.UI_Inven.Disable();
        InputSystemController.InputSystem.Options.Disable();
        WindowList.Instance.PopupSortManager.CloseAllWindow(); //�����ִ� UI â ��δݱ�   
    }
}
