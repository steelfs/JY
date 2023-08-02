using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equip : ItemData, IEquipable
{
    [Header("장비 아이템 데이터")]
    public GameObject equipPrefab;//실제 장비를 했을 때 플레이어모델에 붙을 실제 프리팹// 드롭되는 아이템과는 다른 아이템이다.

    public virtual EquipType EquipParts => EquipType.Weapon;//아이템이 장비될 위치를 알려주는 프로퍼티

    public void EquipItem(GameObject target, InvenSlot slot)//target = 장비할 대상, 장착할 아이쳄이 위치한 슬롯
    {

        if (target != null)
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();// 대상이 IEquipTarget을 상속 받았다면
            if (equipTarget != null)
            {
                equipTarget.EquipItem(EquipParts, slot); //slot에 들어있는 아이템을 장착해라
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
                if (oldSlot == null) //현재 장비가 되지 않은 상황이면 
                {
                    EquipItem(target, slot); //장비해라
                }
                else //장비 되어있으면 
                {
                    UnEquipItem(target, oldSlot); //우선 장비해제
                    if (oldSlot != slot)// 만약 선택된 슬롯이 이전과 다르다면
                    {
                        EquipItem(target, slot);// 다른 아이템을 선택한 것이므로 현재클릭한 슬롯의 아이템을 장착해라
                    }
                }
            }
        }
    }


 
}
