using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - HealingPotion", menuName = "Scriptable Object/ItemData - HealingPotion", order = 5)]
public class ItemData_HealingPotion : ItemData, IUseable
{
    [Header("힐링 포션 데이터")]
    public float heal = 30.0f;

    public bool Use(GameObject target) // 성공하면 인벤토리에서 수량 감소
    {
        bool result = false;

        if (target != null)
        {
            IHealth health = target.GetComponent<IHealth>();
            if (health != null)
            {
                if (health.HP < health.MaxHP)
                {
                    health.HP += heal;
                    result = true;
                }
                else
                {
                    result = false;
                    Debug.Log("HP 가 가득 차 있습니다.");
                }
            }
        }

        return result;
        //HP가 있는 대상에게 사용했을 때 //즉시 HP증가
    }


}
