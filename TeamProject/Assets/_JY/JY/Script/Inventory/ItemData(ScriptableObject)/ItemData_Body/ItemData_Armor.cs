using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;


[CreateAssetMenu(fileName = "New Item Data - Armor", menuName = "Scriptable Object/Item Data/ItemData - Armor", order = 9)]
public class ItemData_Armor : ItemData, IEquippable
{
    [Header("장비 타입")]
    public EquipType EquipType;
    public uint attack_Point;
    public uint defence_Point;
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
    uint IEquippable.ATT => attack_Point;
    uint IEquippable.DP => defence_Point;
}
