using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamBorderStateUI : MonoBehaviour
{
    [SerializeField]
    float hpGaugeSpeed = 10.0f;
    [SerializeField]
    float stmGaugeSpeed = 2.0f;
    [SerializeField]
    float expGaugeSpeed = 2.0f;


    float hp_UI_Value = -1.0f;
    float stm_UI_Value = -1.0f;
    float exp_UI_Value = -1.0f;

    Slider hpSlider;
    TextMeshProUGUI hpText;
    TextMeshProUGUI hpMaxText;

    Slider stmSlider;
    TextMeshProUGUI stmText;
    TextMeshProUGUI stmMaxText;


    Slider expSlider;
    TextMeshProUGUI expText;
    TextMeshProUGUI expMaxText;


    //[SerializeField]
    //int buffMaxSize = 4;

    /// <summary>
    /// ����Ÿ��
    /// </summary>
    [SerializeField]
    SkillType[] buffType;

    /// <summary>
    /// Ÿ�̸� ������ �̹���
    /// </summary>
    [SerializeField]
    Image[] stateTimer;
    public Image[] StateIconTimer => stateTimer;
    
    /// <summary>
    /// ���� ������ ������ �̹���
    /// </summary>
    [SerializeField]
    Image[] imgIconArray;
    public Image[] StateIconImg => imgIconArray;

    List<SkillData> state_UI_Datas;
    IEnumerator hpGauge;
    IEnumerator stmGauge;
    IEnumerator expGauge;

    //IEnumerator uiGauge;

    WaitForFixedUpdate couroutineWait = new();
    private void Awake()
    {
        Transform child = transform.GetChild(0); //hp
        hpSlider = child.GetChild(0).GetComponent<Slider>();
        hpText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // ������
        hpMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>() ; // �ִ���
        
        child = transform.GetChild(1);  //stm
        stmText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // ���罺�¹̳�
        stmMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // �ִ뽺�¹̳�
        stmSlider = child.GetChild(0).GetComponent<Slider>();
        
        child = transform.GetChild(2); //�����̻� 
        int childCount = child.childCount;
        imgIconArray = new Image[childCount];
        stateTimer = new Image[childCount];
        state_UI_Datas = new List<SkillData>(childCount);

        child.gameObject.SetActive(false);      //�����̻�Ⱦ��� ����ġ�� �ٲٱ����� ������
        
        Image image;
        for (int i = 0; i < childCount; i++)
        {
            image = child.GetChild(i).GetChild(0).GetComponent<Image>(); //�̹��� ������ ������ ��������Ʈ ��ü �������� 
            stateTimer[i] = child.GetChild(i).GetComponent<Image>();
            imgIconArray[i] = image; //�����ð� ������ �̹��� ��ü 
            
        }
        hpGauge = HP_GaugeSetting(0.0f, 0.0f);
        stmGauge = Stm_GaugeSetting(0.0f, 0.0f);
        expGauge = Exp_GaugeSetting(0.0f, 0.0f);

        //uiGauge = UI_GaugeSetting(null,null,null,0,0,0);

        child = transform.GetChild(3);  //exp
        expText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // �������ġ
        expMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // �ִ����ġ
        expSlider = child.GetChild(0).GetComponent<Slider>();

    }
 
    public void SetStmGaugeAndText(float changeValue , float maxValue) 
    {
        StopCoroutine(stmGauge);
        stmGauge = Stm_GaugeSetting(changeValue, maxValue);
        StartCoroutine(stmGauge);

        //StopCoroutine(uiGauge);
        //uiGauge = UI_GaugeSetting(stmSlider,stmText,stmMaxText,stm_UI_Value, changeValue, maxValue);
        //StartCoroutine(uiGauge);
        //stm_UI_Value = changeValue;
    }

    public void SetHpGaugeAndText(float changeValue, float maxValue)
    {
        StopCoroutine(hpGauge);
        hpGauge = HP_GaugeSetting(changeValue, maxValue);
        StartCoroutine(hpGauge);

        //StopCoroutine(uiGauge);
        //uiGauge = UI_GaugeSetting(hpSlider, hpText,hpMaxText, hp_UI_Value, changeValue, maxValue);
        //StartCoroutine(uiGauge);
        //hp_UI_Value = changeValue;
    }

    public void SetExpGaugeAndText(float changeValue, float maxValue)
    {
        StopCoroutine(expGauge);
        expGauge = Exp_GaugeSetting(changeValue, maxValue);
        StartCoroutine(expGauge);

        //StopCoroutine(uiGauge);
        //uiGauge = UI_GaugeSetting(expSlider, expText,expMaxText, exp_UI_Value, changeValue, maxValue);
        //StartCoroutine(uiGauge);
        //exp_UI_Value = changeValue;
    }

    IEnumerator HP_GaugeSetting(float change_HpValue,float maxValue)
    {
        float tempValue = change_HpValue - hp_UI_Value;
        hpMaxText.text = $"{maxValue}";
        float timeElaspad = hp_UI_Value / maxValue;
        float checkValue = change_HpValue / maxValue; //������ �� 0~ 1��
        if (tempValue > 0) //ȸ�� 
        {
            while (timeElaspad < checkValue) //���°����� ������ ��ġ��Ӻ���
            {
                timeElaspad += Time.deltaTime * hpGaugeSpeed;
                hpSlider.value = timeElaspad;
                hp_UI_Value =  timeElaspad * maxValue;
                hpText.text = $"{hp_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        else if (tempValue < 0) //������  
        {
            while (timeElaspad > checkValue) //���°����� ������ ��ġ��Ӻ���
            {
                timeElaspad -= Time.deltaTime * hpGaugeSpeed;
                hpSlider.value = timeElaspad;
                hp_UI_Value = timeElaspad * maxValue; //�ε巴��~
                hpText.text = $"{hp_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        hpText.text = $"{change_HpValue:f0}";
        hpSlider.value = checkValue;
        hp_UI_Value = change_HpValue;
    }

    /// <summary>
    /// ���׹̳� UI ������ 
    /// </summary>
    /// <returns></returns>
    IEnumerator Stm_GaugeSetting(float change_StmValue, float maxValue)
    {
        float tempValue = change_StmValue - stm_UI_Value;
        stmMaxText.text = $"{maxValue}";
        float timeElaspad = stm_UI_Value / maxValue;
        float checkValue = change_StmValue / maxValue;
        if (tempValue > 0) //ȸ�� 
        {
            while (timeElaspad < checkValue) 
            {
                timeElaspad += Time.deltaTime * stmGaugeSpeed;
                stmSlider.value = timeElaspad ;
                stm_UI_Value = timeElaspad * maxValue;
                stmText.text = $"{stm_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        else if (tempValue < 0) //������  
        {
            while (timeElaspad > checkValue) //���°����� ������ ��ġ��Ӻ���
            {
                timeElaspad -= Time.deltaTime * stmGaugeSpeed;  
                stmSlider.value = timeElaspad ;
                stm_UI_Value = timeElaspad * maxValue; //�ε巴��~
                stmText.text = $"{stm_UI_Value:f0}";

                yield return couroutineWait;
            }
        }
        stmText.text = $"{change_StmValue:f0}";
        stmSlider.value = checkValue;
        stm_UI_Value = change_StmValue;
    }
    /// <summary>
    /// ����ġ UI ������ �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator Exp_GaugeSetting(float change_ExpValue, float maxValue)
    {
        float tempValue = change_ExpValue - exp_UI_Value;
        expMaxText.text = $"{maxValue}";
        float timeElaspad = exp_UI_Value / maxValue;
        float checkValue = change_ExpValue / maxValue;
        if (tempValue > 0) //ȸ�� 
        {
            while (timeElaspad < checkValue) //���°����� ������ ��ġ��Ӻ���
            {
                timeElaspad += Time.deltaTime * expGaugeSpeed; //�ε巴��~
                exp_UI_Value = timeElaspad *  maxValue;
                expSlider.value = timeElaspad;
                expText.text = $"{exp_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        else if (tempValue < 0) //������  
        {
            while (timeElaspad > checkValue) //���°����� ������ ��ġ��Ӻ���
            {
                timeElaspad -= Time.deltaTime * expGaugeSpeed; //�ε巴��~
                exp_UI_Value = timeElaspad * maxValue;
                expSlider.value = timeElaspad;
                expText.text = $"{exp_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        expText.text = $"{change_ExpValue:f0}";
        expSlider.value = checkValue;
        exp_UI_Value = change_ExpValue;
    }

    /// <summary>
    /// UI �������ξ���������� ���� (�������� ����� ������ ) �� �־ ������
    /// CurrentValue �� �ǽð����� ����������ϴµ� ����̾�� �ȵ�
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="text"></param>
    /// <param name="maxText"></param>
    /// <param name="currentValue"></param>
    /// <param name="changeValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    IEnumerator UI_GaugeSetting(Slider slider,TextMeshProUGUI text,TextMeshProUGUI maxText, float currentValue, float changeValue, float maxValue)
    {
        float tempValue = changeValue - currentValue;
        maxText.text = $"{maxValue}";
        float timeElaspad = currentValue / maxValue;
        float checkValue = changeValue / maxValue;
        if (tempValue > 0) //ȸ�� 
        {
            while (timeElaspad < checkValue) //���°����� ������ ��ġ��Ӻ���
            {
                timeElaspad += Time.deltaTime * expGaugeSpeed; //�ε巴��~
                currentValue = timeElaspad * maxValue;
                slider.value = timeElaspad;
                text.text = $"{currentValue:f0}";
                yield return couroutineWait;
            }
        }
        else if (tempValue < 0) //������  
        {
            while (timeElaspad > checkValue) //���°����� ������ ��ġ��Ӻ���
            {
                timeElaspad -= Time.deltaTime * expGaugeSpeed; //�ε巴��~
                currentValue = timeElaspad * maxValue;
                slider.value = timeElaspad;
                text.text = $"{currentValue:f0}";
                yield return couroutineWait;
            }
        }
        text.text = $"{changeValue:f0}";
        slider.value = checkValue;
      
    }

    // �����̻��� ��¥ .. ������¥�а� �����ϱ� �ʹ� ���̸��Ƽ� �ϵ��ڵ��̸���.. ��¥�� �Ѱ����̶�..
    /// <summary>
    /// ���� UI ���� 
    /// </summary>
    /// <param name="i">UI ����</param>
    /// <param name="sprite">�����̻� ������</param>
    public void AddState(SkillData skill)
    {
        state_UI_Datas.Add(skill);
        imgIconArray[0].sprite = skill.skill_sprite;
        stateTimer[0].fillAmount = 0.0f;
    }

    public void TrunActionValueSetting(float value) 
    {
        stateTimer[0].fillAmount = value;
    }
    /// <summary>
    /// ���� UI �ʱ�ȭ
    /// </summary>
    /// <param name="i">UI ����</param>
    /// <param name="sprite">�����̻� ������</param>
    public void RemoveState()
    {
        state_UI_Datas.Clear();
        imgIconArray[0].sprite = null;
        stateTimer[0].fillAmount = 0.0f;
    }
}
