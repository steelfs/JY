using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill_Normal_Attack : SkillData
{
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(7).GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Skill_LevelUp);

    }
    public override void InitSkillData()
    {
        SkillName = "일반 공격";
        SkillLevel = 1;
        //AnimClip
        //audioClip
        AttackRange = 4;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillPower = 1.0f;
        SkillType = SkillType.Normal;
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        Require_Stamina_For_UsingSkill = (int)(SkillLevel * 0.33f);
        //Debug.Log("레벨업 실행");
        SkillPower += 0.02f;
        if (SkillLevel % 5 == 0)
        {
            Require_Stamina_For_UsingSkill += 1;
        }
    }
    protected override void SetCurrentLevel_Description_Info(out string info)
    {
        info = $"일반 공격\n데미지 : {SkillPower * 100:f0}%\n스테미너 {Require_Stamina_For_UsingSkill:f0} 소모";
    }
    protected override void SetNextLevel_Description_Info(out string info)
    {
        if ((SkillLevel + 1) % 5 != 0)
        {
            info = $"일반 공격 \n데미지 : {(SkillPower + 0.02) * 100:f0}%\n스테미너 {Require_Stamina_For_UsingSkill:f0}소모";
        }
        else
        {
            info = $"일반 공격 \n데미지 : {(SkillPower + 0.02) * 100:f0}%\n스테미너 {Require_Stamina_For_UsingSkill + 1:f0}소모";

        }
    }
    //더블클릭시 스킬발동 구현할 차례
}
