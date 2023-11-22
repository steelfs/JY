using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Craft", menuName = "Scriptable Object/Item Data/ItemData - Craft", order = 5)]
public class ItemData_Craft : ItemData, IEquippable
{
    [Header("조합 전용 데이터")]
    public CraftType CraftType;
    public float successRate;
    public ItemData Critical_Success_Item;
    public uint STR;
    public uint INT;
    public uint LUK;
    public uint DEX;
    public uint attack_Point;
    public uint defence_Point;
    public float Critical_Rate;
    public float Dodge_Rate;

    float IEquippable.Critical_Rate => Critical_Rate;

    float IEquippable.Dodge_Rate => Dodge_Rate;

    uint IEquippable.STR => STR;
    uint IEquippable.INT => INT;
    uint IEquippable.LUK => LUK;
    uint IEquippable.DEX => DEX;
    uint IEquippable.ATT => attack_Point;
    uint IEquippable.DP => defence_Point;
}
