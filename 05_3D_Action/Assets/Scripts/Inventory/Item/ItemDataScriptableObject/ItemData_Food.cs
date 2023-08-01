using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - Food", menuName = "Scriptable Object/ItemData - Food", order = 3)]
public class ItemData_Food : ItemData, IConsumeable
{
    [Header("음식 아이템")]
    public float recoveryHP_Value;
    public float duration;
    public uint tick;

    public void consume(GameObject target)
    {
        IHealth Health = target.GetComponent<IHealth>();
        if (Health != null)
        {
            Health.HealthRegenerateByTick(recoveryHP_Value, duration, tick);
        }
        //Player player = target.GetComponent<Player>();
        //if (player != null) // 동전은 플레이어에게만 사용될 수 있다.
        //{
        //    player.HealthRegenerateByTick(heal, duration, tick);
        //}
    }
}