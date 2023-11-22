using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleMapPlayerBase : Base_PoolObj, ICharcterBase
{
    //public static BattleMapPlayerBase instance;
    /// <summary>
    /// ���� ĳ���� ��Ʈ���Ҽ��ִ»������� üũ
    /// </summary>
    bool isControll = false;
    public bool IsControll
    {
        get => isControll;
        set => isControll = value;
    }

    /// <summary>
    /// ĳ���� ������ ������ ����
    /// </summary>
    Player_ charcterData;
    public Player_ CharcterData => charcterData;

    /// <summary>
    /// ĳ���� �𵨸��� ����� �� ����Ű�� �ִ� ������Ʈ�� ��ġ 
    /// ī�޶� �� Ÿ�ٹ� UI ��ġ �����Ҷ� ����Ѵ�.
    /// </summary>
    Transform cameraTarget;

    /// <summary>
    /// �̵����װ� �����ؼ� üũ�ϴ� ����
    /// </summary>
    bool isMoveCheck = false;
    public bool IsMoveCheck => isMoveCheck;

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
    /// ���� ����ġ���ִ� Ÿ��
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
    /// ������ UI �� �ִ� ĵ���� ��ġ
    /// </summary>
    Transform battleUICanvas;
    public Transform BattleUICanvas => battleUICanvas;


    /// <summary>
    /// �ൿ�� Ȥ�� �̵� �Ÿ�
    /// </summary>
    [SerializeField]
    float moveSize = 5.0f;
    
    public float MoveSize
    {
        get => moveSize;
        set => moveSize = value;
    }

    /// <summary>
    /// �̵��� �ִϸ��̼� 
    /// </summary>
    [SerializeField]
    Animator unitAnimator;

    /// <summary>
    /// �ִϸ��̼� �̸��� �̸� ĳ��
    /// </summary>
    int isWalkingHash = Animator.StringToHash("IsWalking");

    /// <summary>
    /// �̵� �ӵ� 
    /// </summary>
    [Range(0.0f,10.0f)]
    [SerializeField]
    float moveSpeed = 3.0f;


    /// <summary>
    /// ������ܿ��ִ� ĳ���� ����â
    /// </summary>
    UICamera viewPlayerCamera;

    public Action<Tile,float> onMoveRangeClear;

    protected override void Awake()
    {
        base.Awake();
        charcterData = GetComponentInChildren<Player_>();
        unitAnimator = transform.GetChild(0).GetComponent<Animator>();
        cameraTarget = transform.GetChild(transform.childCount-1); //������ ��ġ�� �־����
    }

    private void Start()
    {
        battleUICanvas = WindowList.Instance.transform.GetChild(0).GetChild(0);  // TrackingUI ���� ĵ������ġ
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
                BattleUI.stmGaugeSetting(stmValue, playerData.Base_MaxStamina); //�Ҹ�� �ൿ�� ǥ��
            }
            onMoveRangeClear?.Invoke(currentTile, currentMoveSize);
            if (TurnManager.Instance.TurnIndex > 0 &&  stmValue < 1.0f) //�ּ��ൿ��? ���� ������ 
            {
                TurnManager.Instance.CurrentTurn.TurnEndAction();//������ 
            }
        };
      
        playerData.on_CurrentHP_Change += (hpValue) =>
        {
            uiComp.SetHpGaugeAndText(hpValue, playerData.Base_MaxHP);
            if (battleUI != null)
            {
                BattleUI.hpGaugeSetting(hpValue, playerData.Base_MaxHP); //�Ҹ�� �ൿ�� ǥ��
            }
        };

        charcterData.on_Buff_Start += (buffValue) =>
        {
            battleUI.AddOfStatus(buffValue);
        };
        StartCoroutine(rateDisable());
    }

    /// <summary>
    /// �ʱ�ȭ �� ť�� ������
    /// </summary>
    /// <returns></returns>
    IEnumerator rateDisable() 
    {
        yield return null;
        //start �����̳����� ������Ʈ ù��°�� ��Ȱ��ȭ ���Ѽ� �ٽ� ť�� �־������.
        ResetData();
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
            battleUI.releaseStatus += (_) => { Debug.Log("��������"); charcterData.DeBuff(); }; //�������� ���
        }
        if (viewPlayerCamera == null)  //ī�޶� ���þȵ������� 
        {
            viewPlayerCamera = EtcObjects.Instance.TeamCharcterView;// EtcObject �� �̸� ������ ���ӿ�����Ʈ �������� ť�� �������̴� 
            viewPlayerCamera.TargetObject = cameraTarget; //ĳ���;ȿ� �ǹؿ����ι�° ������Ʈ�� ī�޶� Ÿ���� �����־ߦi�ƴٴѴ�.
            viewPlayerCamera.gameObject.SetActive(true); //���ó������� Ȱ��ȭ��Ű��
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
        if (viewPlayerCamera != null)
        {
            viewPlayerCamera.TargetObject = null; //Ÿ�� �����
            viewPlayerCamera.gameObject.SetActive(false); // ��Ȱ��ȭ ��Ű�� ���������� ť�� ������.
            viewPlayerCamera = null; //���� �����
        }
        if (currentTile != null) 
        {
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentTile); //�̵�����  ���½�Ű�� 
            currentTile.ExistType = Tile.TileExistType.None; // �Ӽ� ������ 
            currentTile = null; //Ÿ�� ��������
        }
        //�� ������Ʈ �ʱ�ȭ
        transform.SetParent(poolTransform); //Ǯ�� ������
        gameObject.SetActive(false); // ť�� ������.
    }

   
    public void SetTile(Tile currentTile) 
    {
        this.currentTile = currentTile;
    } 
  

    /// <summary>
    /// �±پ��� ¥�� ��ã�� ��������
    /// 
    /// �̵����� ������ 
    /// - ��� ��Ȳ���� �߻��ϴ����� �ľ��̾ȵǳ� Ÿ���� ���� charcter �� �����̾ȵǴ� ��Ȳ�� �߻� 
    ///   �̵��� �ش�������� �����͸� �ٲٰ��ֱ⶧���� �����ΰŰ����� ��Ȯ�ϰ� �ľ��� ���ϰ�����. 
    ///  �ذ�  : �̵�����ǥ���Ҷ� �ʱ�ȭ �ϴ·������� �������� 
    /// </summary>
    /// <param name="path">A��Ÿ �ִܰŸ� Ÿ�ϸ���Ʈ</param>
    /// <returns></returns>
    public IEnumerator CharcterMove(Tile moveTile)
    {
        isMoveCheck = true; //�̵� ������ üũ�ϱ� 

        List<Tile> path = Cho_BattleMap_AStar.PathFind(
                                                            SpaceSurvival_GameManager.Instance.BattleMap,
                                                            SpaceSurvival_GameManager.Instance.MapSizeX,
                                                            SpaceSurvival_GameManager.Instance.MapSizeY,
                                                            currentTile,
                                                            moveTile
                                                            );

        Vector3 targetPos = currentTile.transform.position; //���̾��°�� ���� Ÿ����ġ ����
        unitAnimator.SetBool(isWalkingHash, true); //�̵��ִϸ��̼� ��� ����
        foreach (Tile tile in path)  // �����ִ°�� 
        {
            float timeElaspad = 0.0f;
            targetPos = tile.transform.position; //���ο� ��ġ��� 
            transform.GetChild(0).transform.rotation = Quaternion.LookRotation(targetPos - transform.position); //�ش���� �ٶ󺸰� 
            currentTile.ExistType = Tile.TileExistType.Move;// ������ġ �̵������ϰ� �ٲٰ�  
            //Debug.Log($"{this.currentTile.Index}Ÿ�� ������Ʈ �̵��߿� Ÿ�� �������ϴ� move�κ���");
            currentTile = tile;
            //Debug.Log($"{this.currentTile.Index}Ÿ�� �� �����Ͱ� ����Ǿߵȴ� charcter �� ");
            tile.ExistType = Tile.TileExistType.Charcter; //�̵�����ġ ������ �ٲ۴�.
            //Debug.Log($"{this.currentTile.Index}Ÿ�� �� �����Ͱ� charcter ����Ǿ���.");
            
            while ((targetPos - transform.position).sqrMagnitude > 0.2f)  //�̵�����
            {
                timeElaspad += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, timeElaspad);
                yield return null;
            }
        }
        transform.position = targetPos;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        unitAnimator.SetBool(isWalkingHash, false);

        GameManager.PlayerStatus.Base_Status.Current_Stamina -= this.currentTile.MoveCheckG; //�����̵��� �Ÿ���ŭ ���¹̳��� ��´�.

        isMoveCheck = false; //�̵��������� üũ
    }

    /// <summary>
    /// �����ϴ� �ڷ�ƾ�� ����� �ݺ���
    /// </summary>
    /// <param name="selectedTile">������ ��ġ</param>
    /// <returns></returns>
    public IEnumerator CharcterAttack(Tile selectedTile)
    {
        yield return null;
    }

    /// <summary>
    /// ���ݹ������� �ִ��� üũ�� �Լ� 
    /// 
    /// </summary>
    /// <returns>���ݰ����ѹ����� true  �Ұ����ϸ� false</returns>
    public bool IsAttackRange()
    {
        return false;
    }
}
