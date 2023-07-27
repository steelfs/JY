using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - Coin", menuName = "Scriptable Object/ItemData - Coin", order = 2)]
public class ItemData_Coin : ItemData, IConsumeable
{
    public void consume(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        if (player != null) // 동전은 플레이어에게만 사용될 수 있다.
        {
            player.Money += (int)price; 
        }
    }
}
