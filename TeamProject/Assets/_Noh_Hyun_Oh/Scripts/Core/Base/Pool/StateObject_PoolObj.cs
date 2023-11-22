using System;
using UnityEngine;
using UnityEngine.UI;

public class StateObject_PoolObj : Base_PoolObj, IStateData
{
    /// <summary>
    /// �����̻��� ����
    /// </summary>
    //StateType stateType;
    //public StateType Type 
    //{
    //    get => stateType;
    //    set 
    //    {
    //        stateType = value;
    //    }
    //}

    SkillData skillData;
    public SkillData SkillData { get=> skillData; set => skillData = value; }
   
    [SerializeField]
    [Range(0.0f,1.0f)]
    /// <summary>
    /// ���ϴ� ���ҵǴ� ��ġ 
    /// </summary>
    private float reducedDuration = 0.5f;
    public float ReducedDuration { get => reducedDuration; set => reducedDuration = value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// �����̻��� �ִ� ���ӽð�
    /// </summary>
    private float maxDuration = 1.0f;
    public float MaxDuration { get => maxDuration; set => maxDuration =value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// ���� ���ӽð�
    /// </summary>
    private float currentDuration = 0.0f;
    /// <summary>
    /// 0���� ���� �����ϸ鼭 MaxDuration ������ ũ�� �Ǹ� �����ȴ�.
    /// </summary>
    public float CurrentDuration { 
        get => currentDuration;

        set 
        {
            if (currentDuration != value) //���� ���������
            {
                if (value < 0.0f) 
                {
                    currentDuration = 0.0f; 

                }
                else if (value > MaxDuration)
                {
                    currentDuration = maxDuration;
                }
                else 
                { 
                    currentDuration = value; //�������ϰ� 
                }
                FillAmoutSetting(currentDuration); //������ ����
            }
            
        }
    }
    /// <summary>
    /// ������ ������ �̹���
    /// </summary>
    Image gaugeImg;

    /// <summary>
    /// ������ �̹���
    /// </summary>
    Image iconImg;

    TeamBorderStateUI stateBoard;
    

    /// <summary>
    /// �������ѹ����ϱ����� ���� �̸������ ����
    /// </summary>
    float computationalScale = -1.0f;
    protected  override void Awake()
    {
        base.Awake();
        gaugeImg = GetComponent<Image>();
        iconImg =  transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// �ʱ�ȭ �Լ� 
    /// </summary>
    /// <param name="skillData">��ų������ ����</param>
    public void InitData(SkillData skillData) 
    {
        if (skillData is Skill_Blessing blessing)
        {
            reducedDuration = 1.0f; //���ϴ� 1������
            maxDuration = blessing.TurnBuffCount;
            iconImg.sprite = blessing.skill_sprite;
            currentDuration = 0.0f;
            gaugeImg.fillAmount = 0.0f;
            computationalScale = (1 / MaxDuration); //���� �̸����صα�
            this.skillData = skillData;
            stateBoard = WindowList.Instance.TeamBorderManager.TeamStateUIs[0]; //������ �ϳ����ҰŴ� �� 0������
            stateBoard.AddState(skillData);
            return;
        }
        Debug.Log($" ��ų������ : {skillData} �� ���������� �ƴմϴ�.");
    }
    /// <summary>
    /// UI ������ �Լ� 
    /// </summary>
    /// <param name="value">������ ��</param>
    private void FillAmoutSetting(float value) 
    {
        gaugeImg.fillAmount = computationalScale * value; //�̸����ص� ������ ���� 
        stateBoard.TrunActionValueSetting(gaugeImg.fillAmount);
    }
    /// <summary>
    /// �ʱ�ȭ �۾�
    /// </summary>
    public void ResetData() 
    {
        //���°� �ʱ�ȭ�ϰ� 
        //Type = StateType.None; //�����̻� ���� �ʱ�ȭ
        skillData = null;
        reducedDuration = -1.0f; //���ϴ� ����� ���ʱ�ȭ 
        maxDuration = -1.0f; // �ִ�ġ �ʱ�ȭ
        currentDuration = -1.0f; //���� ���ప �ʱ�ȭ
        computationalScale = -1.0f;//���� �ʱ�ȭ
        gaugeImg.fillAmount = 1.0f;// ������ �ʱ�ȭ 
        iconImg.sprite = null;     //�̹��� �ʱ�ȭ 
        stateBoard.RemoveState();
        transform.SetParent(poolTransform); //Ǯ�� ������.
        gameObject.SetActive(false); //ť�� �ٽ� �ֱ����� ��Ȱ��ȭ 
        /*
         transform.SetParent �Լ��� ��ü�� Ȱ��ȭ ��Ȱ��ȭ ��ü�Ǵ½������� ���ɰ�� ����������.
         �׷��� Ȱ��ȭüũ���Ͽ� SetParent �� �����Ѵ�.
         ����Ȯ�� : ������Ʈ�� Ȱ��ȭ �����϶� gameobject.SetActive(false)������ �ٷ�  transform.SetParent(�θ�) �� �����ϴ� ������.
         */
    }

   
}
