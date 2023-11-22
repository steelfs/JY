using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Cash", menuName = "Scriptable Object/Item Data/ItemData - Cash", order = 2)]
public class ItemData_Cash : ItemData, IConsumable
{
    public void Consume(GameObject target)
    {
        Player_ player = target.GetComponent<Player_>();
        if (player != null)
        {
            //player.Money += (int)price;
        }
    }


}
