using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Equip", menuName = "Scriptable Object/Item Data/ItemData - Equip", order = 3)]
public class ItemData_Equip : ItemData, IEquippable
{
    [Header("장비아이템 데이터")]
    public uint attackPoint;
    public uint defencePoint;
    public uint STR;
    public uint INT;
    public uint LUK;
    public uint DEX;
    public float Critical_Rate;
    public float Dodge_Rate;

    float IEquippable.Critical_Rate => Critical_Rate;
    float IEquippable.Dodge_Rate => Dodge_Rate;
    uint IEquippable.STR => STR;
    uint IEquippable.INT => INT;
    uint IEquippable.LUK => LUK;
    uint IEquippable.DEX => DEX;
    uint IEquippable.ATT => attackPoint;
    uint IEquippable.DP => defencePoint;
}
