using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    //rotation 조정시 position 변경
    public Action<Slime> onEnemyEnter;
    public Action<Slime> onEnemyExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if (slime != null)
        {
            onEnemyEnter?.Invoke(slime);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if (slime != null)
        {
            onEnemyExit?.Invoke(slime);
        }
    }

}
