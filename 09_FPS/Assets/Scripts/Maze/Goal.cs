using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Action on_GameClear;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           on_GameClear?.Invoke();
            //GameManager.Inst.on_GameClear?.Invoke();
            //GameManager.Inst.Player.Spawn();
        }
    }
}