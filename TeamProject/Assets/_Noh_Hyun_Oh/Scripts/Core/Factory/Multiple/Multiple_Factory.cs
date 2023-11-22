using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// �⺻������ ������ �����Ǵ� ��ü ����
/// EnumList.MultipleFactoryObjectList �̰��� ��ü�߰��ɳ��� �����߰��Ͻø�˴ϴ�.
/// </summary>
public class Multiple_Factory : ChildComponentSingeton<Multiple_Factory>
{
    /// <summary>
    /// ����ȭ�鿡 ������ ������ƮǮ
    /// </summary>
    Pool_SaveData saveDataPool;

    /// <summary>
    /// ����ȭ�鸮��Ʈ ��ư�� 
    /// </summary>
    Pool_SavePageButton savePageButtonPool;

    /// <summary>
    /// �������Ȳ �ѷ��� Ǯ
    /// </summary>
    Pool_TurnGaugeUnit turnGaugeUnitPool;

    /// <summary>
    /// ������ UI ���� Ǯ
    /// </summary>
    Pool_TrackingBattleUI trackingBattleUIPool;

    /// <summary>
    /// ���� UI ���� Ǯ
    /// </summary>
    Pool_State statePool;

    /// <summary>
    /// �ϰ����� �� ������Ʈ ���� Ǯ
    /// </summary>
    Pool_BattleMapTurnUnit battleMapPlayerPool;

    /// <summary>
    /// �ϰ����� �� ������Ʈ ���� Ǯ
    /// </summary>
    Pool_BattleMapTurnUnit battleMapEnemyPool;

   
    /// <summary>
    /// ������ �÷��̾� ĳ���� ������Ǯ �ϳ��� ���� �ʿ�����ϴ�..
    /// </summary>
    Pool_PlayerUnit playerUnitPool;

   
    /// <summary>
    /// ���������� �����۽��� ���������� ����� Ǯ
    /// </summary>
    Pool_MerchantItem merchantItemPool;


    /// <summary>
    /// Ÿ�ϸ� Ǯ �׽�Ʈ������ ����Ϸ��� �־�J�� �Ű澲���ʾƿ���
    /// </summary>
    TileMapPool tileMapPool;


    Pool_EnemyUnit size_S_Human_Enemy_Pool;
    Pool_EnemyUnit size_M_Human_Hunter_Enemy_Pool;
    Pool_EnemyUnit size_M_Human_Psionic_Enemy_Pool;
    Pool_EnemyUnit size_S_Robot_Enemy_Pool;
    Pool_EnemyUnit size_L_Robot_Enemy_Pool;


    /// <summary>
    /// ���丮 ������ �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="scene">������ �����ʿ����</param>
    /// <param name="mode">������� �����ʿ����</param>
    protected override void OnEnable()
    {
        base.OnEnable();
        saveDataPool = GetComponentInChildren<Pool_SaveData>(true);
        savePageButtonPool = GetComponentInChildren<Pool_SavePageButton>(true);
        turnGaugeUnitPool = GetComponentInChildren<Pool_TurnGaugeUnit>(true);
        trackingBattleUIPool = GetComponentInChildren<Pool_TrackingBattleUI>(true);
        statePool = GetComponentInChildren<Pool_State>(true);
        playerUnitPool = GetComponentInChildren<Pool_PlayerUnit>(true);
        merchantItemPool = GetComponentInChildren<Pool_MerchantItem>(true);
        saveDataPool.Initialize();
        savePageButtonPool.Initialize();
        turnGaugeUnitPool.Initialize();  
        trackingBattleUIPool.Initialize();
        statePool.Initialize();
        playerUnitPool.Initialize();
        merchantItemPool.Initialize();
        tileMapPool = GetComponentInChildren<TileMapPool>(true);
        tileMapPool.Initialize();

        Pool_BattleMapTurnUnit[] battleTurns = GetComponentsInChildren<Pool_BattleMapTurnUnit>(true);
        
        battleMapPlayerPool = battleTurns[0];
        battleMapPlayerPool.Initialize();
        
        battleMapEnemyPool = battleTurns[1];
        battleMapEnemyPool.Initialize();


        Transform child = transform.GetChild(transform.childCount - 2);
        size_S_Human_Enemy_Pool = child.GetChild(0).GetComponent<Pool_EnemyUnit>();
        size_M_Human_Hunter_Enemy_Pool = child.GetChild(1).GetComponent<Pool_EnemyUnit>();
        size_M_Human_Psionic_Enemy_Pool = child.GetChild(2).GetComponent<Pool_EnemyUnit>();


        child = transform.GetChild(transform.childCount - 1);

        size_S_Robot_Enemy_Pool = child.GetChild(0).GetComponent<Pool_EnemyUnit>();
        size_L_Robot_Enemy_Pool = child.GetChild(1).GetComponent<Pool_EnemyUnit>();


        size_S_Human_Enemy_Pool.Initialize();
        size_M_Human_Hunter_Enemy_Pool.Initialize();
        size_M_Human_Psionic_Enemy_Pool.Initialize();
        size_S_Robot_Enemy_Pool.Initialize();
        size_L_Robot_Enemy_Pool.Initialize();
      
    }

    /// <summary>
    /// ��ü �����ϱ�
    /// </summary>
    /// <param name="type">��ü����</param>
    /// <returns>������ ��ü</returns>
    public Base_PoolObj GetObject(EnumList.MultipleFactoryObjectList type)
    {
        Base_PoolObj obj = null;
        switch (type)
        {
            case EnumList.MultipleFactoryObjectList.SAVE_DATA_POOL:
                obj = saveDataPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.SAVE_PAGE_BUTTON_POOL:
                obj = savePageButtonPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.TURN_GAUGE_UNIT_POOL:
                obj = turnGaugeUnitPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL:
                obj = trackingBattleUIPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.STATE_POOL:
                obj = statePool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.BATTLEMAP_PLAYER_POOL:
                obj = battleMapPlayerPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL:
                obj = battleMapEnemyPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL:
                obj = playerUnitPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.TILE_POOL:
                obj = tileMapPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.MERCHANT_iTEM_POLL:
                obj = merchantItemPool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.SIZE_S_HUMAN_ENEMY_POOL:
                obj = size_S_Human_Enemy_Pool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.SIZE_S_ROBOT_ENEMY_POOL:
                obj = size_S_Robot_Enemy_Pool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.SIZE_M_HUMAN_PSIONIC_ENEMY_POOL:
                obj = size_M_Human_Psionic_Enemy_Pool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.SIZE_M_HUMAN_HUNTER_ENEMY_POOL:
                obj = size_M_Human_Hunter_Enemy_Pool?.GetObject();
                break;
            case EnumList.MultipleFactoryObjectList.SIZE_L_ROBOT_ENEMY_POOL:
                obj = size_L_Robot_Enemy_Pool?.GetObject();
                break;
            default:

                break;
        }
        return obj;
    }

}


