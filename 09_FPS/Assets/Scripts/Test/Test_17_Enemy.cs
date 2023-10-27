using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_17_Enemy : TestBase
{
    //public Enemy enemy;
    public int seed = 0;

    private void Start()
    {
        //enemy.onDie += (target) => Debug.Log($"{target.name} DIE!");
        if (seed != -1)
        {
            Random.InitState(seed);
        }
    }
}
