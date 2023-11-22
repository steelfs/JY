using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SkillType
{
    Sniping,
    Penetrate,
    rampage,
    Blessing,
    Normal

}

public class SkillData : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler, IPointerMoveHandler,IPointerExitHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    string skillName;                        public string SkillName { get => skillName; protected set { skillName = value; } }
    AnimationClip animClip;                  public AnimationClip AnimClip { get => animClip; protected set {  animClip = value; } }
    AudioClip audioClip;                     public AudioClip AudioClip { get => audioClip; protected set { audioClip = value; } }
    int attackRange = 3;                     public int AttackRange { get => attackRange; protected set { attackRange = value; } }
    int require_Stamina_For_UsingSkill;      public int Require_Stamina_For_UsingSkill { get => require_Stamina_For_UsingSkill; protected set { require_Stamina_For_UsingSkill = value; } }
    float finalDamage;                       public float FinalDamage { get => finalDamage; set { finalDamage = value; } }
    float skillPower;                        public float SkillPower { get => skillPower; set { skillPower = value; } }

    public bool IsCritical { get; set; }

    QuickSlot_Type bindingSlot = QuickSlot_Type.None; public QuickSlot_Type BindingSlot { get => bindingSlot; set { bindingSlot = value; } }

    public Sprite skill_sprite;

    uint require_Force_For_skillLevelUp;   
    public uint Require_Force_For_skillLevelUp
    {
        get => require_Force_For_skillLevelUp; 
        protected set
        {
            require_Force_For_skillLevelUp = value; 
            require_Force_Text.text = require_Force_For_skillLevelUp.ToString();
        } 
    }
    int skillLevel;                         
    public int SkillLevel
    {
        get => skillLevel; 
        protected set
        {
            skillLevel = value;
            LevelUp_Skill_Info();
            skillLevel_Text.text = skillLevel.ToString();
        }
    }

    Player_Status player;
    SkillBox_Description skillBox_Description;
    Skill_TempSlot tempSlot;
    SkillType skillType; public SkillType SkillType { get => skillType; protected set { skillType = value; } }
    //UI
    protected Image skill_Icon; 
    protected Button button;
    protected TextMeshProUGUI skillLevel_Text;
    TextMeshProUGUI require_Force_Text;
    //#EndUI

    public Action<SkillData, string, string> on_PointerEnter;
    public Action on_PointerExit;
    public Action on_PointerMove;
    public Action<SkillData> on_PointerClick;
    public Action on_Skill_LevelUp;
    public Action on_ResetData;

    private void Awake()
    {
        on_Skill_LevelUp = LevelUpFor_DataLoad;
    }
    void Start()
    {
        StartCoroutine(RateInit());
    }
    IEnumerator RateInit() 
    {
        yield return null;
        Init();
    }
    public void TestInit()
    {
        Init();
    }
    protected virtual void Init()
    {
        player = GameManager.PlayerStatus;
        skillBox_Description = transform.parent.GetChild(12).GetComponent<SkillBox_Description>();
        skill_Icon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        skillLevel_Text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        require_Force_Text = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        player.Base_Status.on_DarkForceChange -= (darkForce) => button.interactable = Require_Force_For_skillLevelUp < darkForce;
        player.Base_Status.on_DarkForceChange += (darkForce) => button.interactable = Require_Force_For_skillLevelUp < darkForce;
        on_PointerEnter = skillBox_Description.Open;
        on_PointerExit = skillBox_Description.Close;
        on_PointerMove = skillBox_Description.MovePosition;
        skill_Icon.sprite = skill_sprite;
        tempSlot = transform.parent.GetChild(13).GetComponent<Skill_TempSlot>();
        InitSkillData();
        on_ResetData = InitSkillData;
    }
    public virtual void InitSkillData()
    {

    }
    protected virtual void Skill_LevelUp()
    {
        if (Require_Force_For_skillLevelUp < player.DarkForce)
        {
            player.Base_Status.Base_DarkForce -= Require_Force_For_skillLevelUp;
            SkillLevel++;
        }
    }
    void LevelUpFor_DataLoad()
    {
        SkillLevel++;
    }
    protected virtual void SetCurrentLevel_Description_Info(out string info)
    {
        info = string.Empty;
        //stamina 사용량
        // 공격력 증가량
        // 
    }
    protected virtual void SetNextLevel_Description_Info(out string info)
    {
        info = string.Empty;
        //stamina 사용량
        // 공격력 증가량
        // 
    }
    protected virtual void LevelUp_Skill_Info()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        on_PointerClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCurrentLevel_Description_Info(out string current_Info);
        SetNextLevel_Description_Info(out string next_Info);
        on_PointerEnter?.Invoke(this, current_Info, next_Info);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        on_PointerMove?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        on_PointerExit?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        tempSlot.Open(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        tempSlot.MovePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        tempSlot.Close();
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            QuickSlot quickSlot = obj.GetComponent<QuickSlot>();
            if (quickSlot != null)
            {
                quickSlot.SkillData = this;
            }
        }
    
    }

}
