using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equip : ItemData, IEquipable
{
    [Header("장비 아이템 데이터")]
    public GameObject equipPrefab;//실제 장비를 했을 때 플레이어모델에 붙을 실제 프리팹// 드롭되는 아이템과는 다른 아이템이다.

    public virtual EquipType EquipParts => EquipType.Weapon;//아이템이 장비될 위치를 알려주는 프로퍼티

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
