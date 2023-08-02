using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - RegenPotion", menuName = "Scriptable Object/ItemData - RegenPotion", order = 7)]
public class ItemData_RegenPotion : ItemData, IUseable
{
    [Header("������ ������")]
    public float mana = 30.0f;
    public float hp = 50.0f;
    public float duration = 1.0f;

    public bool Use(GameObject target) // �����ϸ� �κ��丮���� ���� ����
    {
        bool result = false;

        if (target != null)
        {
            IHealth health = target.GetComponent<IHealth>();
            IMana mana = target.GetComponent<IMana>();
            if (mana != null && health != null)
            {
                if (mana.MP < mana.MaxMP || health.HP < health.MaxHP)
                {
                    mana.RegenerateMana(this.mana, duration);
                    health.HealthRegenerate(hp, duration);
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
        }
        return result;
        //HP�� �ִ� ��󿡰� ������� �� //��� HP����
    }


}
