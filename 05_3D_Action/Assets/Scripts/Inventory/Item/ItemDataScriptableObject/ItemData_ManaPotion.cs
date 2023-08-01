using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - ManaPotion", menuName = "Scriptable Object/ItemData - ManaPotion", order = 6)]
public class ItemData_ManaPotion : ItemData, IUseable
{
    [Header("마나 포션 데이터")]
    public float mana = 30.0f;

    public bool Use(GameObject target) // 성공하면 인벤토리에서 수량 감소
    {
        bool result = false;

        if (target != null)
        {
            IMana mana = target.GetComponent<IMana>();
            if (mana != null)
            {
                mana.MP += this.mana;
                result = true;
            }
        }

        return result;
        //HP가 있는 대상에게 사용했을 때 //즉시 HP증가
    }


}
