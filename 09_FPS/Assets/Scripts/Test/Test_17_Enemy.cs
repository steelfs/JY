using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Test_17_Enemy : TestBase
{
    //public Enemy enemy;
    public int seed = 0;

    public Revolver revolver;
    public float gunPower = 5;
    private void Start()
    {
        //enemy.onDie += (target) => Debug.Log($"{target.name} DIE!");
        if (seed != -1)
        {
            Random.InitState(seed);
        }
        revolver.fireRate = 10;
        revolver.damage = gunPower;
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.transform.position = new(5.5f, 0, -2.5f);
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        agent.speed = 0;
    }

}
