using EnumList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;

public class EnemyTurnObject : TurnBaseObject
{
 
    /// <summary>
    /// 적군 유닛의 행동이 전부끝낫는지 체크하기위한 변수
    /// </summary>
    int turnEndCheckValue = 0;
    public int TurnEndCheckValue => turnEndCheckValue;
    /// <summary>
    /// 테스트용 변수 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 10;
    /// <summary>
    /// 캐릭터 데이터는 외부에서 셋팅하기때문에 해당 델리게이트 연결해줘야함
    /// </summary>
    public Func<BattleMapEnemyBase[]> initEnemy;

    public Action turnStart;

    BattleMap_Player_Controller bpc;

    CameraOriginTarget cot;
    /// <summary>
    /// 몬스터 다죽으면 맵초기화시키기위해 찾아오기
    /// </summary>
    InitCharcterSetting battleMapEndAction;

    [SerializeField]
    GameObject bossPrefab;

    /// <summary>
    /// 데이터 초기화 함수 
    /// </summary>
    public override void InitData()
    {
        cot = FindObjectOfType<CameraOriginTarget>(true);
        BattleMapEnemyBase[] enemyList = initEnemy?.Invoke(); //외부에서 몬스터 배열이 들어왔는지 체크
        battleMapEndAction = FindObjectOfType<InitCharcterSetting>();
        bpc = FindObjectOfType<BattleMap_Player_Controller>();  
        if (enemyList == null || enemyList.Length == 0) //몬스터 초기화가 안되있으면 
        {
            int enumStartValue = (int)EnumList.MultipleFactoryObjectList.SIZE_S_HUMAN_ENEMY_POOL;
            int enumEndValue = (int)EnumList.MultipleFactoryObjectList.SIZE_L_ROBOT_ENEMY_POOL+1;
            switch (SpaceSurvival_GameManager.Instance.CurrentStage)
            {
                case StageList.stage1:
                    enumStartValue = (int)EnumList.MultipleFactoryObjectList.SIZE_S_HUMAN_ENEMY_POOL;
                    enumEndValue = (int)EnumList.MultipleFactoryObjectList.SIZE_S_ROBOT_ENEMY_POOL+1;
                    break;
                case StageList.stage2:
                    enumStartValue = (int)EnumList.MultipleFactoryObjectList.SIZE_M_HUMAN_HUNTER_ENEMY_POOL;
                    enumEndValue = (int)EnumList.MultipleFactoryObjectList.SIZE_M_HUMAN_PSIONIC_ENEMY_POOL+1;
                    break;
                case StageList.stage3:
                    enumStartValue = (int)EnumList.MultipleFactoryObjectList.SIZE_M_HUMAN_HUNTER_ENEMY_POOL;
                    enumEndValue = (int)EnumList.MultipleFactoryObjectList.SIZE_L_ROBOT_ENEMY_POOL+1;
                    break;
            }
            int randValue = 0;
            //테스트 데이터 생성
            for (int i = 0; i < testPlayerLength; i++)//캐릭터들 생성해서 셋팅 
            {
                float RandNumber = UnityEngine.Random.value;
                float num = randValue;
                randValue = UnityEngine.Random.Range(enumStartValue, enumEndValue);
                BattleMapEnemyBase go = (BattleMapEnemyBase)Multiple_Factory.Instance.GetObject(
                    (EnumList.MultipleFactoryObjectList)randValue);
                
                charcterList.Add(go);
                
                go.name = $"Enemy_{i}";
                if (RandNumber < 0.25f)
                    go.EnemyData.wType = Enemy_.WeaponType.Swrod;
                else if (RandNumber < 0.5f)
                    go.EnemyData.wType = Enemy_.WeaponType.Riffle;
                else
                    go.EnemyData.wType = go.EnemyData.wType;
                go.EnemyData.mType = go.EnemyData.mType;

                go.EnemyData.HP = go.EnemyData.MaxHp;

                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //데이터 연결 
                go.transform.position = go.CurrentTile.transform.position; //셋팅된 타일위치로 이동시킨다.
                go.onDie = (unit) => { 
                    charcterList.Remove(unit);
                    PlayerQuest_Gyu playerQuest = SpaceSurvival_GameManager.Instance.PlayerQuest;
                    foreach (var quest in playerQuest.CurrentQuests) 
                    {
                        if (quest.QuestType == QuestType.Killcount) 
                        {
                            int forSize = quest.QuestMosters.Length; 
                            for (int i = 0; i < forSize; i++)
                            {
                                if (unit.EnemyData.mType == quest.QuestMosters[i]) 
                                {
                                    quest.CurrentCount[i]++;
                                } 

                            }
                        }
                    }
                    
                    if (charcterList.Count < 1)
                    {
                        //Debug.Log("유닛전멸 마을로이동하든 뭘하든 처리");
                        //기존 작업중인것들 코루틴들이 전부 실행다된후에 초기화 로직이 실행되야한다.
                        StopAllCoroutines();
                        StartCoroutine(BattleMapEnd()) ;
                    }

                };

                go.onCameraTarget = () => cot.Target = go.transform;

                go.onActionEndCheck = CheckTurnEnd; //유닛의 행동 종료됬는지 체크하는 함수연결

                go.EnemyData.OnInit();
            }
            //보스 추가시 밑에 기능연결
            //if (SpaceSurvival_GameManager.Instance.IsBoss)
            //{
            //    BattleMapEnemyBase go = Instantiate(bossPrefab).GetComponent<BattleMapEnemyBase>(); 
            //    charcterList.Add(go);
            //    go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //데이터 연결 
            //    go.transform.position = go.CurrentTile.transform.position; //셋팅된 타일위치로 이동시킨다.
            //    go.onDie += (unit) =>
            //    {
            //        charcterList.Remove(unit);
            //        PlayerQuest_Gyu playerQuest = SpaceSurvival_GameManager.Instance.PlayerQuest;
            //        foreach (var quest in playerQuest.CurrentQuests)
            //        {
            //            if (quest.QuestType == QuestType.Story)
            //            {
            //                int forSize = quest.QuestMosters.Length;
            //                for (int i = 0; i < forSize; i++)
            //                {
            //                    if (unit.EnemyData.mType == quest.QuestMosters[i])
            //                    {
            //                        quest.CurrentCount[i]++;
            //                    }

            //                }
            //            }
            //        }
            //        StopAllCoroutines();
            //        StartCoroutine(BattleMapEnd());
            //    };
            //    go.onActionEndCheck = CheckTurnEnd; //유닛의 행동 종료됬는지 체크하는 함수연결
            //}    
        }
        else // 외부에서 데이터가 들어왔을경우  이경우가 정상적인경우다  내가 데이서 셋팅안할것이기때문에...
        {
            foreach (BattleMapEnemyBase enemy in enemyList)
            {
                charcterList.Add(enemy); //턴관리할 몹 셋팅
            }
        }

        SpaceSurvival_GameManager.Instance.GetEnemeyTeam = () => charcterList.OfType<BattleMapEnemyBase>();
    }

    /// <summary>
    /// 몬스터가 전부 죽었을때 마을로 이동시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator BattleMapEnd()
    {
        yield return null;
        WindowList.Instance.BattleMapClearUI.gameObject.SetActive(true);
        WindowList.Instance.BattleMapClearUI.SetRewordText();
       
    }

    /// <summary>
    /// 유닛 행동끝낫는지 체크해서 턴종료시키는 함수 
    /// </summary>
    private void CheckTurnEnd()
    {
        turnEndCheckValue++;
        if (turnEndCheckValue == charcterList.Count) //모든 행동이끝났으면 
        {
            //Debug.Log($"적군턴끝 행동력 :{TurnActionValue}");
            turnEndCheckValue = 0;  // 체크끝났으니 초기화 
            TurnEndAction();    //턴종료
        }

    }


    Tile playerTileIndex;
    
    public override void TurnStartAction()
    {
        // 첫로딩시 생성타이밍안맞음 
        BattleMapEnemyBase enemyData;
        foreach (var item in charcterList)
        {
            enemyData = (BattleMapEnemyBase)item;
            enemyData.BattleUI.TrunActionStateChange(); //턴시작시 상태이상 들을 게이지 진행시킨다
            enemyData.BattleUI.stmGaugeSetting(enemyData.EnemyData.Stamina, enemyData.EnemyData.MaxStamina);
            enemyData.BattleUI.hpGaugeSetting(enemyData.EnemyData.HP, enemyData.EnemyData.MaxHp);
        }


        StartCoroutine(TestC());
    }
    IEnumerator TestC() 
    {
        playerTileIndex = SpaceSurvival_GameManager.Instance.PlayerTeam[0].currentTile;

        int forSize = charcterList.Count;
        for (int i = 0; i < forSize; i++)
        {
            if (charcterList[i].IsAttackRange())
            {
                yield return charcterList[i].CharcterAttack(playerTileIndex);
                CheckTurnEnd();
                continue;
            }
            yield return charcterList[i].CharcterMove(playerTileIndex);
        }
    }
}
