using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - HealingPotion", menuName = "Scriptable Object/ItemData - HealingPotion", order = 5)]
public class ItemData_HealingPotion : ItemData, IUseable
{
    [Header("���� ���� ������")]
    public float heal = 30.0f;

    public bool Use(GameObject target) // �����ϸ� �κ��丮���� ���� ����
    {
        bool result = false;

        if (target != null)
        {
            IHealth health = target.GetComponent<IHealth>();
            if (health != null)
            {
                health.HP += heal;
                result = true;

            }
        }

        return result;
        //HP�� �ִ� ��󿡰� ������� �� //��� HP����
    }


}
