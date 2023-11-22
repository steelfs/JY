using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item_Mixing_Table : MonoBehaviour
{
    Dictionary<ItemCode, ItemCode[]> combinationTable = new Dictionary<ItemCode, ItemCode[]>();
    private void Start()
    {
        Init();
    }
    void Init()//조합테이블 생성
    {
        combinationTable.Add(ItemCode.Intermidiate_Blue_Crystal, new ItemCode[2] { ItemCode.BlueCrystal, ItemCode.BlueCrystal });
        combinationTable.Add(ItemCode.Intermidiate_Dark_Crystal, new ItemCode[2] { ItemCode.DarkCrystal, ItemCode.DarkCrystal });
        combinationTable.Add(ItemCode.Intermidiate_Green_Crystal, new ItemCode[2] { ItemCode.Green_Crystal, ItemCode.Green_Crystal});
        combinationTable.Add(ItemCode.Intermidiate_Red_Crystal, new ItemCode[2] { ItemCode.Red_Crystal, ItemCode.Red_Crystal });
        combinationTable.Add(ItemCode.Intermidiate_Unknown_Crystal, new ItemCode[2] { ItemCode.Unknown_Crystal, ItemCode.Unknown_Crystal });
        combinationTable.Add(ItemCode.Advanced_Blue_Crystal, new ItemCode[2] { ItemCode.Intermidiate_Blue_Crystal, ItemCode.Intermidiate_Blue_Crystal });
        combinationTable.Add(ItemCode.Advanced_Red_Crystal, new ItemCode[2] { ItemCode.Intermidiate_Red_Crystal, ItemCode.Intermidiate_Red_Crystal });
        combinationTable.Add(ItemCode.Advanced_Dark_Crystal, new ItemCode[2] { ItemCode.Intermidiate_Dark_Crystal, ItemCode.Intermidiate_Dark_Crystal });
        combinationTable.Add(ItemCode.Advanced_Green_Crystal, new ItemCode[2] { ItemCode.Intermidiate_Green_Crystal, ItemCode.Intermidiate_Green_Crystal });
        combinationTable.Add(ItemCode.Advanced_Unknown_Crystal, new ItemCode[2] { ItemCode.Intermidiate_Unknown_Crystal, ItemCode.Intermidiate_Unknown_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade13, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Unknown_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade11, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Red_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade10, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.BlueCrystal });
        combinationTable.Add(ItemCode.Bullet_Grade9, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Intermidiate_Unknown_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade8, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Green_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade7, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Intermidiate_Red_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade6, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Intermidiate_Blue_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade5, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Intermidiate_Green_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade4, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Advanced_Green_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade3, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Advanced_Red_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade2, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Advanced_Blue_Crystal });
        combinationTable.Add(ItemCode.Bullet_Grade1, new ItemCode[2] { ItemCode.Bullet_Default, ItemCode.Advanced_Dark_Crystal });
    }
    public bool ValidData(ItemCode leftCode, ItemCode middleCode, out ItemCode resultCode)
    {
        bool result = false;
        resultCode = ItemCode.Advanced_Dark_Crystal;//의미없음 null 이 안되기 때문에 임의데이터 부여 
        foreach (KeyValuePair<ItemCode, ItemCode[]> entry in combinationTable)
        {
            ItemCode matchedCode = entry.Key;
            ItemCode[] ingredients = entry.Value;
            //조합테이블의 코드와 파라미터로 받은 코드가 순서에 관계없이 일치하다면 
            if (leftCode == ingredients[0] && middleCode == ingredients[1] || leftCode == ingredients[1] && middleCode == ingredients[0])
            {
                result = true;
                resultCode = matchedCode;
                break;
            }
        }
        return result;
    }
    public float CalculateSuccessRate(ItemData item,uint darkForceCount)
    {
        float baseRate = 0.0f;
        ItemData_Craft crystal = item as ItemData_Craft;
        ItemData_Bullet bullet = item as ItemData_Bullet;
        if (crystal != null)
        {
            baseRate = crystal.successRate;
        }
        else if (bullet != null)
        {
            baseRate = bullet.successRate;
        }
        float forceBoost = darkForceCount * 0.2f;
        float finalSuccessRate = baseRate + forceBoost;
   

        finalSuccessRate = Mathf.Clamp(finalSuccessRate, 0.0f, 100.0f);

        return finalSuccessRate;
    }
    public bool LevelUp(ItemData item, uint darkForceCount, out bool critical)
    {
        GameManager.PlayerStatus.Base_Status.Base_DarkForce -= darkForceCount;
        bool result = false;

        float successRate = CalculateSuccessRate(item, darkForceCount);
        float criticalRate = successRate * 0.1f;

        ItemData_Craft craftable = item as ItemData_Craft;
        if (craftable.Critical_Success_Item != null)
        {
            if (UnityEngine.Random.Range(0, 100) < criticalRate)
            {
                critical = true;
                result = true;
            }
            else if (UnityEngine.Random.Range(0, 100) < successRate)
            {
                critical = false;
                result = true;
            }
            else
            {
                critical = false;
                result = false;
            }
        }
        else if (UnityEngine.Random.Range(0, 100) < successRate)
        {
            critical = false;
            result = true;
        }
        else
        {
            critical = false;
            result = false;
        }

        return result;
    }
}
