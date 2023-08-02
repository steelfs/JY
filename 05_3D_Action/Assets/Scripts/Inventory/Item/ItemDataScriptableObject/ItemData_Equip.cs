using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equip : ItemData, IEquipable
{
    [Header("��� ������ ������")]
    public GameObject equipPrefab;//���� ��� ���� �� �÷��̾�𵨿� ���� ���� ������// ��ӵǴ� �����۰��� �ٸ� �������̴�.

    public virtual EquipType EquipParts => EquipType.Weapon;//�������� ���� ��ġ�� �˷��ִ� ������Ƽ

    public void EquipItem(GameObject target, InvenSlot slot)
    {

        if (target != null)
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
            if (equipTarget != null)
            {
                equipTarget.EquipItem(EquipParts, slot);
            }
        }
    }

    public void UnEquipItem(GameObject target, InvenSlot slot)
    {
        if (target != null)
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
            if (equipTarget != null)
            {
                equipTarget.UnEquipItem(EquipParts);
            }
        }
    }
    public void ToggleEquip(GameObject target, InvenSlot slot)
    {
        if (target != null)
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
            if (equipTarget != null)
            {
                InvenSlot oldSlot = equipTarget[EquipParts];
                if (oldSlot == null)
                {
                    EquipItem(target, slot);
                }
                else
                {
                    UnEquipItem(target, slot);
                    if (oldSlot != slot)
                    {
                        EquipItem(target, slot);
                    }
                }
            }
        }
    }


 
}
