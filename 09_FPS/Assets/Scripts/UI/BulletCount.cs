using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    TextMeshProUGUI bulletCount;

    private void Awake()
    {
        bulletCount = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        GameManager.Inst.Player.Gun.on_BulletCountChange = OnBulletCountChange;
        foreach(var gun in GameManager.Inst.Player.Guns)
        {
            gun.on_BulletCountChange += OnBulletCountChange;
        }
    }

    private void OnBulletCountChange(int count)
    {
        bulletCount.text = count.ToString();
    }
}
