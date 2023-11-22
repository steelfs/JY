using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Base_Status//아무것도 장비하지 않은 상태의 플레이어의 기본 공격력, 방어력을 저장. 버프사용, 사용중 장비, 해제 시 다시 설정할 때와 데이터를 Save, Load 할 때 사용
{
    //프로퍼티의 setter가 private이면 기준이 될 데이터 이기 때문에 실시간으로 변경해서는 안되는 데이터이다.
    //프로퍼티의 setter가 private이 아니면 버프스킬, 장비착용 등에 영향을 받지 않는 데이터이므로 실시간으로 변경이 가능하다.

    Player_Status playerStatus;
    [SerializeField]
    uint abilityPoint;
    public uint AbilityPoint
    {
        get => abilityPoint;
        set
        {
            abilityPoint = value;
            playerStatus.AbilityPoint = abilityPoint;
        }
    }
    [SerializeField]
    string playerName;
    public string Name 
    {
        get => playerName;
        set
        {
            playerName = value;
            playerStatus.Name = playerName;
        }
    }
    [SerializeField]
    uint level;
    public uint Level 
    {
        get => level;
        set
        {
            if (level != value)
            {
                level = value;
                playerStatus.Level = level;
                if (!isLoading)
                {
                    playerStatus.LevelUp();
                }
            }
        }
    }
    [SerializeField]
    uint exp;
    public uint Exp
    {
        get => exp;
        set
        {
            if (exp != value)
            {
                uint expOver = 0;
                if (value >= ExpMax)
                {
                    expOver = value - ExpMax;
                    Level++;
                }
                else
                {
                    exp = (uint)Mathf.Clamp(value, 0, ExpMax);
                }
                exp += expOver;
                playerStatus.Exp = exp;
                on_ExpChange?.Invoke(exp);//UI 에서 받을 신호 
            }
        }
    }
    public Action<uint> on_ExpChange;
    [SerializeField]
    uint expMax;
    public uint ExpMax 
    {
        get => expMax;
        private set
        {
            expMax = value;
            playerStatus.ExpMax = expMax;
            on_MaxExp_Change?.Invoke(expMax);
        }
    }
    [SerializeField]
    float base_MaxHP;
    public float Base_MaxHP
    { 
        get => base_MaxHP;
        private set
        {
            base_MaxHP = value;
            playerStatus.MaxHP = base_MaxHP;
            on_MaxHP_Change?.Invoke(base_MaxHP);
        }
    }
    [SerializeField]
    float currentHP;
    public float CurrentHP 
    {
        get => currentHP;
        set
        {
            currentHP = Mathf.Clamp(value, 0, base_MaxHP);
            playerStatus.HP = currentHP;
            on_CurrentHP_Change?.Invoke(currentHP);//UI
            if (currentHP <= 0)
            {
                on_Die?.Invoke();
            }
        }
    }
    public Action on_Die;
    public Action<float> on_CurrentHP_Change;
    [SerializeField]
    float current_Stamina;
    public float Current_Stamina 
    {
        get => current_Stamina;
        set
        {
            current_Stamina = Mathf.Clamp(value, 0, base_MaxStamina);
            playerStatus.Stamina = current_Stamina;
            on_CurrentStamina_Change?.Invoke(current_Stamina);//UI
        }
    }
    public Action<float> on_CurrentStamina_Change;
    [SerializeField]
    float base_MaxStamina;
    public float Base_MaxStamina 
    {
        get => base_MaxStamina;
        private set
        {
            base_MaxStamina = value;
            playerStatus.MaxStamina = base_MaxStamina;
            on_MaxStamina_Change?.Invoke(base_MaxStamina);//UI
        }
    }
    [SerializeField]
    uint BaseDarkForce;
    public uint Base_DarkForce
    {
        get => BaseDarkForce;
        set
        {
            BaseDarkForce = value;
            playerStatus.DarkForce = BaseDarkForce;
            on_DarkForceChange?.Invoke(BaseDarkForce);//UI
        }
    }
    public Action<uint> on_DarkForceChange;

    [SerializeField]
    uint base_ATT;
    public uint Base_ATT
    {
        get => base_ATT;
        set
        {
            base_ATT = value;
            playerStatus.ATT = base_ATT;
        }
    }

    [SerializeField]
    uint base_DP;
    public uint Base_DP
    {
        get => base_DP;
        set
        {
            base_DP = value;
            playerStatus.DP = base_DP;
        }
    }

    [SerializeField]
    uint base_STR;
    public uint Base_STR 
    {
        get => base_STR;
        private set
        {
            base_STR = value;
            on_ResetStatus?.Invoke();
        }
    }
    [SerializeField]
    uint base_INT;
    public uint Base_INT 
    {
        get => base_INT;
        private set
        {
            base_INT = value;
            on_ResetStatus?.Invoke();
        }
    }
    [SerializeField]
    uint base_LUK;
    public uint Base_LUK 
    {
        get => base_LUK;
        private set
        {
            base_LUK = value;
            on_ResetStatus?.Invoke();
        }
    }
    [SerializeField]
    uint base_DEX;
    public uint Base_DEX 
    {
        get => base_DEX;
        private set
        {
            base_DEX = value;
            on_ResetStatus?.Invoke();
        }
    }
    [SerializeField]
    float baseCriticalPower;
    public float BaseCriticalPower
    {
        get => baseCriticalPower;
        private set
        {
            baseCriticalPower = value;
            playerStatus.CriticalPower = baseCriticalPower;
            on_ResetStatus?.Invoke();
        }
    }
    [SerializeField]
    float damage_Min;
    public float Damage_Min
    {
        get => damage_Min;
        private set
        {
            damage_Min = value;
            playerStatus.DamageMin = damage_Min;
        }
    }

    public Action on_ResetStatus;
    public Action<float> on_MaxHP_Change;
    public Action<float> on_MaxStamina_Change;
    public Action<uint> on_MaxExp_Change;
    public Action on_LevelChange;
    public Base_Status(Player_Status player_Status)
    {
        this.playerStatus = player_Status;
        
        playerStatus.on_increase_ExpMax += (result) => ExpMax = result;
        playerStatus.on_increase_MaxHP += (increase) => Base_MaxHP += increase;
        playerStatus.on_increase_MaxStamina += (increase) => Base_MaxStamina += increase;
        playerStatus.on_Rise_STR += () => Base_STR++;
        playerStatus.on_Rise_INT += () => Base_INT++;
        playerStatus.on_Rise_LUK += () => Base_LUK++;
        playerStatus.on_Rise_DEX += () => Base_DEX++;
        playerStatus.on_Rise_CriticalPower += () => BaseCriticalPower += 0.1f;

    }
    bool isLoading;
    public void LoadData(Base_Status saveData)
    {
        isLoading = true;
        this.ExpMax = saveData.ExpMax;
        this.Level = saveData.Level;
        this.Exp = saveData.Exp;
        this.Base_ATT = saveData.Base_ATT;
        this.Base_DP = saveData.Base_DP;
        this.Base_STR = saveData.Base_STR;//다른 능력치에 영향을 주는 능력치를 먼저 업데이트 해야 함.
        this.Base_INT = saveData.Base_INT;
        this.Base_LUK = saveData.Base_LUK;
        this.Base_DEX = saveData.Base_DEX;
        this.AbilityPoint = saveData.AbilityPoint;
        this.Name = saveData.Name;
        this.CurrentHP = saveData.CurrentHP;
        this.Base_MaxHP = saveData.Base_MaxHP;
        this.Current_Stamina = saveData.Current_Stamina;
        this.Base_MaxStamina = saveData.Base_MaxStamina;
        this.Base_DarkForce = saveData.Base_DarkForce;
        this.BaseCriticalPower = saveData.BaseCriticalPower;
        this.Damage_Min = saveData.Damage_Min;
        this.ExpMax = saveData.ExpMax;//expMAx는 LevelUp 할 때 변경되기 때문에 한번 더 업데이트 필요
        isLoading = false;
    }
    public void Init()
    {
        Name = "Player";
        ExpMax = 50;
        Level = 1;
        Base_MaxHP = 100;
        CurrentHP = Base_MaxHP;
        Base_MaxStamina = 15;
        Current_Stamina = Base_MaxStamina;
        Base_ATT = 10;
        Base_DP = 10;
        Base_STR = 5;
        Base_INT = 5;
        Base_LUK = 5;
        Base_DEX = 5;
        BaseCriticalPower = 1.8f;
        AbilityPoint = 50;
        Base_DarkForce = 5000;
        Damage_Min = 0.5f;
        
    }
}
public class Player_Status : MonoBehaviour, IPopupSortWindow// , 장비장착, 버프사용시 플레이어에서 신호받아서 Base_Status의 내용 업데이트
                                          // 버튼을 눌었을 때 및 레벨업시 Base_Status의 능력치를 업데이트하는 함수 작성
{
    CanvasGroup canvasGroup;
    CanvasGroup detailCanvasGroup;
    TextMeshProUGUI nameText;
    TextMeshProUGUI levelText;
    TextMeshProUGUI attText;
    TextMeshProUGUI dpText;
    TextMeshProUGUI strText;
    TextMeshProUGUI intText;
    TextMeshProUGUI lukText;
    TextMeshProUGUI dexText;
    TextMeshProUGUI abilityPoint_Text;
    TextMeshProUGUI hpText;
    TextMeshProUGUI maxHP_Text;
    TextMeshProUGUI maxStamina_Text;
    TextMeshProUGUI staminaText;
    TextMeshProUGUI damageText;
    TextMeshProUGUI criticalDamageText;
    TextMeshProUGUI criticalRateText;
    TextMeshProUGUI dodgeRateText;
    TextMeshProUGUI expText;
    TextMeshProUGUI expMax_Text;

    Button closeButton;
    Button strButton;
    Button intButton;
    Button lukButton;
    Button dexButton;
    Button detailOpen_Button;
    Player_ player;
    Base_Status base_Status;
    Equipments_Data_Server equipments_DataServer;
    public Equipments_Data_Server Equipments_Data => equipments_DataServer;
    EquipBox equipBox;

    public Base_Status Base_Status => base_Status;
    string playerName;
    float hp;
    uint exp;
    float stamina;
    uint str;
    uint intelligence;
    uint luk;
    uint dex;
    uint att;
    uint dp;
    float damage;
    float damageMin;
    float criticalDamage;
    float criticalPower;
    float criticalRate;
    uint abilityPoint;
    uint expMax;
    uint level;
    float maxHP;
    float maxStamina;
    float dodgeRate;
    uint darkForce;

    public float Damage
    {
        get => damage;
        set
        {
            damage = value;
            damageText.text = $"{damage * DamageMin:f0} ~ {damage:f0}";
        }
    }
    public uint DarkForce
    {
        get => darkForce;
        set
        {
            darkForce = value;
        }
    }
    public uint AbilityPoint//프로퍼티 바뀔 때 UI 업데이트 및 Base_Status에 저장
    {
        get => abilityPoint;
        set
        {
            abilityPoint = value;
            abilityPoint_Text.text = $"{abilityPoint}";
        }
    }
    public uint Exp
    {
        get => exp;
        set
        {
            exp = value;
            expText.text = $"{exp}";
        }
    }
    public uint ExpMax
    {
        get => expMax;
        set
        {
            expMax = value;
            expMax_Text.text = $"/ {expMax}";
        }
    }
    public string Name
    {
        get => playerName;
        set
        {
            playerName = value;
        }
    }

    public uint Level 
    {
        get => level;
        set
        {
            level = value;
            levelText.text = $"{level}";
           // LevelUp();
        }
    }


    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            hpText.text = $"{hp}";
        }
    }
    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
            maxHP_Text.text = $"/ {maxHP}";
        }
    }

    public float MaxStamina
    {
        get => maxStamina;
        set
        {
            maxStamina = value;
            maxStamina_Text.text = $"/ {maxStamina}";
        }
    }
    public float Stamina
    {
        get => stamina;
        set
        {
            stamina = value;
            staminaText.text = $"{stamina:f0}";
        }
    }
    public uint ATT
    {
        get => att;
        set
        {
            att = value;
            attText.text = $"{att}";
        }
    }

    public uint DP
    {
        get => dp;
        set
        {
            dp = value;
            dpText.text = $"{dp}";
        }
    }

    public uint STR
    {
        get => str;
        set
        {
            str = value;
            strText.text = $"{str}";
        }
    }

    public uint INT
    {
        get => intelligence;
        set
        {
            intelligence = value;
            intText.text = $"{intelligence}";
        }
    }

    public uint LUK
    {
        get => luk;
        set
        {
            luk = value;
            lukText.text = $"{luk}";
        }
    }

    public uint DEX
    {
        get => dex;
        set
        {
            dex = value;
            dexText.text = $"{dex}";
        }
    }


    public float CriticalPower
    {
        get => criticalPower;
        set
        {
            criticalPower = value;
        }
    }
    public float CriticalDamage
    {
        get => criticalDamage;
        set
        {
            criticalDamage = value;
            criticalDamageText.text = $"{criticalDamage * DamageMin:f0} ~ {criticalDamage:f0}";
        }
    }
    public float DamageMin
    {
        get => damageMin;
        set
        {
            damageMin = value;
        }
    }
    public float CriticalRate
    {
        get => criticalRate;
        set
        {
            criticalRate = value;
            criticalRateText.text = $"{criticalRate:f1} %";
        }
    }

    public float DodgeRate
    {
        get => dodgeRate;
        set
        {
            dodgeRate = value;
            dodgeRateText.text = $"{dodgeRate:f1} %";
        }
    }

    public Action<IPopupSortWindow> PopupSorting { get; set; }

    //int



    private void Awake()
    {
        GetComponents();
        closeButton = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        closeButton.onClick.AddListener(Close);

        detailOpen_Button.onClick.AddListener(ToggleDetail_OpenClose);
        strButton.onClick.AddListener(RiseStrength);
        intButton.onClick.AddListener(RiseIntelligence);
        lukButton.onClick.AddListener(RiseLuck);
        dexButton.onClick.AddListener(Rise_Dexterity);
        base_Status = new(this);

        base_Status.on_ResetStatus = Reset_Status;
    }
    private void Start()
    {
        closeDetail();
        Close();
        Init();

    }

    private void Init()
    {
        InputSystemController.Instance.On_StatusOpen = ToggleOpenClose;
        player = GameManager.Player_;
        equipBox = GameManager.EquipBox;
        equipments_DataServer = new(equipBox);
        Reset_Status();

        DarkForceText darkForceText = FindObjectOfType<DarkForceText>();
        base_Status.on_DarkForceChange += darkForceText.Update_DarkForceText;
        base_Status.on_DarkForceChange += GameManager.SkillBox.Refresh;
        base_Status.Init();
    }
 
    void GetComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        detailCanvasGroup = transform.GetChild(2).GetComponent<CanvasGroup>();

        nameText = transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        levelText = transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        attText = transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dpText = transform.GetChild(1).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        strText = transform.GetChild(1).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        intText = transform.GetChild(1).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        lukText = transform.GetChild(1).GetChild(6).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dexText = transform.GetChild(1).GetChild(7).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        abilityPoint_Text = transform.GetChild(1).GetChild(8).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        expText = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        expMax_Text = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        hpText = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        maxHP_Text = transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        staminaText = transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        maxStamina_Text = transform.GetChild(2).GetChild(2).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        damageText = transform.GetChild(2).GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalDamageText = transform.GetChild(2).GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        criticalRateText = transform.GetChild(2).GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dodgeRateText = transform.GetChild(2).GetChild(6).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        detailOpen_Button = transform.GetChild(1).GetChild(9).GetChild(0).GetChild(0).GetComponent<Button>();
        strButton = transform.GetChild(1).GetChild(4).GetChild(1).GetChild(1).GetComponent<Button>();
        intButton = transform.GetChild(1).GetChild(5).GetChild(1).GetChild(1).GetComponent<Button>();
        lukButton = transform.GetChild(1).GetChild(6).GetChild(1).GetChild(1).GetComponent<Button>();
        dexButton = transform.GetChild(1).GetChild(7).GetChild(1).GetChild(1).GetComponent<Button>();
    }
    void Open()
    {
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        WindowList.Instance.PopupSortManager.PopupSortDataAppend(this);
    }
    void Close()
    {
        GameManager.SoundManager.PlayOneShot_OnOffToggle();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    void OpenDetail()
    {
        detailCanvasGroup.alpha = 1.0f;
        detailCanvasGroup.blocksRaycasts = true;
        detailCanvasGroup.interactable = true;
    }
    void closeDetail()
    {
        detailCanvasGroup.alpha = 0;
        detailCanvasGroup.blocksRaycasts = false;
        detailCanvasGroup.interactable = false;
    }
    void ToggleOpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    void ToggleDetail_OpenClose()
    {
        if (detailCanvasGroup.alpha == 1)
        {
            closeDetail();
        }
        else
        {
            OpenDetail();
        }
    }

    public void LevelUp()
    {
        base_Status.AbilityPoint += 5;
        on_increase_MaxHP?.Invoke(increaseMaxHP()); 
        on_increase_MaxStamina?.Invoke(increaseMaxStamina());
        base_Status.CurrentHP = MaxHP;
        base_Status.Current_Stamina = MaxStamina;
        base_Status.Exp = 0;
        on_increase_ExpMax?.Invoke((uint)(ExpMax * 1.2f));
        on_LevelUp?.Invoke();
    }
    public Action<uint> on_increase_MaxHP;
    public Action<uint> on_increase_MaxStamina;
    public Action<uint> on_increase_ExpMax;
    public Action on_Rise_STR;
    public Action on_Rise_INT;
    public Action on_Rise_LUK;
    public Action on_Rise_DEX;
    public Action on_Rise_CriticalPower;
    public Action on_LevelUp;
    // totalATT, TotalDP 값을 업데이트하는 함수 실행
    //@@작업할 것 this.ATT = base_Status.base_ATT + equipments_Total_ATT_DP.Total_ATT;//플레이어의 공격력 = 기본공격력 + 장비아이템들의 공격력 총 합
    // this.DP = base_Status.base_DP + equipments_Total_ATT_DP.Total_DP;
    public void Reset_Status()
    {
        //실제 적용될 스테이터스는 BaseStatus + 장비의 합을 항상 적용받고있어야 한다.
        equipments_DataServer = equipments_DataServer.GetEquipments_Total_ATT_DP();

        //방어력, 크리티컬확률, 회피율 등 다른 스텟의 영향을 받는 스텟을 업데이트 하기 전 STR, INT, LUK, DEX 기본스텟을 먼저 Base + 장비스텟 적용 후 나머지 능력치를 이에 맞춰 셋팅한다.
        //this.STR,INT,DEX,LUK = base + Equip
        this.STR = equipments_DataServer.Total_STR + base_Status.Base_STR;
        this.INT = equipments_DataServer.Total_INT + base_Status.Base_INT;
        this.LUK = equipments_DataServer.Total_LUK + base_Status.Base_LUK;
        this.DEX = equipments_DataServer.Total_DEX + base_Status.Base_DEX;
        this.ATT = (uint)(DEX * 0.5f) + base_Status.Base_ATT + equipments_DataServer.Total_ATT + base_Status.Base_ATT;
        this.DP = base_Status.Base_DP + equipments_DataServer.Total_DP + (uint)(STR * 0.5f);
        this.CriticalRate = equipments_DataServer.Total_CriticalRate + (LUK * 0.3f);
        this.DodgeRate = equipments_DataServer.Total_DodgeRate + (DEX * 0.3f);

        this.Damage = this.ATT;
        this.CriticalDamage = this.ATT * CriticalPower;
    }

    public bool IsDodge()
    {
        bool result = false;
        float randomValue = UnityEngine.Random.Range(0, 100);
        if (this.DodgeRate > randomValue)
        {
            result = true;
        }

        return  result;
    }
    public bool IsCritical(SkillData skillData)
    {
        bool result = false;
        float criticalChance = UnityEngine.Random.Range(0, 100);
        if (this.CriticalRate > criticalChance)
        {
            result = true;
            float finalDamage = GetFinalDamage((this.ATT * skillData.SkillPower) * CriticalPower);
            skillData.FinalDamage = finalDamage;
            //Debug.Log($"크리티컬O : {finalDamage}");
        }
        else
        {
            float finalDamage = GetFinalDamage(this.ATT * skillData.SkillPower);
            skillData.FinalDamage = finalDamage;
            //Debug.Log($"크리티컬X : {finalDamage}");

        }
        return result;
    }
    float GetFinalDamage(float originalDamage)//min/ max 값 사이의 랜덤한 값을 최종데미지로 설정
    {
        float finalDamage = 0;
        float damageMin = originalDamage * DamageMin;
        finalDamage = UnityEngine.Random.Range(damageMin, originalDamage);
        return finalDamage;
    }
    uint increaseMaxHP()
    {
        float increaseBase= 10;
        float result = increaseBase + STR;
        return (uint)result;
    }
    uint increaseMaxStamina()
    {
        float increaseBase = 1;
        float result = increaseBase + (INT * 0.5f);
        return (uint)result;
    }
     public void Recovery_HP(int recoveryValue, float duration)
    {
       // base_Status.Current_Stamina--;// stamina 차감
        base_Status.CurrentHP += recoveryValue;
        //StartCoroutine(Recovery_HP_(recoveryValue, duration));
    }
    
    IEnumerator Recovery_HP_(int recoveryValue, float duration)
    {
        float regenPerSecond = recoveryValue / duration;
        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            base_Status.CurrentHP += regenPerSecond * Time.deltaTime;
            yield return null;
        }
    }
    public void Recovery_Stamina(int recoveryValue, float duration)
    {
        base_Status.Current_Stamina += recoveryValue;
        //StartCoroutine(Recovery_Stamina_(recoveryValue, duration));
    }
    IEnumerator Recovery_Stamina_(int recoveryValue, float duration)
    {
        float regenPerSecond = recoveryValue / duration;
        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            base_Status.Current_Stamina += regenPerSecond * Time.deltaTime;
            yield return null;
        }
    }
    void Rise_CriticalPower()
    {
        on_Rise_CriticalPower?.Invoke();
    }
    void Rise_Dexterity()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            on_Rise_DEX?.Invoke();
        }
    }
    void RiseStrength()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            on_Rise_STR?.Invoke();
        }
    }
    void RiseIntelligence()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            on_Rise_INT?.Invoke();
        }
    }
    void RiseLuck()
    {
        if (base_Status.AbilityPoint > 0)
        {
            base_Status.AbilityPoint--;
            on_Rise_LUK?.Invoke();
        }
    }
    public void GetExp(uint exp)
    {
        base_Status.Exp += exp;
    }

    public void OpenWindow()
    {
        Open();
    }

    public void CloseWindow()
    {
        Close();
    }

    public void SaveData()
    {

        string json = JsonUtility.ToJson(base_Status);

        string path = $"{Application.dataPath}/Save/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fullPath = $"{path}Save.Json";
        System.IO.File.WriteAllText(fullPath, json);
    }
    public void LoadData()
    {
        string path = $"{Application.dataPath}/Save/Save.Json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            Base_Status loadedData = JsonUtility.FromJson<Base_Status>(json);
            base_Status.LoadData(loadedData);
        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }

    /// <summary>
    /// 죽었는지 체크 float 은 0 으로 비교하는건 바람직하지않지만.. Mathf.Clamp(value, 0, base_MaxHP); 를 사용해서 0으로 잡고있으니  비교는된다.
    /// </summary>
    /// <returns>죽었으면 true  안죽었으면 false</returns>
    public bool IsPlayerDie()
    {
        return Base_Status.CurrentHP == 0.0f;
    }
}
