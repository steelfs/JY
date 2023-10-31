using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Test_17_Enemy : TestBase
{
    //public Enemy enemy;

    public int seed = -1;

    public Revolver revolver;
    public float gunPower = 5;

    private void Start()
    {
        //enemy.onDie += (target) => Debug.Log($"{target.name} DIE!");
        if( seed != -1)
            Random.InitState(seed);

        //revolver.fireRate = 10;
        //revolver.damage = gunPower;
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        //enemy.transform.position = new(5.5f, 0, -2.5f);

        NavMeshAgent agent = enemy.gameObject.GetComponent<NavMeshAgent>();
        agent.speed = 0.0f;
        agent.Warp(new(5.5f, 0, -2.5f));
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        GameManager.Inst.Player.HP -= 10;
    }
    //회복 아이템 만들기
    //플레이어 HP에 따라 curve사용해서 어둡게 하기
    //HP 숫자로 표시
    //죽으면 회복아이템, 총을 랜덤하게 드랍
    protected override void Test3(InputAction.CallbackContext context)
    {
        GameManager.Inst.Player.HP += 10;
    }
}
