using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    public float heal = 30.0f;
    protected override void OnItemConsume(Player player)
    {
        player.HP += heal;
        Destroy(this.gameObject);
    }
}
