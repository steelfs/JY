using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    uint ATT { get;}
    uint DP { get; }
    uint STR { get; }
    uint INT { get; }
    uint LUK { get; }
    uint DEX { get; }
    float Critical_Rate { get; }
    float Dodge_Rate { get; }
}
