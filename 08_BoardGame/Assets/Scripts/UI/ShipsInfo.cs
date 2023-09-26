using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipsInfo : MonoBehaviour
{
    TextMeshProUGUI[] texts;
    public PlayerBase Player;
    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        //texts = new TextMeshProUGUI[5];
        //int i = 1;
        //int j = 0;
        //while(j < texts.Length)
        //{
        //    texts[j] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        //    i += 2;
        //    j++;
        //}
    }
    private void Start()
    {
        for (int i = 0; i < Player.Ships.Length; i++)
        {
            Ship ship = Player.Ships[i];

            PrintHP(texts[i], ship);
            int index = i;
            Player.Ships[i].onHit += (ship) => PrintHP(texts[index], ship);
            Player.Ships[i].onSinking += (_) => PrintSinking(texts[index]);

        }
        
        //else if (enemy != null)
        //{
        //    for (int i = 0; i < enemy.Ships.Length; i++)
        //    {
        //        Ship ship = enemy.Ships[i];
        //        enemy.Ships[i].on_HpChange += UpdateText;
        //    }
        //}
    }

    void PrintHP(TextMeshProUGUI text, Ship ship)
    {
        text.text = $"{ship.HP} / {ship.Size}";
    }

    void PrintSinking(TextMeshProUGUI text)
    {
        text.fontSize = 45;
        text.text = "<#ff0000>Destroy!!</color>";
    }
    //void UpdateText(int hp, ShipType type)
    //{
    //    if (hp < 1)
    //    {
    //        ShipDestroyed(type);
    //    }
    //    else
    //    {
    //        texts[(int)type - 1].text = $"{hp} / {Player.Ships[(int)type - 1].Size}";
    //    }


    //}
    //void ShipDestroyed(ShipType type)
    //{
    //    texts[(int)type - 1].color = Color.red;
    //    texts[(int)type - 1].fontSize = 45;
    //    texts[(int)type - 1].text = "Destroy!";
    //}
    //함선의 HP를 현재 HP /maxHP 출력
    //침몰시 빨간컬러 Destroy 출력
}
