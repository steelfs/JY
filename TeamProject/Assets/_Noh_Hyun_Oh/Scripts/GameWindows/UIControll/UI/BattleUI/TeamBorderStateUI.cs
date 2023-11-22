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
    /// 버프타입
    /// </summary>
    [SerializeField]
    SkillType[] buffType;

    /// <summary>
    /// 타이머 조절할 이미지
    /// </summary>
    [SerializeField]
    Image[] stateTimer;
    public Image[] StateIconTimer => stateTimer;
    
    /// <summary>
    /// 버프 아이콘 보여줄 이미지
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
        hpText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // 현재피
        hpMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>() ; // 최대피
        
        child = transform.GetChild(1);  //stm
        stmText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // 현재스태미나
        stmMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // 최대스태미나
        stmSlider = child.GetChild(0).GetComponent<Slider>();
        
        child = transform.GetChild(2); //상태이상 
        int childCount = child.childCount;
        imgIconArray = new Image[childCount];
        stateTimer = new Image[childCount];
        state_UI_Datas = new List<SkillData>(childCount);

        child.gameObject.SetActive(false);      //상태이상안쓰고 경험치로 바꾸기위해 가리기
        
        Image image;
        for (int i = 0; i < childCount; i++)
        {
            image = child.GetChild(i).GetChild(0).GetComponent<Image>(); //이미지 아이콘 보여줄 스프라이트 객체 가져오기 
            stateTimer[i] = child.GetChild(i).GetComponent<Image>();
            imgIconArray[i] = image; //남은시간 보여줄 이미지 객체 
            
        }
        hpGauge = HP_GaugeSetting(0.0f, 0.0f);
        stmGauge = Stm_GaugeSetting(0.0f, 0.0f);
        expGauge = Exp_GaugeSetting(0.0f, 0.0f);

        //uiGauge = UI_GaugeSetting(null,null,null,0,0,0);

        child = transform.GetChild(3);  //exp
        expText = child.GetChild(1).GetComponent<TextMeshProUGUI>(); // 현재경험치
        expMaxText = child.GetChild(3).GetComponent<TextMeshProUGUI>(); // 최대경험치
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
        float checkValue = change_HpValue / maxValue; //수정될 값 0~ 1값
        if (tempValue > 0) //회복 
        {
            while (timeElaspad < checkValue) //들어온값보다 작으면 수치계속변경
            {
                timeElaspad += Time.deltaTime * hpGaugeSpeed;
                hpSlider.value = timeElaspad;
                hp_UI_Value =  timeElaspad * maxValue;
                hpText.text = $"{hp_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        else if (tempValue < 0) //데미지  
        {
            while (timeElaspad > checkValue) //들어온값보다 작으면 수치계속변경
            {
                timeElaspad -= Time.deltaTime * hpGaugeSpeed;
                hpSlider.value = timeElaspad;
                hp_UI_Value = timeElaspad * maxValue; //부드럽게~
                hpText.text = $"{hp_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        hpText.text = $"{change_HpValue:f0}";
        hpSlider.value = checkValue;
        hp_UI_Value = change_HpValue;
    }

    /// <summary>
    /// 스테미나 UI 조절용 
    /// </summary>
    /// <returns></returns>
    IEnumerator Stm_GaugeSetting(float change_StmValue, float maxValue)
    {
        float tempValue = change_StmValue - stm_UI_Value;
        stmMaxText.text = $"{maxValue}";
        float timeElaspad = stm_UI_Value / maxValue;
        float checkValue = change_StmValue / maxValue;
        if (tempValue > 0) //회복 
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
        else if (tempValue < 0) //데미지  
        {
            while (timeElaspad > checkValue) //들어온값보다 작으면 수치계속변경
            {
                timeElaspad -= Time.deltaTime * stmGaugeSpeed;  
                stmSlider.value = timeElaspad ;
                stm_UI_Value = timeElaspad * maxValue; //부드럽게~
                stmText.text = $"{stm_UI_Value:f0}";

                yield return couroutineWait;
            }
        }
        stmText.text = $"{change_StmValue:f0}";
        stmSlider.value = checkValue;
        stm_UI_Value = change_StmValue;
    }
    /// <summary>
    /// 경험치 UI 조절용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Exp_GaugeSetting(float change_ExpValue, float maxValue)
    {
        float tempValue = change_ExpValue - exp_UI_Value;
        expMaxText.text = $"{maxValue}";
        float timeElaspad = exp_UI_Value / maxValue;
        float checkValue = change_ExpValue / maxValue;
        if (tempValue > 0) //회복 
        {
            while (timeElaspad < checkValue) //들어온값보다 작으면 수치계속변경
            {
                timeElaspad += Time.deltaTime * expGaugeSpeed; //부드럽게~
                exp_UI_Value = timeElaspad *  maxValue;
                expSlider.value = timeElaspad;
                expText.text = $"{exp_UI_Value:f0}";
                yield return couroutineWait;
            }
        }
        else if (tempValue < 0) //데미지  
        {
            while (timeElaspad > checkValue) //들어온값보다 작으면 수치계속변경
            {
                timeElaspad -= Time.deltaTime * expGaugeSpeed; //부드럽게~
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
    /// UI 공용으로쓸라고햇지만 버그 (연속으로 실행시 문제됨 ) 가 있어서 사용안함
    /// CurrentValue 를 실시간으로 갱신해줘야하는데 방법이없어서 안됨
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
        if (tempValue > 0) //회복 
        {
            while (timeElaspad < checkValue) //들어온값보다 작으면 수치계속변경
            {
                timeElaspad += Time.deltaTime * expGaugeSpeed; //부드럽게~
                currentValue = timeElaspad * maxValue;
                slider.value = timeElaspad;
                text.text = $"{currentValue:f0}";
                yield return couroutineWait;
            }
        }
        else if (tempValue < 0) //데미지  
        {
            while (timeElaspad > checkValue) //들어온값보다 작으면 수치계속변경
            {
                timeElaspad -= Time.deltaTime * expGaugeSpeed; //부드럽게~
                currentValue = timeElaspad * maxValue;
                slider.value = timeElaspad;
                text.text = $"{currentValue:f0}";
                yield return couroutineWait;
            }
        }
        text.text = $"{changeValue:f0}";
        slider.value = checkValue;
      
    }

    // 상태이상은 진짜 .. 기존에짜둔거 수정하기 너무 양이많아서 하드코딩이많다.. 어짜피 한개뿐이라..
    /// <summary>
    /// 버프 UI 셋팅 
    /// </summary>
    /// <param name="i">UI 순번</param>
    /// <param name="sprite">상태이상 아이콘</param>
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
    /// 버프 UI 초기화
    /// </summary>
    /// <param name="i">UI 순번</param>
    /// <param name="sprite">상태이상 아이콘</param>
    public void RemoveState()
    {
        state_UI_Datas.Clear();
        imgIconArray[0].sprite = null;
        stateTimer[0].fillAmount = 0.0f;
    }
}
