using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EquipType
{
    None,
    Hat,
    Weapon,
    Body,
    Jewel
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 데이터")]
    public ItemType ItemType;
    public ItemCode code;                      
    public string itemName;         
    public GameObject modelPrefab;
    public Sprite itemIcon;                   
    public uint price = 0;                    
    public uint maxStackCount = 1;            
    public string itemDescription;

    public Gyu_QuestBaseData[] questBinding = null;
   
    public void ItemCountBinding(int bindCount) 
    {
        if (questBinding != null) 
        {
            foreach (var item in questBinding)
            {
                item.SetCounting(bindCount,code);
            }
        }
    }
}
