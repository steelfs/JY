using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - Drink", menuName = "Scriptable Object/ItemData - Drink", order = 4)]
public class ItemData_Drink : ItemData, IConsumeable
{

    [Header("���Ƿ�")]
    public uint recoveryMP_Value;
    public float duration;

    public void consume(GameObject target)
    {
        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            mana.RegenerateMana(recoveryMP_Value, duration);
        }

        //Player player = target.GetComponent<Player>();
        //if (player != null) // ������ �÷��̾�Ը� ���� �� �ִ�.
        //{
        //    player.ManaRegenerate(recoveryMP_Value, 1.0f);
        //}
    }
}
