using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Potion", menuName = "Scriptable Object/Item Data/ItemData - Potion", order = 7)]
public class ItemData_Potion : ItemData
{
    [Header("포션 전용 데이터")]
    public float duration;
    public int recoveryValue;
    
    public void Consume(Player_ target)
    {
        Player_ player = target.GetComponent<Player_>();
        player.Play_PotionSound();
        switch (code)
        {
            case ItemCode.HpPotion:
                player.Player_Status.Recovery_HP(recoveryValue, duration);
                break;
            case ItemCode.MpPotion:
                player.Player_Status.Recovery_Stamina(recoveryValue, duration);
                break;
            case ItemCode.SecretPotion:
                player.Player_Status.Recovery_HP(recoveryValue, duration);
                player.Player_Status.Recovery_Stamina(recoveryValue, duration);
                break;
        }
    }

}
