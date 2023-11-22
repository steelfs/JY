using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Rampage : SkillData
{
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(9).GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Skill_LevelUp);

    }
    public override void InitSkillData()
    {
        SkillName = "난사";
        SkillLevel = 1;
        AttackRange = 8;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillPower = 0.7f;
        SkillType = SkillType.rampage;
        //AnimClip
        //audioClip
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        SkillPower += 0.05f;
        if (SkillLevel % 3 == 0)
        {
            Require_Stamina_For_UsingSkill += 1;
        }
    }
    protected override void SetCurrentLevel_Description_Info(out string info)
    {
        info = $"여덟명의 적에게 방사피해를 주는 난사를 가한다.\n데미지 : {SkillPower * 100:f0}%\n스테미너 {Require_Stamina_For_UsingSkill:f0} 소모";
    }
    protected override void SetNextLevel_Description_Info(out string info)
    {
        if ((SkillLevel + 1) % 3 != 0)
        {
            info = $"데미지 : {(SkillPower + 0.05) * 100:f0}%\n스테미너 {Require_Stamina_For_UsingSkill:f0}소모";
        }
        else
        {
            info = $"데미지 : {(SkillPower + 0.05) * 100:f0}%\n스테미너 {Require_Stamina_For_UsingSkill + 1:f0}소모";

        }
    }
}
