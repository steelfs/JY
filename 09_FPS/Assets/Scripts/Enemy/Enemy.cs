using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //플레이어의 총에 맞으면 죽는다.
    public float hp = 30;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                Die();
            }
        }
    }
    public float maxHp = 30;

    public Action<Enemy> on_Die;

    private void OnEnable()
    {
        HP = maxHp;
    }
    void Die()
    {
        on_Die?.Invoke(this);
        this.gameObject.SetActive(false);
    }

    //public Action on_DecreaseEnemyCount;
    //protected override void OnDisable()
    //{
    //    on_DecreaseEnemyCount?.Invoke();
    //    base.OnDisable();
    //}
    //private void Awake()
    //{
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        this.gameObject.SetActive(false);
    //    }
    //}
}
