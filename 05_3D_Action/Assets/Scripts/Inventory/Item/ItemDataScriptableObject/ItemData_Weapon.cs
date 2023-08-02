using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - Weapon ", menuName = "Scriptable Object/ItemData - Weapon ", order = 8)]

public class ItemData_Weapon : ItemData_Equip
{
    [Header("���� ������")]
    public float attackPower = 30.0f;
}
