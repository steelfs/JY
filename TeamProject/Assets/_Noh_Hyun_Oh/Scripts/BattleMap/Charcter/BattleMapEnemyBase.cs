using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleMapEnemyBase : Base_PoolObj ,ICharcterBase 
{
    /// <summary>
    /// ���ʹ� ��Ʈ���Ҽ������� ���ĸ� �������
    /// </summary>
    public bool IsControll { get; set; }

    /// <summary>
    /// ���� ���� �޾ƿ���
    /// </summary>
    Enemy_ enemyData;
    public Enemy_ EnemyData => enemyData;

    public virtual bool IsMoveCheck { get; }

    /// <summary>
    /// UI �� ����ٴ� ��ġ
    /// </summary>
    Transform cameraTarget;
    /// <summary>
    /// ������ UI 
    /// </summary>
    private TrackingBattleUI battleUI = null;
    public TrackingBattleUI BattleUI
    {
        get => battleUI;
        set => battleUI = value;

    }

    /// <summary>
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;

    /// <summary>
    /// ���� �ڽ��� ��ġ�� Ÿ��
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
    /// �ൿ�� Ȥ�� �̵����� �Ÿ�
    /// </summary>
    [SerializeField]
    protected float moveSize = 4.0f;
    public float MoveSize
    {
        get => moveSize;
        set => moveSize = value;
    }

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float moveSpeed = 3.0f;

    /// <summary>
    /// �̵��� ���ð�
    /// </summary>
    [SerializeField]
    WaitForSeconds waitTime = new WaitForSeconds(0.5f);
    /// <summary>
    /// �ൿ�������� ��ȣ���� ��������Ʈ
    /// </summary>
    public Action onActionEndCheck;

    /// <summary>
    /// ������ ī�޶� �����ϱ�
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
                BattleUI.stmGaugeSetting(stmValue, enemyData.MaxStamina); //�Ҹ�� �ൿ�� ǥ��
            }
        };
        enemyData.on_Enemy_HP_Change += (hpValue) =>
        {
            if (battleUI != null)
            {
                BattleUI.hpGaugeSetting(hpValue, enemyData.MaxHp); //�Ҹ�� �ൿ�� ǥ��
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
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI ���� ĵ������ġ
        InitUI();
    }

    protected override void OnEnable()
    {
        if (battleUICanvas != null)  //ĵ���� ��ġ�� ã�Ƴ�����
        {
            InitUI();//�ʱ�ȭ
        }
    }

    /// <summary>
    /// ������ UI �ʱ�ȭ �Լ� ����
    /// </summary>
    public void InitUI()
    {
        if (battleUI != null) //���� ������
        {
            battleUI.gameObject.SetActive(true); //Ȱ��ȭ�� ��Ų��
        }
        else //������ UI�� ���þȵ������� �����Ѵ�
        {
            battleUI = (TrackingBattleUI)Multiple_Factory.Instance.
                GetObject(EnumList.MultipleFactoryObjectList.TRACKING_BATTLE_UI_POOL); // ����ó�� �ʱ�ȭ�Ҷ� ��Ʋ UI �����ϰ� 
            battleUI.gameObject.name = $"{name} _ Tracking"; //�̸�Ȯ�ο�
            battleUI.transform.SetParent(battleUICanvas);//Ǯ�� ĵ���� �ؿ����⶧���� ��Ʋ��UI�� ������ ĵ���� ��ġ ������ �̵���Ų��.
            battleUI.gameObject.SetActive(true); //Ȱ��ȭ ��Ų��.
            battleUI.FollowTarget = cameraTarget;     //UI �� ���ְ� 1:1 ��ġ�� ���־�� ������ ��Ƶд�.
        }
    }

    /// <summary>
    /// �������� ������ ������
    /// ���� �ʱ�ȭ ��Ű�� Ǯ�� ������ ť�� ������.
    /// </summary>
    public void ResetData()
    {
        if (BattleUI != null) //��Ʋ UI�� ���õ������� 
        {
            BattleUI.ResetData();// ������ UI �ʱ�ȭ 
            BattleUI = null; // ����
        }
        currentTile.ExistType = Tile.TileExistType.None; // �Ӽ� ������ 
        
        if(enemyData.GrapPosition.transform.childCount > 0)
        {
            GameObject temp = enemyData.GrapPosition.GetChild(0).gameObject;
            Destroy(temp);
        }

        currentTile = null; //Ÿ�� ��������
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(poolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
    }

   

    public void Defence(float damage, bool isCritical = false)
    {
        enemyData.onHit();
        float finalDamage = Mathf.Max(0, damage - enemyData.DefencePower);
        GameManager.EffectPool.GetObject(finalDamage, transform, isCritical);
        enemyData.HP -= finalDamage;
    }


    /// <summary>
    /// ĳ���� �̵��� �ݺ���
    /// </summary>
    /// <param name="playerTile">�÷��̾� ��ġ���ִ� Ÿ��</param>
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

        Vector3 targetPos = currentTile.transform.position; //���̾��°�� ���� Ÿ����ġ ����
                                                            //unitAnimator.SetBool(isWalkingHash, true); //�̵��ִϸ��̼� ��� ����
        //onCameraTarget?.Invoke();   //�̵��Ҷ� ī�޶� ��������
        yield return waitTime;
        
        //foreach (Tile tile in path) //���� �ߺ� ���� ������ Ÿ�ϰ� �̸������ؼ� üũ���� 
        //{
        //    tile.ExistType = Tile.TileExistType.Monster;//�̰��� ���������� �ؿ������� ������ ����Ǿ� �����۵ȴ�.
        //}

        foreach (Tile tile in path)  // �����ִ°�� 
        {
            float timeElaspad = 0.0f;
            enemyData.Move();
            targetPos = tile.transform.position; //���ο� ��ġ��� 
            transform.GetChild(0).transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //�ش���� �ٶ󺸰� 
            this.currentTile.ExistType = Tile.TileExistType.None;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� ������Ʈ �̵��߿� Ÿ�� �������ϴ� move�κ���");
            this.currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� �� �����Ͱ� ����Ǿߵȴ� charcter �� ");
            tile.ExistType = Tile.TileExistType.Monster;//�̰��� ���������� �ؿ������� ������ ����Ǿ� �����۵ȴ�.

            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //�̵�����
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

        if (IsAttackRange()) //�����Ҽ�������
        {
            yield return CharcterAttack(playerTile);// ���� 
        }

        onActionEndCheck?.Invoke(); //�ൿ�������� ��ȣ������
    }

    void Die()
    {
        GameManager.PlayerStatus.GetExp((uint)enemyData.EnemyExp);
        GameManager.Item_Spawner.SpawnItem(this);
        onDie?.Invoke(this);
    }

    /// <summary>
    /// ���� �ϴ� �ݺ��� 
    /// </summary>
    public IEnumerator CharcterAttack(Tile attackTile)
    {
        //Debug.Log($"{enemyData.name} - {enemyData.wType} - {enemyData.mType} - {enemyData.AttackPower}");
        transform.GetChild(0).transform.rotation = Quaternion.LookRotation(attackTile.transform.position - transform.position);
        if(enemyData.wType == Enemy_.WeaponType.Riffle && enemyData.mType != Monster_Type.Size_L)
            GameManager.EffectPool.GetObject(SkillType.Penetrate, attackTile.transform.position);
        enemyData.Attack_Enemy(SpaceSurvival_GameManager.Instance.PlayerTeam[0].CharcterData);
        yield return waitTime; //���� �ִϸ��̼� ���������� ��ٷ��ִ°͵� �����Ű���.
    }
    
    /// <summary>
    /// ���ݹ��� ���� �ִ��� üũ�ϴ� �Լ�
    /// </summary>
    /// <returns>���ݰ��� true �Ұ��� false</returns>
    public bool IsAttackRange()
    {
        return Cho_BattleMap_Enemy_AStar.SetEnemyAttackSize(currentTile, enemyData.AttackRange); //���ݹ��� üũ 
    }

}
