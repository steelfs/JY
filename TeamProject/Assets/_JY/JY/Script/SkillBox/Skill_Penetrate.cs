using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Penetrate : SkillData
{
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(10).GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Skill_LevelUp);
  
    }
    public override void InitSkillData()
    {
        SkillName = "����";
        SkillLevel = 1;
        //AnimClip
        //audioClip
        AttackRange = 4;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillPower = 0.8f;
        SkillType = SkillType.Penetrate;
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        SkillPower += 0.08f;
        if (SkillLevel % 3 == 0)
        {
            Require_Stamina_For_UsingSkill += 1;
        }
    }
    protected override void SetCurrentLevel_Description_Info(out string info)
    {
        info = $"�������� 4���� ������ ���� ������ ���Ѵ�.\n������ : {SkillPower * 100:f0}%\n���׹̳� {Require_Stamina_For_UsingSkill:f0} �Ҹ�";
    }
    protected override void SetNextLevel_Description_Info(out string info)
    {
        if ((SkillLevel + 1) % 3 != 0)
        {
            info = $"������ : {(SkillPower + 0.08) * 100:f0}%\n���׹̳� {Require_Stamina_For_UsingSkill:f0}�Ҹ�";
        }
        else
        {
            info = $"������ : {(SkillPower + 0.08) * 100:f0}%\n���׹̳� {Require_Stamina_For_UsingSkill + 1:f0}�Ҹ�";

        }
    }
}
