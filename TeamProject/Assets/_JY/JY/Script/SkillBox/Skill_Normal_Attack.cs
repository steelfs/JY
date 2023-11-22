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
        SkillName = "�Ϲ� ����";
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
        //Debug.Log("������ ����");
        SkillPower += 0.02f;
        if (SkillLevel % 5 == 0)
        {
            Require_Stamina_For_UsingSkill += 1;
        }
    }
    protected override void SetCurrentLevel_Description_Info(out string info)
    {
        info = $"�Ϲ� ����\n������ : {SkillPower * 100:f0}%\n���׹̳� {Require_Stamina_For_UsingSkill:f0} �Ҹ�";
    }
    protected override void SetNextLevel_Description_Info(out string info)
    {
        if ((SkillLevel + 1) % 5 != 0)
        {
            info = $"�Ϲ� ���� \n������ : {(SkillPower + 0.02) * 100:f0}%\n���׹̳� {Require_Stamina_For_UsingSkill:f0}�Ҹ�";
        }
        else
        {
            info = $"�Ϲ� ���� \n������ : {(SkillPower + 0.02) * 100:f0}%\n���׹̳� {Require_Stamina_For_UsingSkill + 1:f0}�Ҹ�";

        }
    }
    //����Ŭ���� ��ų�ߵ� ������ ����
}
