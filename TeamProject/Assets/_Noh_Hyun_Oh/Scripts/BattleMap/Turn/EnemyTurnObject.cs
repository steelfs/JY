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
    /// ���� ������ �ൿ�� ���γ������� üũ�ϱ����� ����
    /// </summary>
    int turnEndCheckValue = 0;
    public int TurnEndCheckValue => turnEndCheckValue;
    /// <summary>
    /// �׽�Ʈ�� ���� 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 10;
    /// <summary>
    /// ĳ���� �����ʹ� �ܺο��� �����ϱ⶧���� �ش� ��������Ʈ �����������
    /// </summary>
    public Func<BattleMapEnemyBase[]> initEnemy;

    public Action turnStart;

    BattleMap_Player_Controller bpc;

    CameraOriginTarget cot;
    /// <summary>
    /// ���� �������� ���ʱ�ȭ��Ű������ ã�ƿ���
    /// </summary>
    InitCharcterSetting battleMapEndAction;

    [SerializeField]
    GameObject bossPrefab;

    /// <summary>
    /// ������ �ʱ�ȭ �Լ� 
    /// </summary>
    public override void InitData()
    {
        cot = FindObjectOfType<CameraOriginTarget>(true);
        BattleMapEnemyBase[] enemyList = initEnemy?.Invoke(); //�ܺο��� ���� �迭�� ���Դ��� üũ
        battleMapEndAction = FindObjectOfType<InitCharcterSetting>();
        bpc = FindObjectOfType<BattleMap_Player_Controller>();  
        if (enemyList == null || enemyList.Length == 0) //���� �ʱ�ȭ�� �ȵ������� 
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
            //�׽�Ʈ ������ ����
            for (int i = 0; i < testPlayerLength; i++)//ĳ���͵� �����ؼ� ���� 
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

                go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //������ ���� 
                go.transform.position = go.CurrentTile.transform.position; //���õ� Ÿ����ġ�� �̵���Ų��.
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
                        //Debug.Log("�������� �������̵��ϵ� ���ϵ� ó��");
                        //���� �۾����ΰ͵� �ڷ�ƾ���� ���� ����ٵ��Ŀ� �ʱ�ȭ ������ ����Ǿ��Ѵ�.
                        StopAllCoroutines();
                        StartCoroutine(BattleMapEnd()) ;
                    }

                };

                go.onCameraTarget = () => cot.Target = go.transform;

                go.onActionEndCheck = CheckTurnEnd; //������ �ൿ �������� üũ�ϴ� �Լ�����

                go.EnemyData.OnInit();
            }
            //���� �߰��� �ؿ� ��ɿ���
            //if (SpaceSurvival_GameManager.Instance.IsBoss)
            //{
            //    BattleMapEnemyBase go = Instantiate(bossPrefab).GetComponent<BattleMapEnemyBase>(); 
            //    charcterList.Add(go);
            //    go.GetCurrentTile = () => (SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster)); //������ ���� 
            //    go.transform.position = go.CurrentTile.transform.position; //���õ� Ÿ����ġ�� �̵���Ų��.
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
            //    go.onActionEndCheck = CheckTurnEnd; //������ �ൿ �������� üũ�ϴ� �Լ�����
            //}    
        }
        else // �ܺο��� �����Ͱ� ���������  �̰�찡 �������ΰ���  ���� ���̼� ���þ��Ұ��̱⶧����...
        {
            foreach (BattleMapEnemyBase enemy in enemyList)
            {
                charcterList.Add(enemy); //�ϰ����� �� ����
            }
        }

        SpaceSurvival_GameManager.Instance.GetEnemeyTeam = () => charcterList.OfType<BattleMapEnemyBase>();
    }

    /// <summary>
    /// ���Ͱ� ���� �׾����� ������ �̵���Ű�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator BattleMapEnd()
    {
        yield return null;
        WindowList.Instance.BattleMapClearUI.gameObject.SetActive(true);
        WindowList.Instance.BattleMapClearUI.SetRewordText();
       
    }

    /// <summary>
    /// ���� �ൿ�������� üũ�ؼ� �������Ű�� �Լ� 
    /// </summary>
    private void CheckTurnEnd()
    {
        turnEndCheckValue++;
        if (turnEndCheckValue == charcterList.Count) //��� �ൿ�̳������� 
        {
            //Debug.Log($"�����ϳ� �ൿ�� :{TurnActionValue}");
            turnEndCheckValue = 0;  // üũ�������� �ʱ�ȭ 
            TurnEndAction();    //������
        }

    }


    Tile playerTileIndex;
    
    public override void TurnStartAction()
    {
        // ù�ε��� ����Ÿ�̹־ȸ��� 
        BattleMapEnemyBase enemyData;
        foreach (var item in charcterList)
        {
            enemyData = (BattleMapEnemyBase)item;
            enemyData.BattleUI.TrunActionStateChange(); //�Ͻ��۽� �����̻� ���� ������ �����Ų��
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
