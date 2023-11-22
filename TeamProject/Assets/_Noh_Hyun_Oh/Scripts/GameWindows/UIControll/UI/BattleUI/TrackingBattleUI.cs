using EnumList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ UI ��� Ŭ���� 
/// �����̻� �� ü�� ���� UI ��� ����ִ� ���۳�Ʈ
/// </summary>
public class TrackingBattleUI : Base_PoolObj
{
#if UNITY_EDITOR
    public bool isDebug = false; //����� ǥ������ üũ�ڽ� �����Ϳ� �߰�����
#endif

    /// <summary>
    /// ������ ����
    /// </summary>
    [SerializeField]
    private Transform followTarget = null;
    public Transform FollowTarget
    {
        get => followTarget;
        set
        {
            followTarget = value;
        }

    }

    /// <summary>
    /// ���� �����ְ��ִ� ī�޶� �Ÿ����������� ���ȴ� 
    /// </summary>
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    /// <summary>
    /// ī�޶���� �Ÿ� �ּҰ� �̰Ÿ� ���ϰ� �Ǹ� �Ⱥ�����ߵȴ�.
    /// </summary>
    float minDirection = 3.0f;
    [SerializeField]
    /// <summary>
    /// ī�޶���� �Ÿ� �ִ밪 �̰Ÿ� �̻��� �Ǹ� �Ⱥ�����ߵȴ�
    /// </summary>
    float maxDirection = 20.0f;

    /// <summary>
    /// ī�޶� Ÿ����ġ���� Y ������ �󸶳� �������������� �� screen��ǥ �̴� x , y 
    /// </summary>
    [SerializeField]
    float uiDir = 30.0f;

    /// <summary>
    /// ī�޶�� ĳ���Ͱ��� �⺻�Ÿ��� ī�޶� Ÿ�������� ����ִ� ĳ���Ͱ��� �Ÿ� 
    /// </summary>
    float defaultDir = 10.0f;

    /// <summary>
    /// �����̻� ���ٿ� �� ����
    /// </summary>
    const int gridWidthLength = 4;

    /// <summary>
    /// 1�������� ���ٿ� ���� ������ ������
    /// �������� �ѹ����ϱ����� �ļ� 
    /// </summary>
    readonly float gridWidthScale = 1.0f / gridWidthLength;


    /// <summary>
    /// �����̻��� �� �׸��巹�̾ƿ� �׷�
    /// </summary>
    GridLayoutGroup glg;

    /// <summary>
    /// ���������� �����̻� ��ġ��  
    /// </summary>
    RectTransform rtTop;

    /// <summary>
    /// �Ʒ��� ������ ü�¹� ���¹̳� ������ ��ġ��
    /// </summary>
    RectTransform rtBottom;

    /// <summary>
    /// �����̻� ��ġ�� ���� ����
    /// </summary>
    Vector2 topDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// ü�¹� ���¹̳� ��ġ�� ���� ����
    /// </summary>
    Vector2 bottomDefaultAnchoredPosition = Vector2.zero;

    /// <summary>
    /// �����̻� ũ�������� ����
    /// </summary>
    Vector2 topDefaultDeltaSize = Vector2.zero;

    /// <summary>
    /// ü�� �� ���¹̳� ũ�������� ����
    /// </summary>
    Vector2 bottomDefaultSize = Vector2.zero;

    /// <summary>
    /// �����̻��� ũ�⺯���� ���� ���Ǵ� ����
    /// </summary>
    Vector2 topGroupCellSize = Vector2.zero;

    /// <summary>
    /// �����̻��� �þ��� �׸�����ġ���������� ���Ǵ� ����
    /// </summary>
    Vector2 gridPos = Vector2.zero;

    /// <summary>
    /// �����̻� UI �� �׸���׷� ��ġ��
    /// </summary>
    Transform stateGroup;


    /// <summary>
    /// ���ӽð��� �����ų� �������� ����ؼ� ���°� ������ ��� ȣ��
    /// ������ ��ų������ �Ⱦ�
    /// </summary>
    public Action<SkillData> releaseStatus;

    /// <summary>
    /// �����̻� �ִ밹�� 
    /// </summary>
    int stateSize = 4;
    int StateSize {
        get => stateSize;
        set
        {
            if (value > stateSize) //ũ�Ⱑ �⺻�������ũ�� 
            {
                stateSize *= 2; //�ϴ� �ι�� �ø���
                IStateData[] temp = states; //���������� ����صд�.
                states = new IStateData[stateSize]; // �ø������ŭ ���� �迭 �����. 
                for (int i = 0; i < temp.Length; i++)
                {
                     states[i] = temp[i]; //���������� �ٽ� ��´�.
                }
            }
        }
        
    }

    /// <summary>
    /// ���� �����̻� ���� ��Ƶ� �迭
    /// </summary>
    private IStateData[] states;
    public IStateData[] States => states;

    private List<SkillData> stateSkillDatas;

    /// <summary>
    /// ü�� ������ ������ ��Ʈ
    /// </summary>
    private RectTransform hpRect;

    /// <summary>
    /// ���׹̳� ������ ������ ��Ʈ
    /// </summary>
    private RectTransform stmRect;

    private float hp_UI_Value = 1.0f;   // ü�� ������������ ����         ���� ���°�
    private float stm_UI_Value = 1.0f;  // ���׹̳� ���������� ����       ���� ���°�

    private float change_HpValue = 0.0f;    //������ ������ ������
    private float change_StmValue = 0.0f;   //������ ������ ������

    /// <summary>
    /// �ܺο��� ȣ���ؼ� ����� ����
    /// </summary>
    public Action<float, float> hpGaugeSetting;
    IEnumerator hpChangeCoroutine;

    /// <summary>
    /// �ܺο��� ȣ���ؼ� ����� �
    /// </summary>
    public Action<float, float> stmGaugeSetting;
    IEnumerator stmChangeCoroutine;
    
    //������ �����ӵ� ���� 
    [SerializeField]
    [Range(0.1f,2.0f)]
    float gaugeSpeed = 1.0f;


    CanvasGroup cg;

    /// <summary>
    /// 
    /// </summary>
    WaitForFixedUpdate uiGaugeSpeed = new();

    Slider hpSlider;
    Slider stmSlider;

    /// <summary>
    /// �ʱⰪ���� �����صд� ���߿� �Ÿ������� ������������ ����Ұ�
    /// </summary>
    protected override  void Awake()
    {
        base.Awake();
        cg = GetComponent<CanvasGroup>();
        stateGroup = transform.GetChild(0);
        glg = stateGroup.GetComponent<GridLayoutGroup>();
        rtTop = stateGroup.GetComponent<RectTransform>();
        Transform tempChild = transform.GetChild(1);
        rtBottom = tempChild.GetComponent<RectTransform>();

        mainCamera = Camera.main;
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //ī�޶� ã�Ƽ� �����Ѵ�. - �ó׸ӽ����� ī�޶� ��ȯ�� �ش簪�� ��ü�ϴ� ������ �߰��� �ʿ�
        //��ó�� ������� ������(�⺻)���� �����صд�.
        topGroupCellSize = glg.cellSize;
        //�����̻�
        topDefaultAnchoredPosition = rtTop.anchoredPosition;
        topDefaultDeltaSize = rtTop.sizeDelta;
        //ü�� ���¹̳�
        bottomDefaultAnchoredPosition = rtBottom.anchoredPosition;
        bottomDefaultSize = rtBottom.sizeDelta;



        states = new IStateData[stateSize]; // �����̻��� �迭ũ�⸦ ��Ƶд�.
        stateSkillDatas = new(stateSize);

        stmRect = tempChild.GetChild(1).GetChild(0).GetComponent<RectTransform>(); // stm rect
        hpChangeCoroutine = HP_GaugeSetting();
        hpRect = tempChild.GetChild(2).GetChild(0).GetComponent<RectTransform>(); // hp rect
        stmChangeCoroutine = Stm_GaugeSetting();
        hpGaugeSetting = (now, max) => {
            //������ �ǽð� ó���� �����ϰ� ������. �����ѹ��� 1���� �����ǵ��� ȣ���� �ʿ�
            //��Ÿ �� 1���� �������� ó���ϵ��� ȸ���� ��������
            change_HpValue = now/max;
            StopCoroutine(hpChangeCoroutine);
            hpChangeCoroutine = HP_GaugeSetting();
            StartCoroutine(hpChangeCoroutine);

        };
        stmGaugeSetting = (now, max) => {
            change_StmValue = now/max;
            StopCoroutine(stmChangeCoroutine);
            stmChangeCoroutine = Stm_GaugeSetting();
            StartCoroutine(stmChangeCoroutine);
        };




    }

    /// <summary>
    /// Ȱ��ȭ�� �����ϱ�
    /// </summary>
    protected override void OnEnable()
    {
        InitTracking();
    }
   
    /// <summary>
    /// ��Ʋ�� ���۽� �ʱ�ȭ�� ���� ���� 
    /// </summary>
    private void InitTracking() 
    {
        //mainCamera = Camera.main; //����Ƽ���� �������ִ°� ����غ��� awake������ �������´�. OnEnable ó��ȣ�⿡�����������´�.
        StopAllCoroutines();//���������ϴ��������� ���߰� 
        StartCoroutine(StartTracking()); //������ô
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    IEnumerator StartTracking() 
    {
        while (true)
        {
            SetTrackingUI(); //ĳ���Ͱ� �����϶����� UI ũ��� ��ġ�� �����������Ѵ�. 
            yield return null;

        }
    }
    /// <summary>
    /// ���� 0~ 1 
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator HP_GaugeSetting()
    {
        Vector2 tempVector = Vector2.zero; //rect transform ����ȯ�� ����� ����
        if (change_HpValue > hp_UI_Value) //ȸ�� 
        {
            while (hp_UI_Value < change_HpValue) //���°����� ������ ��ġ��Ӻ���
            {
                hp_UI_Value += Time.deltaTime * gaugeSpeed; //�ε巴��~
                RectUISetting(hpRect, hp_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(hpRect, change_HpValue);
        }
        else if (change_HpValue < hp_UI_Value) //������  
        {
            while (hp_UI_Value > change_HpValue)
            {
                hp_UI_Value -= Time.deltaTime * gaugeSpeed; //���� ������ ��� ���⸸ �ݴ���
                RectUISetting(hpRect, hp_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(hpRect, change_HpValue);
        }
        else 
        {
            RectUISetting(hpRect, change_HpValue);
        }
    }
    /// <summary>
    /// ���׹̳� UI ������ �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator Stm_GaugeSetting()
    {
        if (change_StmValue > stm_UI_Value) //ȸ�� 
        {
            while (stm_UI_Value < change_StmValue) //���°����� ������ ��ġ��Ӻ���
            {
                stm_UI_Value += Time.deltaTime * gaugeSpeed; //�ε巴��~
                RectUISetting(stmRect, stm_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(stmRect, change_StmValue);
        }
        else if (change_StmValue < stm_UI_Value) //������  
        {
            while (stm_UI_Value > change_StmValue)
            {
                stm_UI_Value -= Time.deltaTime * gaugeSpeed;
                RectUISetting(stmRect, stm_UI_Value);
                yield return uiGaugeSpeed;
            }
            RectUISetting(stmRect, change_StmValue);
        }
        else 
        {
            RectUISetting(stmRect, change_StmValue);
        }
    }
    /// <summary>
    /// Gauge UI �� �����ϱ����� �Լ�
    /// </summary>
    /// <param name="uiRect">������ RectTransform</param>
    /// <param name="value">������ ��</param>
    private void RectUISetting(RectTransform uiRect,float value) 
    {
        Vector2 tempVector = Vector2.zero;          //rect transform ����ȯ�� ����� ����
        tempVector = uiRect.anchorMax;              //rect transform Anchors ���� max �� 
        tempVector.x = value;                       // ���߿� x ���� ���̸� ��
        uiRect.anchorMax = tempVector;              // right ���������� �޾ƿ��� 
        uiRect.offsetMax = Vector2.zero;            // right �� 0���� �����ؼ� �̹��� �̵���Ű��
    }
    /// <summary>
    /// ī�޶�� �÷��̾�� �Ÿ��� �缭 ������ UI ũ�⸦ ������Ű�� �Լ�
    /// </summary>
    private void SetTrackingUI()
    {
        if (FollowTarget != null ) //�÷��̾ ������츸 ����
        {
            Vector3 playerPosition = mainCamera.WorldToScreenPoint(FollowTarget.position); //�÷��̾� ��ũ����ǥ�� �о�´�.
            playerPosition.y += uiDir; //ĳ������ġ���߾ӿ��� ��¦���� 
            transform.position = playerPosition; //������ ������Ʈ�� ��ġ�� �i�ư���.
            
#if UNITY_EDITOR
            if (isDebug) 
            {
                Debug.Log(playerPosition);
            }
#endif
            //UI���̴� �Ÿ� ������ Ȯ��
            if (playerPosition.z < maxDirection && playerPosition.z > minDirection) //�ش�����ȿ����� �����ش�. z�������θ���������� �߰����ָ�ȴ�.
            {
                //�������� ��������� ������ �����ϰ� �־����� ������ �����ϴ� ������ �ϰ������ ���ϱ�� �Ϸ��� if���� ��û���� 
                float scale = defaultDir / playerPosition.z; //������ �̱����� ���ϱ⿬������ ������;����� ����� ��ã�Ѵ�.
                                                             //�ʱⰪ���� ������ ���ؼ� ���������Ѵ�.
                                                             ////�������� ����������
                rtBottom.anchoredPosition = bottomDefaultAnchoredPosition * scale;
                rtBottom.sizeDelta = bottomDefaultSize * scale;

                ////�����̻�
                glg.cellSize = topGroupCellSize * scale; //������� �����ϰ� 
                gridPos = topDefaultAnchoredPosition * scale; //�����̻�⺻��ġ�� �����ѵ� 
                //�����̻��� ���ٿ��� ���� , ���ٿ��� ���� ���� ����ɶ��� ���� ������Ѵ�.
                gridPos.y += (glg.cellSize.y * //�������ŭ ������ �߰��Ѵ� 
                                (int)((transform.GetChild(0).childCount - 1) * gridWidthScale)); //������ �Ѿ�� 

                rtTop.anchoredPosition = gridPos; //������ ���� ����       
                
                rtTop.sizeDelta = topDefaultDeltaSize * scale; //�����̻��� ũ�⸦ ����

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true); //ȭ�鿡 �����ش�.
                }

            }
            else //����� 
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false); //�Ⱥ����ش�.
                }
            }
        }
    }
   
    /// <summary>
    /// ���ϴ� �����̻��� ���൵�� ���ҽ�Ű�� �Լ�  (�����ϴ��Լ�)
    /// �ϸ޴������� �ϰ�����  - �׽�Ʈ ������
    /// </summary>
    public void TrunActionStateChange() 
    {
        for (int i=0; i<  states.Length; i++) 
        {
            if (states[i] == null) continue; //���̸� �������� 
            states[i].CurrentDuration += states[i].ReducedDuration; //���� �����ϸ� �����̻� ����
            if (states[i].CurrentDuration == states[i].MaxDuration) // �����̻� ���ӽð��� �������� 
            {
                releaseStatus?.Invoke(states[i].SkillData);//����������ٰ� ��ȣ�� ������.
                stateSkillDatas.Remove(states[i].SkillData);
                states[i].ResetData();//���� �ʱ�ȭ �ϰ� 
                states[i] = null; //�迭���� ����

            }
        }

    }


    /// <summary>
    /// ���� �������� �����̻� ������ ����
    /// �迭�� �߰��ϰ� �����Ѵ�.
    /// </summary>
    /// <param name="addData">�����̻��� ����</param>
    public void AddOfStatus(SkillData skillData) 
    {
        if (stateSkillDatas.Contains(skillData)) //�߰��� ���������� 
        {
            foreach (IStateData state in states)
            {
                if (state != null && state.SkillData == skillData)
                {
                    ((StateObject_PoolObj)state).InitData(skillData);
                    return;
                }
            }
        }
        else 
        {
            for(int i=0; i< StateSize; i++)//��ü�˻��غ��� 
            {
                if (states[i] == null) //������ִ°�� 
                {
                    IStateData stateData = SettingStateUI(skillData);//Ǯ���� ��ü�����ͼ� UI����
                    stateData.SkillData = skillData;
                    states[i] = stateData;//�߰��ϰ�
                    stateSkillDatas.Add(skillData);
                    return;//����������.
                }
            }
        }
        //StateSize++; //���¸���Ʈ ���������� �迭 ������ø��� 
        //AddStateArray(addData); // �Լ��� �ٽ�ȣ���ؼ� �迭�� �߰���Ų��.
    }

    /// <summary>
    /// �����̻��� �ߵ��� ȣ��� �Լ� 
    /// �����̻󺰷� UIó������ ������ �߰��ҿ���
    /// </summary>
    /// <param name="skillData">��ų ����</param>
    /// <returns>�����̻��� ������ �����ؼ� ��ȯ</returns>
    private IStateData SettingStateUI(SkillData skillData)
    {
        StateObject_PoolObj poolObj = (StateObject_PoolObj)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.STATE_POOL); //Ǯ���� ������
        poolObj.transform.SetParent(stateGroup);// �θ� �����ϰ� 
        poolObj.InitData(skillData);    //��ų ������ �Ѱܼ� �ʱ�ȭ �Լ�����
        return poolObj;

    }

    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// ���ð� �ʱ�ȭ�� Ǯ�� ������ɷ��ִ� �����̻� �� ���� �ʱ�ȭ 
    /// ���������� ť�� �߰��ϰ� Ǯ�� ����������.
    /// </summary>
    public  void ResetData() 
    {
      
        //��������Ʈ �ʱ�ȭ 
        releaseStatus = null;
        //�Ÿ�������� ī�޶�� �������̵� �÷��̾� ������ ���� 
        //mainCamera = null;
        FollowTarget = null;
        for (int i = 0; i < StateSize; i++) //�����̻� ������ ����
        {
            if (states[i] != null) //���� ��� �ִ°͵� ã�Ƽ�
            {
                states[i].ResetData(); //���ΰ��� �ʱ�ȭ�� Ǯ�� �������۾� �ϰ�
                states[i] = null; // ������ ����
            }
        }
        RectUISetting(hpRect, 1.0f);
        RectUISetting(stmRect, 1.0f);
        transform.SetParent(poolTransform);//Ǯ�� ������
        gameObject.SetActive(false); //ť�� ������ 
        //�ʱ�ȭ �������� Ǯ�� ������.

        
    }

    public void SetVisibleUI() 
    {
        cg.alpha = 1.0f;
    }

    public void SetInVisibleUI() 
    {
        cg.alpha = 0.0f;

    }


}
