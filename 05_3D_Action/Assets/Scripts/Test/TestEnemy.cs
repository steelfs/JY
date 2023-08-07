using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class TestEnemy : TestBase
{
    public Enemy enemy;
    NavMeshAgent agent;

    public Transform moveTarget;
    private void Start()
    {
        agent = enemy.GetComponent<NavMeshAgent>();
    }
    protected override void Test1(InputAction.CallbackContext context)
    {
        agent.SetDestination(moveTarget.position);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        enemy.HP -= 30;
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        enemy.HP += 30;
    }
}
