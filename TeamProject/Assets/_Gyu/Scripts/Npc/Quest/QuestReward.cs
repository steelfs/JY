using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReward
{
    public enum ItemCodes// 밑의 ItemData 배열과 같은 순서를 유지해야 알맞은 프리팹을 소환한다.
    {
        Cash,
        HpPotion,
        MpPotion,
        SecretPotion,
        SpellBook,
        Shield,
        Shield_Extended,
        Bat,
        Bow,
        Enhancable_Bow,
        Dagger,
        Hammer,
        Pistol,
        Enhancable_Pistol,
        Rifle,
        Enhancable_Rifle,
        Scythe,
        ShotGun,
        Enhancable_shotGun,
        SwordLaser,
        SwordLaser_Advanced,
        Enhancable_Sword,
        TwoHandAxe,
        TwoHandSword,
        Wand,
        AquaCrystal,
        BlueCrystal,
        Intermidiate_Blue_Crystal,
        Advanced_Blue_Crystal,
        DarkCrystal,
        Intermidiate_Dark_Crystal,
        Advanced_Dark_Crystal,
        Green_Crystal,
        Intermidiate_Green_Crystal,
        Advanced_Green_Crystal,
        Pink_Crystal,
        Purple_Crystal,
        Red_Crystal,
        Intermidiate_Red_Crystal,
        Advanced_Red_Crystal,
        Unknown_Crystal,
        Intermidiate_Unknown_Crystal,
        Advanced_Unknown_Crystal,
        Yellow_Crystal,
        Bullet_Default,
        Bullet_Grade1,
        Bullet_Grade2,
        Bullet_Grade3,
        Bullet_Grade4,
        Bullet_Grade5,
        Bullet_Grade6,
        Bullet_Grade7,
        Bullet_Grade8,
        Bullet_Grade9,
        Bullet_Grade10,
        Bullet_Grade11,
        Bullet_Grade12,
        Bullet_Grade13
    }

    public ItemCodes RewardItem;
}
