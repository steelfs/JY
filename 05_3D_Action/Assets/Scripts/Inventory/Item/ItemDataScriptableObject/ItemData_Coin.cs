using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Data - Coin", menuName = "Scriptable Object/ItemData - Coin", order = 2)]
public class ItemData_Coin : ItemData, IConsumeable
{
    public void consume(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        if (player != null) // ������ �÷��̾�Ը� ���� �� �ִ�.
        {
            player.Money += (int)price; 
        }
    }
}
// ItemData_Food - IConsumable ����� HP Tick���� ����
// Drink ���� MP Regen���� ����
