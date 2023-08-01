using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - ManaPotion", menuName = "Scriptable Object/ItemData - ManaPotion", order = 6)]
public class ItemData_ManaPotion : ItemData, IUseable
{
    [Header("���� ���� ������")]
    public float mana = 30.0f;

    public bool Use(GameObject target) // �����ϸ� �κ��丮���� ���� ����
    {
        bool result = false;

        if (target != null)
        {
            IMana mana = target.GetComponent<IMana>();
            if (mana != null)
            {
                mana.MP += this.mana;
                result = true;
            }
        }

        return result;
        //HP�� �ִ� ��󿡰� ������� �� //��� HP����
    }


}
