using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Blessing : SkillData
{
    int turnBuffCount = 0;
    public int TurnBuffCount => turnBuffCount;
    
    protected override void Init()
    {
        base.Init();
        button = transform.parent.GetChild(8).GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Skill_LevelUp);
        //AnimClip
        //audioClip
    }
    public override void InitSkillData()
    {
        SkillName = "����";
        SkillLevel = 1;
        Require_Force_For_skillLevelUp = 1;
        Require_Stamina_For_UsingSkill = 1;
        SkillPower = 1.2f;
        SkillType = SkillType.Blessing;

        turnBuffCount = 10;
    }
    protected override void LevelUp_Skill_Info()
    {
        Require_Force_For_skillLevelUp += 1;
        SkillPower += 0.05f;
        if (SkillLevel % 2 == 0)
        {
            Require_Stamina_For_UsingSkill += 1;
        }
        if (turnBuffCount < 30)
        {
            turnBuffCount++;
        }
    }
    protected override void SetCurrentLevel_Description_Info(out string info)
    {
        info = $"���� �� ���� �÷��̾��� �ɷ�ġ�� ����Ų��.\n{turnBuffCount}�ϵ��� ���ݷ�, ���� {SkillPower * 100:f0}% ����\n���׹̳� �Ҹ� {Require_Stamina_For_UsingSkill}";
    }
    protected override void SetNextLevel_Description_Info(out string info)
    {
        int min = 1;
        int max = 30;
        if ((SkillLevel + 1) % 2 != 0)
        {
            info = $"���� �� ���� �÷��̾��� �ɷ�ġ�� ����Ų��.\n{Mathf.Clamp(turnBuffCount + 1, min, max):f0}�ϵ��� ���ݷ�, ���� {(SkillPower  + 0.05) * 100:f0}% \n���׹̳� �Ҹ� {Require_Stamina_For_UsingSkill}";
        }
        else
        {
            info = $"���� �� ���� �÷��̾��� �ɷ�ġ�� ����Ų��.\n{Mathf.Clamp(turnBuffCount + 1, min, max):f0}�ϵ��� ���ݷ�, ���� {(SkillPower + 0.05) * 100:f0}% \n���׹̳� �Ҹ� {Require_Stamina_For_UsingSkill + 1}";

        }
    }
}
