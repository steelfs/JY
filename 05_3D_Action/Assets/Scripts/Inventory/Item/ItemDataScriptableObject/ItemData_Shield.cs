using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - Shield ", menuName = "Scriptable Object/ItemData - Shield ", order = 9)]

public class ItemData_Shield : ItemData_Equip
{
    [Header("방패 데이터")]
    public float defencePower = 15.0f;

    public override EquipType EquipParts => EquipType.Shield;
}