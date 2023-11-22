using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Enhancable", menuName = "Scriptable Object/Item Data/ItemData - Enhancable", order = 4)]
public class ItemData_Enhancable : ItemData_Equip, IEnhancable
{
    [Header("��ȭ ���� ������ ������")]
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

    public bool LevelUp(uint darkForceCount)//enhancerUI���� ȣ��, 
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
    public void LevelUpItemStatus()// �ڷ�ƾ ������ ���� ��Ų �� Enhancer�� State�� Clear�� �ٲٱ�
    {
        Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        //������ new?
        this.attackPoint = resultAttackPoint;
        this.defencePoint = resultDefencePoint;
        this.itemName = itemname;
        priviousLevel = itemLevel;
        Itemlevel++;

        GameManager.SlotManager.RemoveItem(this, GameManager.SlotManager.Index_JustChange_Slot);
        GameManager.SlotManager.AddItem(this);
        Debug.Log("�� ���� �Ϸ�");
        // Debug.Log(GameManager.SlotManager.slots[Current_Inventory_State.Equip][GameManager.SlotManager.IndexForEnhancer].ItemData);
        //�� �������� Slot�� �� ������ �����͸� Assign ������Ѵ�.
    }
    /// <summary>
    /// ������� �ҷ����� �������� ����� ������ ��� ���� ��Ű�� �Լ� 
    /// </summary>
    /// <param name="slot"></param>
    public void LevelUpItemStatus(Slot slot)// �ڷ�ƾ ������ ���� ��Ų �� Enhancer�� State�� Clear�� �ٲٱ�
    {
        Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemname);

        //������ new?
        this.attackPoint = resultAttackPoint;
        this.defencePoint = resultDefencePoint;
        this.itemName = itemname;
        priviousLevel = itemLevel;
        Itemlevel++;

        GameManager.SlotManager.RemoveItem(this, slot.Index);
        slot.AssignSlotItem(this);
        //Debug.Log("�� ���� �Ϸ�");
        // Debug.Log(GameManager.SlotManager.slots[Current_Inventory_State.Equip][GameManager.SlotManager.IndexForEnhancer].ItemData);
        //�� �������� Slot�� �� ������ �����͸� Assign ������Ѵ�.
    }

    public void Calculate_LevelUp_Result_Value(out uint resultAttackPoint, out uint resultDefencePoint, out string itemName)
    {
        float increaseRatio = 0.3f;
        uint increaseAttackValue = (uint)(this.attackPoint * increaseRatio + (Itemlevel * 2));
        uint increaseDefenceValue = (uint)(this.defencePoint * increaseRatio + (Itemlevel * 2));

        itemName = this.itemName + "��";
        resultAttackPoint = this.attackPoint + increaseAttackValue;
        resultDefencePoint = this.defencePoint + increaseDefenceValue;
    }
    void LevelDownItemStatus()
    {
        float decreaseRatio = 0.3f; // ������ �� ����� ������ ����
        uint decreaseAttackValue = (uint)(attackPoint * decreaseRatio * (Itemlevel * 0.5f));
        uint decreaseDefenceValue = (uint)(defencePoint * decreaseRatio * (Itemlevel * 0.5f));

        // ���ҷ��� ���� �������� ũ�� �ʵ��� �Ѵ�.
        decreaseAttackValue = (uint)Mathf.Min(attackPoint, decreaseAttackValue);
        decreaseDefenceValue = (uint)Mathf.Min(defencePoint, decreaseDefenceValue);

        attackPoint -= decreaseAttackValue;
        defencePoint -= decreaseDefenceValue;
    }
 
}
