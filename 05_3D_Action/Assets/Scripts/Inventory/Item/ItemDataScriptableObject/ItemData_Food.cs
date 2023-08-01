using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - Food", menuName = "Scriptable Object/ItemData - Food", order = 3)]
public class ItemData_Food : ItemData, IConsumeable
{
    [Header("���� ������")]
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
        //if (player != null) // ������ �÷��̾�Ը� ���� �� �ִ�.
        //{
        //    player.HealthRegenerateByTick(heal, duration, tick);
        //}
    }
}