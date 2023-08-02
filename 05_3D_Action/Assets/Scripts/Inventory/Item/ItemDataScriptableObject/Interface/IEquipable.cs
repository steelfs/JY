using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    EquipType EquipParts { get; } //이 아이템이 장착될 부위

    void EquipItem(GameObject target, InvenSlot slot);//target = 장착 대상, slot = targetSlot
    void UnEquipItem(GameObject target, InvenSlot slot);//아이템 해제

    void ToggleEquip(GameObject target, InvenSlot slot);//상황에짜라 아이템을 장착, 해제하는 함수 
}
