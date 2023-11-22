using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Enhancable", menuName = "Scriptable Object/Item Data/ItemData - Enhancable", order = 4)]
public class ItemData_Enhancable : ItemData_Equip, IEnhancable
{
    [Header("강화 가능 아이템 데이터")]
    public byte itemLevel;
    public EnhanceType enhanceType;

    byte priviousLevel;
    public byte Itemlevel
    {
        get => itemLevel;
        private set
        {
            if (itemLevel != value)
            {
                itemLevel = value;
            }
           
        }
    }

    public bool LevelUp(uint darkForceCount)//enhancerUI에서 호출, 
    {
        bool result = false;
        float SuccessRate = CalculateSuccessRate(darkForceCount);
        if (Random.Range(0, 100) < SuccessRate)
        {
            result = true;
        }
        return result;
    }
    public float CalculateSuccessRate(uint darkForceCount)
    {
        float finalSuccessRate = 0.0f;
        float forceBoost = darkForceCount * 0.2f;
        float levelBonus = 100 - (Itemlevel * 10);

        finalSuccessRate = Mathf.Clamp(levelBonus + forceBoost, 0.0f, 100.0f);

        return finalSuccessRate;
    }
    public void LevelUpItemStatus()// 코루틴 끝나고 실행 시킨 후 Enhancer의 State를 Clear로 바꾸기
    {
        Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        //생성자 new?
        this.attackPoint = resultAttackPoint;
        this.defencePoint = resultDefencePoint;
        this.itemName = itemname;
        priviousLevel = itemLevel;
        Itemlevel++;

        GameManager.SlotManager.RemoveItem(this, GameManager.SlotManager.Index_JustChange_Slot);
        GameManager.SlotManager.AddItem(this);
        Debug.Log("값 변경 완료");
        // Debug.Log(GameManager.SlotManager.slots[Current_Inventory_State.Equip][GameManager.SlotManager.IndexForEnhancer].ItemData);
        //이 시점에서 Slot에 이 아이템 데이터를 Assign 해줘야한다.
    }
    /// <summary>
    /// 저장관련 불러오기 로직에서 사용할 레벨업 장비 렙업 시키는 함수 
    /// </summary>
    /// <param name="slot"></param>
    public void LevelUpItemStatus(Slot slot)// 코루틴 끝나고 실행 시킨 후 Enhancer의 State를 Clear로 바꾸기
    {
        Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        //생성자 new?
        this.attackPoint = resultAttackPoint;
        this.defencePoint = resultDefencePoint;
        this.itemName = itemname;
        priviousLevel = itemLevel;
        Itemlevel++;

        GameManager.SlotManager.RemoveItem(this, slot.Index);
        slot.AssignSlotItem(this);
        //Debug.Log("값 변경 완료");
        // Debug.Log(GameManager.SlotManager.slots[Current_Inventory_State.Equip][GameManager.SlotManager.IndexForEnhancer].ItemData);
        //이 시점에서 Slot에 이 아이템 데이터를 Assign 해줘야한다.
    }

    public void Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemName)
    {
        float increaseRatio = 0.3f;
        uint increaseAttackValue = (uint)(this.attackPoint * increaseRatio + (Itemlevel * 2));
        uint increaseDefenceValue = (uint)(this.defencePoint * increaseRatio + (Itemlevel * 2));

        itemName = this.itemName + "★";
        resultAttackPoint = this.attackPoint + increaseAttackValue;
        resultDefencePoint = this.defencePoint + increaseDefenceValue;
    }
    void LevelDownItemStatus()
    {
        float decreaseRatio = 0.3f; // 레벨업 때 사용한 비율과 동일
        uint decreaseAttackValue = (uint)(attackPoint * decreaseRatio * (Itemlevel * 0.5f));
        uint decreaseDefenceValue = (uint)(defencePoint * decreaseRatio * (Itemlevel * 0.5f));

        // 감소량이 현재 점수보다 크지 않도록 한다.
        decreaseAttackValue = (uint)Mathf.Min(attackPoint, decreaseAttackValue);
        decreaseDefenceValue = (uint)Mathf.Min(defencePoint, decreaseDefenceValue);

        attackPoint -= decreaseAttackValue;
        defencePoint -= decreaseDefenceValue;
    }
 
}
