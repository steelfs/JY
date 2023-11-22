using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 기본적으로 복수로 생성되는 객체 정의
/// EnumList.MultipleFactoryObjectList 이곳에 객체추가될내용 같이추가하시면됩니다.
/// </summary>
public class Multiple_Factory : ChildComponentSingeton<Multiple_Factory>
{
    /// <summary>
    /// 저장화면에 보여질 오브젝트풀
    /// </summary>
    Pool_SaveData saveDataPool;

    /// <summary>
    /// 저장화면리스트 버튼들 
    /// </summary>
    Pool_SavePageButton savePageButtonPool;

    /// <summary>
    /// 턴진행상황 뿌려줄 풀
    /// </summary>
    Pool_TurnGaugeUnit turnGaugeUnitPool;

    /// <summary>
    /// 추적형 UI 생성 풀
    /// </summary>
    Pool_TrackingBattleUI trackingBattleUIPool;

    /// <summary>
    /// 상태 UI 생성 풀
    /// </summary>
    Pool_State statePool;

    /// <summary>
    /// 턴관리될 용 오브젝트 생성 풀
    /// </summary>
    Pool_BattleMapTurnUnit battleMapPlayerPool;

    /// <summary>
    /// 턴관리될 용 오브젝트 생성 풀
    /// </summary>
    Pool_BattleMapTurnUnit battleMapEnemyPool;

   
    /// <summary>
    /// 전투맵 플레이어 캐릭터 생성할풀 하나라서 딱히 필요없긴하다..
    /// </summary>
    Pool_PlayerUnit playerUnitPool;

   
    /// <summary>
    /// 상점에서의 아이템슬롯 생성용으로 사용할 풀
    /// </summary>
    Pool_MerchantItem merchantItemPool;


    /// <summary>
    /// 타일맵 풀 테스트용으로 사용하려고 넣어놧다 신경쓰지않아오됨
    /// </summary>
    TileMapPool tileMapPool;


    Pool_EnemyUnit size_S_Human_Enemy_Pool;
    Pool_EnemyUnit size_M_Human_Hunter_Enemy_Pool;
    Pool_EnemyUnit size_M_Human_Psionic_Enemy_Pool;
    Pool_EnemyUnit size_S_Robot_Enemy_Pool;
    Pool_EnemyUnit size_L_Robot_Enemy_Pool;


    /// <summary>
    /// 팩토리 생성시 초기화 함수
    /// </summary>
    /// <param name="scene">씬정보 딱히필요없음</param>
    /// <param name="mode">모드정보 딱히필요없음</param>
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
    /// 객체 생성하기
    /// </summary>
    /// <param name="type">객체종류</param>
    /// <returns>생성된 객체</returns>
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


