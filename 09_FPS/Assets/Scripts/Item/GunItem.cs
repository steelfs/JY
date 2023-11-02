using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : Item
{
    public GunType GunType;
    protected override void OnItemConsume(Player player)
    {
        player.GunChange(GunType);
    }
}
